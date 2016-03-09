using System;
using System.Text.RegularExpressions;
using WinAppDeploy.GUI.Utils;

namespace WinAppDeploy.GUI.Extensions
{
    public static class ConsoleOutputExtensions
    {
        #region General

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

        #endregion General

        #region devices cmd

        public static bool IsDeviceInfo(this string input)
        {
            var pattern = string.Concat("^", RegexHelper.IpPattern); // Start by an IP
            var final = Regex.IsMatch(input, pattern);

            return final;
        }

        #endregion devices cmd

        #region list cmd

        public static string CleanListingHeader(this string input)
        {
            var pattern = "Remote action succeeded.";
            var finalposition = input.LastIndexOf(pattern, StringComparison.Ordinal);

            var final = input.Remove(0, finalposition + pattern.Length);
            return final.TrimStart();

            //var pattern = "(.*\\s)*(Remote action succeeded\\.)";
            //var final = Regex.Replace(input, pattern, "");

            //return final;
        }

        public static string CleanListingFooter(this string input)
        {
            var pattern = "(Cleaning up remote target components)(.*\\s)*";
            var final = Regex.Replace(input, pattern, "");

            return final;
        }

        #endregion list cmd
    }
}