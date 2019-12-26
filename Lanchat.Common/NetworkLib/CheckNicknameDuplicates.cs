namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        // Check nickname duplicates
        public void CheckNickcnameDuplicates(string nickname)
        {
            var users = NodeList.FindAll(x => x.ClearNickname == nickname);
            if (users.Count > 1)
            {
                var index = 1;
                foreach (var item in users)
                {
                    item.NicknameNum = index;
                    index++;
                }
            }
            else if (users.Count > 0)
            {
                users[0].NicknameNum = 0;
            }
        }
    }
}
