using System;
using System.Globalization;
using ConsoleGUI.Data;
using Lanchat.ClientCore;

namespace Lanchat.Terminal.UserInterface
{
    public static class Theme
    {
        public static bool NeedTrueColor { get; set; }
        public static Color Foreground { get; set; }
        public static Color Background { get; set; }
        public static Color Borders { get; set; }
        public static Color TabActive { get; set; }
        public static Color TabInactive { get; set; }
        public static Color TabAttentionNeeded { get; set; }
        public static Color LogHour { get; set; }
        public static Color LogNickname { get; set; }
        public static Color LogMessage { get; set; }
        public static Color LogStatus { get; set; }
        public static Color LogWarning { get; set; }
        public static Color LogError { get; set; }
        public static Color StatusOnline { get; set; }
        public static Color StatusAfk { get; set; }
        public static Color StatusDnd { get; set; }

        public static void LoadFromConfig()
        {
            var theme = Storage.LoadTheme(Program.Config.Theme);
            if (theme == null)
            {
                return;
            }

            NeedTrueColor = theme.NeedTrueColor;
            Foreground = ConvertColor(theme.Foreground) ?? ConsoleColor.White;
            Background = ConvertColor(theme.Background) ?? ConsoleColor.Black;
            Borders = ConvertColor(theme.Borders) ?? ConsoleColor.White;
            TabActive = ConvertColor(theme.TabActive) ?? ConsoleColor.Blue;
            TabInactive = ConvertColor(theme.TabInactive) ?? ConsoleColor.Black;
            TabAttentionNeeded = ConvertColor(theme.TabAttentionNeeded) ?? ConsoleColor.Green;
            LogHour = ConvertColor(theme.LogHour) ?? ConsoleColor.White;
            LogNickname = ConvertColor(theme.LogNickname) ?? ConsoleColor.Blue;
            LogMessage = ConvertColor(theme.LogMessage) ?? ConsoleColor.White;
            LogStatus = ConvertColor(theme.LogStatus) ?? ConsoleColor.Cyan;
            LogWarning = ConvertColor(theme.LogWarning) ?? ConsoleColor.Yellow;
            LogError = ConvertColor(theme.LogError) ?? ConsoleColor.Red;
            StatusOnline = ConvertColor(theme.StatusOnline) ?? ConsoleColor.White;
            StatusAfk = ConvertColor(theme.StatusAfk) ?? ConsoleColor.Yellow;
            StatusDnd = ConvertColor(theme.StatusDnd) ?? ConsoleColor.Red;
        }

        private static Color? ConvertColor(string hexColor)
        {
            try
            {
                var r = Convert.ToByte(int.Parse(hexColor[..2], NumberStyles.HexNumber));
                var g = Convert.ToByte(int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber));
                var b = Convert.ToByte(int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber));
                return new Color(r, g, b);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}