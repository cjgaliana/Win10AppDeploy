using System.Text.RegularExpressions;

namespace WinAppDeploy.GUI.Extensions
{
    public static class ConsoleOutputExtensions
    {
        public static string CleanHeader(this string input)
        {
            var pattern = "^((.)*\\s){4}";
            var final = Regex.Replace(input, pattern, "");

            return final;
        }

        public static string CleanFooter(this string input)
        {
            var pattern = "Done(\\.)?(\\s)?$"; //Done at the end of the string
            var final = Regex.Replace(input, pattern, "");

            return final;
        }
    }
}