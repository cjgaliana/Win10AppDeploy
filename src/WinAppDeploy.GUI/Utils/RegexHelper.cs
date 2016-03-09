namespace WinAppDeploy.GUI.Utils
{
    public class RegexHelper
    {
        public static string GuidPattern =
            "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})";

        public static string IpPattern = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";
    }
}