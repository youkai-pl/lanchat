namespace Lanchat.Core.Models
{
    public class StatusUpdate
    {
        public Status NewStatus { get; set; }
    }

    public enum Status
    {
        Online,
        AwayFromKeyboard,
        DoNotDisturb
    }
}