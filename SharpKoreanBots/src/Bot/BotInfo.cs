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
        public UserInfo[] Owner {get;}
        public ulong ID {get;}
        public string Token{get;set;}
        public BotFlag[] Flags {get;}
        public string Library {get;}
        public string Prefix {get;}
        public int Votes {get;}
        public int ServerCount {get;}
        public int ShardCount {get; set;}
        public string Intro {get;}
        public string Description {get;}
        public string Website {get;}
        public string Github {get;}
        public string Discord {get;}
        public BotState State {get;}
        public BotStatus Status {get;}
        public string Name {get;}
        public int Tag {get;}
        public string Avatar {get;}
        public BotCategory[] Categories {get;}
        public string URL {get;}
        public string Vanity {get;}
        public string Background {get;}
        public string Banner {get;}
        
        
        public BotInfo(ulong id, string token = null, string name = null, int tag = 0, BotFlag[] flags = null, UserInfo[] owner = null, string prefix = null, int votes = 0, string intro = null, string description = null, string library = null, BotState state = BotState.Null, BotStatus status = BotStatus.Null , BotCategory[] categories = null, int serverCount = 0, int shardCount = 1, string website = null, string github = null, string discord = null, string avatar = null, string url = null, string vanity = null, string background = null, string banner = null)
        {
            Owner = owner;
            ID = id;
            Token = token;
            Name = name;
            Tag = tag;
            Flags = flags;
            Prefix = prefix;
            Votes = votes;
            Intro = intro;
            Description = description;
            Library = library;
            State = state;
            Categories = categories;
            ServerCount = serverCount;
            ShardCount = shardCount;
            Website = website;
            Github = github;
            Discord = discord;
            Avatar = avatar;
            Status = status;
            URL = url;
            Vanity = vanity;
            Background = background;
            Banner = banner;
        }

        /// <summary>get bot info from bot id</summary>
        public static BotInfo Get(ulong BotID)
        {
            WebClient client = new WebClient();
            string download = client.DownloadString(baseUrl + "bots/" + BotID); //?????? ????????????
            JObject json = JObject.Parse(download); //?????? ??????

            if((int)json["code"] != 200)
            {
                throw new Exception($"Server returned code {json["code"]}\nMessage: {json["message"]}");
            }

            json = json["data"] as JObject;

            List<UserInfo> ownerInfos = new List<UserInfo>();
            // ====?????????====
            for(int i = 0; i < json["owners"].Count(); i++)
            {
                JObject owner = json["owners"][i] as JObject;
                JArray ownerBots = owner["bots"] as JArray;
                List<BotInfo> botinfos = new List<BotInfo>();
                foreach(var ownerBot in ownerBots) //????????? ?????? ??????
                {
                    botinfos.Add(new BotInfo((ulong)ownerBot));
                }
                UserInfo ownerInfo = new UserInfo((ulong)owner["id"], owner["username"].ToString(), (int)owner["tag"], owner["github"].ToString(), bots: botinfos.ToArray());
                ownerInfos.Add(ownerInfo);
            }


            // ====??? ??????====
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
            update.Add("servers", ServerCount);
            update.Add("shards", ShardCount);
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", Token);
            client.Headers.Add("Content-Type", "application/json");
            client.UploadString($"https://koreanbots.dev/api/v2/bots/{ID}/stats", "POST", update.ToString());
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
            string url = $"{baseUrl}bots/{ID}/vote?userID={userId}";
            client.Headers.Add("Authorization", Token);
            string download = client.DownloadString(url);
            JObject json = JObject.Parse(download)["data"] as JObject;
            return json;
        }
        public override string ToString()
        {
            return $"{Name}#{Tag}";
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
                case "??????":
                    return BotCategory.Management;
                case "??????":
                    return BotCategory.Music;
                case "??????":
                    return BotCategory.Record;
                case "??????":
                    return BotCategory.Game;
                case "??????":
                    return BotCategory.Gambling;
                case "??????":
                    return BotCategory.Logging;
                case "?????? ?????????":
                    return BotCategory.SlashCommand;
                case "??? ????????????":
                    return BotCategory.WebDashboard;
                case "???":
                    return BotCategory.MEME;
                case "?????????":
                    return BotCategory.Leveling;
                case "????????????":
                    return BotCategory.Utility;
                case "??????":
                    return BotCategory.Chat;
                case "NSFW":
                    return BotCategory.NSFW;
                case "??????":
                    return BotCategory.Search;
                case "??????":
                    return BotCategory.School;
                case "?????????19":
                    return BotCategory.Covid19;
                case "??????":
                    return BotCategory.Translation;
                case "????????????":
                    return BotCategory.Overwatch;
                case "?????? ?????? ?????????":
                    return BotCategory.LeagueOfLegends;
                case "??????????????????":
                    return BotCategory.BattleGround;
                case "??????????????????":
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
            if((flagInt & 1) == 1) //0b10??? ????????????
            {
                flags.Add(BotFlag.Official);
            }
            return flags.ToArray();
        }
    }
    // public enum Category
    // {
    //     ??????,
    //     ??????,
    //     ??????,
    //     ??????,
    //     ??????,
    //     ??????,
    //     ???????????????,
    //     ???????????????,
    //     ???,
    //     ?????????,
    //     ????????????,
    //     ??????,
    //     NSFW,
    //     ??????,
    //     ??????,
    //     ?????????19,
    //     ??????,
    //     ????????????,
    //     ?????????????????????,
    //     ??????????????????,
    //     ??????????????????
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