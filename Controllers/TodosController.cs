using Microsoft.AspNetCore.Mvc;
using TodoApi.Helpers;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private static readonly List<Todo> _todos = new();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Title))
            {
                return BadRequest("Title is required.");
            }

            if (!HashHelper.IsValidProof(todo.Title.Trim(), todo.Nonce, todo.Proof))
            {
                return BadRequest("Invalid proof of work.");
            }

            var previousHash = _todos.Count > 0 ? _todos.Last().Hash : "GENESIS";

            var newTodo = new Todo
            {
                Id = Guid.NewGuid(),
                Title = todo.Title.Trim(),
                Completed = todo.Completed,
                CreatedAt = DateTime.UtcNow,
                PreviousHash = previousHash,
                Nonce = todo.Nonce,
                Proof = todo.Proof
            };

            newTodo.Hash = HashHelper.ComputeHash(newTodo, newTodo.PreviousHash);

            _todos.Add(newTodo);

            return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Todo updatedTodo)
        {
            if (string.IsNullOrWhiteSpace(updatedTodo.Title))
            {
                return BadRequest("Title is required.");
            }

            if (!HashHelper.IsValidProof(updatedTodo.Title.Trim(), updatedTodo.Nonce, updatedTodo.Proof))
            {
                return BadRequest("Invalid proof of work.");
            }

            var index = _todos.FindIndex(t => t.Id == id);

            if (index == -1)
            {
                return NotFound();
            }

            _todos[index].Title = updatedTodo.Title.Trim();
            _todos[index].Completed = updatedTodo.Completed;
            _todos[index].Nonce = updatedTodo.Nonce;
            _todos[index].Proof = updatedTodo.Proof;

            RebuildChainFrom(index);

            return Ok(_todos[index]);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = _todos.FindIndex(t => t.Id == id);

            if (index == -1)
            {
                return NotFound();
            }

            _todos.RemoveAt(index);

            if (_todos.Count > 0 && index < _todos.Count)
            {
                RebuildChainFrom(index);
            }

            return NoContent();
        }

        [HttpGet("verify")]
        public IActionResult Verify()
        {
            for (int i = 0; i < _todos.Count; i++)
            {
                var expectedPreviousHash = i == 0 ? "GENESIS" : _todos[i - 1].Hash;

                if (_todos[i].PreviousHash != expectedPreviousHash)
                {
                    return Conflict(new
                    {
                        message = "Chain Tampered",
                        index = i
                    });
                }

                var recalculatedHash = HashHelper.ComputeHash(_todos[i], _todos[i].PreviousHash);

                if (_todos[i].Hash != recalculatedHash)
                {
                    return Conflict(new
                    {
                        message = "Chain Tampered",
                        index = i
                    });
                }
            }

            return Ok(new { message = "Chain Valid" });
        }

        private void RebuildChainFrom(int startIndex)
        {
            for (int i = startIndex; i < _todos.Count; i++)
            {
                _todos[i].PreviousHash = i == 0 ? "GENESIS" : _todos[i - 1].Hash;
                _todos[i].Hash = HashHelper.ComputeHash(_todos[i], _todos[i].PreviousHash);
            }
        }
    }
}