using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SharpKoreanBots.Bot
{
    using SharpKoreanBots.User;
    public struct BotInfo
    {
        UserInfo? _owner;
        public UserInfo? Owner
        {
            get { return _owner; }
        }
        ulong _id;
        public ulong ID
        {
            get {return _id;}
        }
        string _token;
        public string Token
        {
            get {return _token;}
            set {_token = value;}
        }
        // BotFlag? _flag;
        // public BotFlag? Flag
        // {
        //     get {return _flag;}
        // }
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
        BotState? _state;
        public BotState? State
        {
            get {return _state;}
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
        public BotInfo(ulong id, string token = null, string name = null, int tag = 0, UserInfo? owner = null, string prefix = null, int votes = 0, string intro = null, string description = null, string library = null, BotState? state = null, int serverCount = 0, int shardCount = 1, string website = null, string github = null, string discord = null, string avatar = null)
        {
            _owner = owner;
            _id = id;
            _token = token;
            _name = name;
            _tag = tag;
            // _flag = flag;
            _prefix = prefix;
            _votes = votes;
            _intro = intro;
            _description = description;
            _library = library;
            _state = state;
            _serverCount = serverCount;
            _shardCount = shardCount;
            _website = website;
            _github = github;
            _discord = discord;
            _avatar = avatar;
        }
        public void Update()
        /// <summary>Update shard count and server count of bot.</summary>
        {
            JObject update = new JObject();
            update.Add("servers", _serverCount);
            update.Add("shards", _shardCount);
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", _token);
            client.Headers.Add("Content-Type", "application/json");
            client.UploadString($"https://koreanbots.dev/api/v2/bots/{_id}/stats", "POST", update.ToString());
        }

        public static BotState getBotState(string stateString)
        {
            switch (stateString)
            {
                case "ok":
                    return BotState.Ok;
                case "reported":
                    return BotState.Reported;
                case "blocked":
                    return BotState.Blocked;
                case "private":
                    return BotState.Private;
                case "archived":
                    return BotState.Archived;
                default:
                    throw new System.Exception("Unkown state");
            }
        }

    }
    
    // public enum BotFlag
    // {
    //     Normal,
    //     Official,
    //     KoreanBotsVerified,
    //     Partner,
    //     DiscordVerified,
    //     Premium,
    //     FirstHackertonWinner
    // }
    public enum BotState
    {
        Ok,
        Reported,
        Blocked,
        Private,
        Archived
    }
}