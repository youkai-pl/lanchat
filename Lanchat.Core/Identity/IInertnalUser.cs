namespace Lanchat.Core.Identity
{
    internal interface IInternalUser
    {
        string Nickname { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}