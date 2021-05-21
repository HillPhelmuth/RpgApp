using System.Linq;

namespace RpgComponentLibrary.Services
{
    public static class ExtensionMethods
    {
        public static string RemoveWhitespace(this string input)
        {
            return new(input.Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
