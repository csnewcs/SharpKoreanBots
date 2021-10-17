using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SharpKoreanBots.Bot
{
    using SharpKoreanBots.User;
    public struct BotInfo
    {
        const string baseUrl = "https://koreanbots.dev/api/v2/";
        UserInfo[] _owner;
        public UserInfo[] Owner
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
        public BotInfo(ulong id, string token = null, string name = null, int tag = 0, UserInfo[] owner = null, string prefix = null, int votes = 0, string intro = null, string description = null, string library = null, BotState? state = null, int serverCount = 0, int shardCount = 1, string website = null, string github = null, string discord = null, string avatar = null)
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

        /// <summary>get bot info from bot id</summary>
        public static BotInfo Get(ulong BotID)
        {
            WebClient client = new WebClient();
            string download = client.DownloadString(baseUrl + "bots/" + BotID); //정보 얻어오기
            JObject json = JObject.Parse(download); //정보 파싱

            if((int)json["code"] != 200)
            {
                throw new Exception($"Server returned code {json["code"]}\nMessage: {json["message"]}");
            }

            json = json["data"] as JObject;

            List<UserInfo> ownerInfos = new List<UserInfo>();
            // ====소유자====
            for(int i = 0; i < json["owners"].Count(); i++)
            {
                JObject owner = json["owners"][i] as JObject;
                JArray ownerBots = owner["bots"] as JArray;
                List<BotInfo> botinfos = new List<BotInfo>();
                foreach(var ownerBot in ownerBots) //유저의 봇들 얻음
                {
                    botinfos.Add(new BotInfo((ulong)ownerBot));
                }
                UserInfo ownerInfo = new UserInfo((ulong)owner["id"], owner["username"].ToString(), (int)owner["tag"], owner["github"].ToString(), bots: botinfos.ToArray());
                ownerInfos.Add(ownerInfo);
            }


            // ====봇 정보====
            JObject bot = json;
            // System.IO.File.WriteAllText("getbot", bot.ToString());
            BotInfo info = new BotInfo((ulong)bot["id"], null, bot["name"].ToString(), (int)bot["tag"], ownerInfos.ToArray(), bot["prefix"]?.ToString(), (int)bot["votes"], bot["intro"]?.ToString(), bot["desc"]?.ToString(), bot["lib"]?.ToString(), BotInfo.GetBotState(bot["state"]?.ToString()), bot["servers"].Type == JTokenType.Null ? 0 : (int)bot["servers"], bot["shards"].Type == JTokenType.Null ? 0 : (int)bot["shards"], bot["web"]?.ToString(), bot["github"]?.ToString(), bot["discord"]?.ToString(), bot["avatar"]?.ToString());
            // info.Update()
            return info;
        }
        /// <summary>Update shard count and server count of bot.</summary>
        public void Update() 
        {
            JObject update = new JObject();
            update.Add("servers", _serverCount);
            update.Add("shards", _shardCount);
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", _token);
            client.Headers.Add("Content-Type", "application/json");
            client.UploadString($"https://koreanbots.dev/api/v2/bots/{_id}/stats", "POST", update.ToString());
        }

        public override string ToString()
        {
            return $"{_name}#{_tag}";
        }
        public static BotState GetBotState(string stateString)
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
    // public enum Category
    // {
    //     관리,
    //     뮤직,
    //     전적,
    //     게임,
    //     도박,
    //     로깅,
    //     빗금명령어,
    //     웹대시보드,
    //     밈,
    //     레벨링,
    //     유틸리티,
    //     대화,
    //     NSFW,
    //     검색,
    //     학교,
    //     코로나19,
    //     번역,
    //     오버워치,
    //     리그오브레전드,
    //     배틀그라운드,
    //     마인크래프트
    // }
    public enum BotCategory
    {
        Management,
        Music,
        Stats,
        Games,
        Gambling,
        Logging,
        SlashCommand,
        WebDashboard,
        MEME,
        Leveling,
        Utilities,
        Chat,
        NSFW,
        Search,
        School,
        Covid19,
        Translation,
        Overwatch,
        LeagueOfLegends,
        BattleGround,
        Minecraft
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
    public enum BotStatus
    {
        Online,
        Idle,
        Dnd,
        Streaming,
        Offline
    }
}