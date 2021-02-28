using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
