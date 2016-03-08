using System.Text.RegularExpressions;
using WinAppDeploy.GUI.Utils;

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

        public static bool IsDeviceInfo(this string input)
        {
            var pattern = string.Concat("^", RegexHelper.IpPattern); // Start by an IP
            var final = Regex.IsMatch(input, pattern);

            return final;
        }
    }
}