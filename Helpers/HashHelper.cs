using System.Security.Cryptography;
using System.Text;
using TodoApi.Models;

namespace TodoApi.Helpers
{
    public static class HashHelper
    {
        public static string ComputeHash(Todo todo, string previousHash)
        {
            var rawData =
                $"{todo.Id}|{todo.Title}|{todo.Completed}|{todo.CreatedAt:O}|{previousHash}";

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            return Convert.ToHexString(bytes);
        }

        public static string ComputeProof(string title, int nonce)
        {
            var rawData = $"{title}|{nonce}";

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            return Convert.ToHexString(bytes);
        }

        public static bool IsValidProof(string title, int nonce, string proof)
        {
            var computed = ComputeProof(title, nonce);
            return computed.Equals(proof, StringComparison.OrdinalIgnoreCase)
                   && computed.StartsWith("00", StringComparison.OrdinalIgnoreCase);
        }
    }
}