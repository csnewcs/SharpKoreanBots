namespace SharpKoreanBots.User
{
    using SharpKoreanBots.Bot;

    public struct UserInfo
    {
        ulong _id;
        string _name;
        int _tag;
        string _github;
        UserFlag _flag;
        BotInfo[] _bots;
        public UserInfo(ulong id, string name = null, int tag = 0, string github = null, UserFlag flag = UserFlag.Normal, BotInfo[] bots = null)
        {
            _id = id;
            _name = name;
            _tag = tag;
            _github = github;
            _flag = flag;
            _bots = bots;
        }
    }
    public enum UserFlag
    {
        Normal,
        Admin,
        BugHunter,
        Reviewer,
        Premium
    }
}