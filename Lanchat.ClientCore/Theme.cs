#pragma warning disable 1591

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Lanchat.Terminal theme model.
    /// </summary>
    public class ThemeModel
    {
        public bool NeedTrueColor { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
        public string Borders { get; set; }
        public string TabActive { get; set; }
        public string TabInactive { get; set; }
        public string TabAttentionNeeded { get; set; }
        public string LogHour { get; set; }
        public string LogNickname { get; set; }
        public string LogMessage { get; set; }
        public string LogStatus { get; set; }
        public string LogWarning { get; set; }
        public string LogError { get; set; }
        public string StatusOnline { get; set; }
        public string StatusAfk { get; set; }
        public string StatusDnd { get; set; }
    }
}