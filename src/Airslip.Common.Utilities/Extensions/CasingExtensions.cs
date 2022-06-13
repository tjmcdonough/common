using Airslip.Common.Types;
using System.Linq;
using System.Text.RegularExpressions;

namespace Airslip.Common.Utilities.Extensions;

public static class CasingExtensions
{
    public static string ToCamelCase(this string str) => _toCase(str, Casing.CAMEL_CASE);

    public static string ToPascalCase(this string original) => _toCase(original, Casing.PASCAL_CASE);

    public static string ToSpacedPascalCase(this string original) => _toCase(original, Casing.SPACED_PASCAL_CASE);

    public static string ToSnakeCase(this string str) =>
        string.Concat(str.Replace(" ", "").Select((x, i) =>
            i > 0 && char.IsUpper(x) && !char.IsUpper(str[i - 1]) ? $"_{x}" : x.ToString())).ToLower();

    public static string ToKebabCasing(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(
                input.Replace("_", "-"),
                @"(?<!^)(?<!-)((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))",
                "-$1")
            .ToLower()
            .Replace(" ", "-")
            .Replace("--", "-");
    }

    private static string _toCase(this string str, Casing casing)
    {
        string newString = string.Empty;
        bool makeNextCharacterUpper = false;
        for (int index = 0; index < str.Length; index++)
        {
            char c = str[index];
            if (index == 0)
                newString += _caseFirstCharacter(c, casing);
            else if (makeNextCharacterUpper)
            {
                newString += $"{char.ToUpper(c)}";
                makeNextCharacterUpper = false;
            }
            else if (char.IsUpper(c))
                newString += $" {c}";
            else if (char.IsLower(c) || char.IsNumber(c))
                newString += c;
            else if (char.IsNumber(c))
                newString += $"{c}";
            else
            {
                makeNextCharacterUpper = true;
                newString += ' ';
            }
        }

        newString = newString.TrimStart();
        return casing switch
        {
            Casing.CAMEL_CASE => newString.Replace(" ", ""),
            Casing.SPACED_PASCAL_CASE => newString,
            Casing.PASCAL_CASE => newString.Replace(" ", ""),
            _ => newString
        };
    }

    private static string _caseFirstCharacter(char character, Casing casing)
    {
        return casing switch
        {
            Casing.CAMEL_CASE => $"{char.ToLower(character)}",
            Casing.PASCAL_CASE => $"{char.ToUpper(character)}",
            Casing.SPACED_PASCAL_CASE => $"{char.ToUpper(character)}",
            _ => character.ToString()
        };
    }
}