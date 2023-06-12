using System.Text;

namespace Sirena.Travel.TestTask.Mapping
{
    internal static class Extensions
    {
        public static Guid GenerateGuid<T>(this T src) where T : notnull
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(src.ToString() ?? string.Empty);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }
}
