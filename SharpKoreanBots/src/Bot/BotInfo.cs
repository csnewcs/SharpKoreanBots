using System;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        BotFlag[] _flags;
        public BotFlag[] Flags
        {
            get {return _flags;}
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
        int _shardCount;
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
        BotState _state;
        public BotState State
        {
            get {return _state;}
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
        BotCategory[] _categories;
        public BotCategory[] Categories
        {
            get {return _categories;}
        }
        string _url;
        public string URL
        {
            get {return _url;}
        }
        string _vanity;
        public string Vanity
        {
            get {return _vanity;}
        }
        string _background;
        public string Background
        {
            get {return _background;}
        }
        string _banner;
        public string Banner
        {
            get {return _banner;}
        }        
        
        
        public BotInfo(ulong id, string token = null, string name = null, int tag = 0, BotFlag[] flags = null, UserInfo[] owner = null, string prefix = null, int votes = 0, string intro = null, string description = null, string library = null, BotState state = BotState.Null, BotStatus status = BotStatus.Null , BotCategory[] categories = null, int serverCount = 0, int shardCount = 1, string website = null, string github = null, string discord = null, string avatar = null, string url = null, string vanity = null, string background = null, string banner = null)
        {
            _owner = owner;
            _id = id;
            _token = token;
            _name = name;
            _tag = tag;
            _flags = flags;
            _prefix = prefix;
            _votes = votes;
            _intro = intro;
            _description = description;
            _library = library;
            _state = state;
            _categories = categories;
            _serverCount = serverCount;
            _shardCount = shardCount;
            _website = website;
            _github = github;
            _discord = discord;
            _avatar = avatar;
            _status = status;
            _url = url;
            _vanity = vanity;
            _background = background;
            _banner = banner;
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
            // JObject bot = json;
            // System.IO.File.WriteAllText("getbot", bot.ToString());
            List<BotCategory> categories = new List<BotCategory>();
            foreach(var category in json["category"])
            {
                categories.Add(BotInfo.GetBotCategory(category.ToString()));
            }
            
            BotInfo info = new BotInfo((ulong)json["id"], null, json["name"].ToString(), (int)json["tag"],BotInfo.GetBotFlags((int)json["flags"]) , ownerInfos.ToArray(), json["prefix"]?.ToString(), (int)json["votes"], json["intro"]?.ToString(), json["desc"]?.ToString(), json["lib"]?.ToString(), BotInfo.GetBotState(json["state"]?.ToString()), BotInfo.GetBotStatus(json["status"]?.ToString()), categories.ToArray(), json["servers"].Type == JTokenType.Null ? 0 : (int)json["servers"], json["shards"].Type == JTokenType.Null ? 0 : (int)json["shards"], json["web"]?.ToString(), json["github"]?.ToString(), json["discord"]?.ToString(), json["avatar"]?.ToString(), json["url"]?.ToString(), json["vanity"]?.ToString(), json["bg"]?.ToString(), json["banner"]?.ToString());
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
        public bool isVoted(ulong userId)
        {
            JObject json = getVoteJson(userId);
            return (bool)json["voted"];
        }
        public bool isVoted(UserInfo user)
        {
            JObject json = getVoteJson(user.ID);
            return (bool)json["voted"];
        }
        public bool isVoted(ulong userId, out DateTime time)
        {
            JObject json = getVoteJson(userId);
            time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)json["lastVote"]); //timestamp = 0
            return (bool)json["voted"];
        }
        public bool isVoted(UserInfo user, out DateTime time)
        {
            JObject json = getVoteJson(user.ID);
            time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)json["lastVote"]); //timestamp = 0
            return (bool)json["voted"];            
        }
        private JObject getVoteJson(ulong userId)
        {
            WebClient client = new WebClient();
            string url = $"{baseUrl}bots/{_id}/vote?userID={userId}";
            client.Headers.Add("Authorization", _token);
            string download = client.DownloadString(url);
            JObject json = JObject.Parse(download)["data"] as JObject;
            return json;
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
                case null:
                    return BotState.Null;
                default:
                    throw new System.Exception("Unkown state");
            }
        }
        public static BotStatus GetBotStatus(string statusString)
        {
            switch (statusString)
            {
                case "online":
                    return BotStatus.Online;
                case "idle":
                    return BotStatus.Idle;
                case "dnd":
                    return BotStatus.Dnd;
                case "streaming":
                    return BotStatus.Streaming;
                case "offline":
                    return BotStatus.Offline;
                case null:
                    return BotStatus.Null;
                default:
                    throw new System.Exception("Unkown status");
            }
        }
        public static BotCategory GetBotCategory(string categoryString)
        {
            switch(categoryString)
            {
                case "관리":
                    return BotCategory.Management;
                case "뮤직":
                    return BotCategory.Music;
                case "전적":
                    return BotCategory.Record;
                case "게임":
                    return BotCategory.Game;
                case "도박":
                    return BotCategory.Gambling;
                case "로깅":
                    return BotCategory.Logging;
                case "빗금 명령어":
                    return BotCategory.SlashCommand;
                case "웹 대시보드":
                    return BotCategory.WebDashboard;
                case "밈":
                    return BotCategory.MEME;
                case "레벨링":
                    return BotCategory.Leveling;
                case "유틸리티":
                    return BotCategory.Utility;
                case "대화":
                    return BotCategory.Chat;
                case "NSFW":
                    return BotCategory.NSFW;
                case "검색":
                    return BotCategory.Search;
                case "학교":
                    return BotCategory.School;
                case "코로나19":
                    return BotCategory.Covid19;
                case "번역":
                    return BotCategory.Translation;
                case "오버워치":
                    return BotCategory.Overwatch;
                case "리그 오브 레전드":
                    return BotCategory.LeagueOfLegends;
                case "배틀그라운드":
                    return BotCategory.BattleGround;
                case "마인크래프트":
                    return BotCategory.Minecraft;                
                default:
                    throw new System.Exception("Unkown category");
            }
        }
        public static BotFlag[] GetBotFlags(int flagInt)
        {
            var flags = new List<BotFlag>();
            if((flagInt & 64) == 64)
            {
                flags.Add(BotFlag.FirstHackertonWinner);
            }
            if((flagInt & 32) == 32)
            {
                flags.Add(BotFlag.Premium);
            }
            if((flagInt & 16) == 16)
            {
                flags.Add(BotFlag.DiscordVerified);
            }
            if((flagInt & 8) == 8)
            {
                flags.Add(BotFlag.Partner);
            }
            if((flagInt & 4) == 4)
            {
                flags.Add(BotFlag.KoreanBotsVerified);
            }
            if((flagInt & 1) == 1) //0b10은 비어있음
            {
                flags.Add(BotFlag.Official);
            }
            return flags.ToArray();
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
        Record,
        Game,
        Gambling,
        Logging,
        SlashCommand,
        WebDashboard,
        MEME,
        Leveling,
        Utility,
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
    
    public enum BotFlag
    {
        Official,
        KoreanBotsVerified,
        Partner,
        DiscordVerified,
        Premium,
        FirstHackertonWinner
    }
    public enum BotState
    {
        Ok,
        Reported,
        Blocked,
        Private,
        Archived,
        Null
    }
    public enum BotStatus
    {
        Online,
        Idle,
        Dnd,
        Streaming,
        Offline,
        Null
    }
}