using System;
using System.Security.Cryptography;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Salted PBKDF2-SHA256 password hashing. Stored format:
    ///   pbkdf2$&lt;iterations&gt;$&lt;base64 salt&gt;$&lt;base64 hash&gt;
    /// Plain-text passwords are never stored.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "pbkdf2";
        private const int SaltSize = 16;
        private const int HashSize = 32;
        public const int DefaultIterations = 100000;

        public static string Hash(string password)
        {
            return Hash(password, DefaultIterations);
        }

        public static string Hash(string password, int iterations)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            byte[] hash = Derive(password, salt, iterations);
            return string.Join("$", Prefix, iterations.ToString(),
                Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string stored)
        {
            if (password == null || string.IsNullOrEmpty(stored)) return false;

            string[] parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != Prefix) return false;

            int iterations;
            if (!int.TryParse(parts[1], out iterations) || iterations <= 0) return false;

            byte[] salt, expected;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actual = Derive(password, salt, iterations, expected.Length);
            return FixedTimeEquals(actual, expected);
        }

        private static byte[] Derive(string password, byte[] salt, int iterations, int size = HashSize)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                return pbkdf2.GetBytes(size);
        }

        // Compares in constant time so the check does not leak timing information.
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
