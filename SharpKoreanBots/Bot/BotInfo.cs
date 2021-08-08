namespace SharpKoreanBots.Bot
{
    using SharpKoreanBots.User;
    public struct BotInfo
    {
        ulong _id;
        public ulong ID
        {
            get {return _id;}
        }
        BotFlag _flag;
        public BotFlag Flag
        {
            get {return _flag;}
        }
        string _library;
        public string Library
        {
            get {return _library;}
        }
        string _prefix;
        public string Prefix
        {
            get {return _prefix;}
        }
        int _votes;
        public int Votes
        {
            get {return _votes;}
        }
        int _serverCount;
        public int ServerCount
        {
            get {return _serverCount;}
            set {_serverCount = value;}
        }
        public int _shardCount;
        public int ShardCount
        {
            get {return _shardCount;}
            set {_shardCount = value;}
        }
        string _intro;
        public string Intro
        {
            get {return _intro;}
        }
        string _description;
        public string Description
        {
            get {return _description;}
        }
        string _website;
        public string Website
        {
            get {return _website;}
        }
        string _github;
        public string Github
        {
            get {return _github;}
        }
        string _discord;
        public string Discord
        {
            get {return _discord;}
        }
        BotStatus _status;
        public BotStatus Status
        {
            get {return _status;}
        }
        string _name;
        public string Name
        {
            get {return _name;}
        }
        int _tag;
        public int Tag
        {
            get {return _tag;}
        }
        string _avatar;
        public string Avatar
        {
            get {return _avatar;}
        }
        public BotInfo(ulong id, string name, int tag, BotFlag flag, string prefix, int votes, string intro, string description, string library, BotStatus status, int serverCount = 0, int shardCount = 1, string website = null, string github = null, string discord = null, string avatar = null)
        {
            _id = id;
            _name = name;
            _tag = tag;
            _flag = flag;
            _prefix = prefix;
            _votes = votes;
            _intro = intro;
            _description = description;
            _library = library;
            _status = status;
            _serverCount = serverCount;
            _shardCount = shardCount;
            _website = website;
            _github = github;
            _discord = discord;
            _avatar = avatar;
        }

    }
    public enum BotFlag
    {
        Normal,
        Official,
        KoreanBotsVerified,
        Partner,
        DiscordVerified,
        Premium,
        FirstHackertonWinner
    }
    public enum BotStatus
    {
        Ok,
        Reported,
        Blocked,
        Private,
        Archived
    }
}