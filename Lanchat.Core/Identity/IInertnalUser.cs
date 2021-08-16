namespace Lanchat.Core.Identity
{
    internal interface IInternalUser
    {
        string Nickname { get; set; }
        UserStatus UserStatus { get; set; }
    }
}