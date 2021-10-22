using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace SharpKoreanBots.User
{
    using SharpKoreanBots.Bot;

    public struct UserInfo
    {
        const string baseUrl = "https://koreanbots.dev/api/v2/";

        ulong _id;
        public ulong ID
        {
            get {return _id;}
        }
        string _name;
        public string Name
        {
            get => _name;
        }
        int _tag;
        public int Tag
        {
            get => _tag;
        }
        string _github;
        public string GitHub
        {
            get => _github;
        }
        UserFlag _flag;
        BotInfo[] _bots;
        public BotInfo[] Bots
        {
            get => _bots;
        }
        public UserInfo(ulong id, string name = null, int tag = 0, string github = null, UserFlag flag = UserFlag.Normal, BotInfo[] bots = null)
        {
            _id = id;
            _name = name;
            _tag = tag;
            _github = github;
            _flag = flag;
            _bots = bots;
        }
        public static UserInfo Get(ulong UserID)
        {
            WebClient client = new WebClient();
            string download = client.DownloadString(baseUrl + "users/" + UserID); //정보 얻어오기
            JObject json = JObject.Parse(download); //정보 파싱

            if((int)json["code"] != 200)
            {
                throw new Exception($"Server returned code {json["code"]}\nMessage: {json["message"]}");
            }
            JObject data = (JObject)json["data"];

            List<BotInfo> bots = new List<BotInfo>();
            foreach(var bot in data["bots"])
            {
                List<UserInfo> owners = new List<UserInfo>();
                foreach(var owner in bot["owners"])
                {
                    owners.Add(new UserInfo((ulong)owner));
                }
                List<BotCategory> categories = new List<BotCategory>();
                foreach(var category in bot["category"])
                {
                    categories.Add(BotInfo.GetBotCategory(category.ToString()));
                }
                bots.Add(new BotInfo((ulong)bot["id"], null, bot["name"].ToString(), (int)bot["tag"], BotInfo.GetBotFlags((int)bot["flags"]), owners.ToArray(), bot["prefix"]?.ToString(), (int)bot["votes"], bot["intro"]?.ToString(), bot["desc"]?.ToString(), bot["lib"]?.ToString(), BotInfo.GetBotState(bot["state"]?.ToString()), categories.ToArray(), bot["servers"].Type == JTokenType.Null ? 0 : (int)bot["servers"], bot["shards"].Type == JTokenType.Null ? 0 : (int)bot["shards"], bot["web"]?.ToString(), bot["github"]?.ToString(), bot["discord"]?.ToString(), bot["avatar"]?.ToString())); 
            }

            return new UserInfo(
                (ulong)data["id"],
                (string)data["username"],
                (int)data["tag"],
                data["github"]?.ToString(),
                bots: bots.ToArray()
                // (UserFlag)data["flag"],
                // BotInfo.Get((ulong)data["id"])
            );
        }
        public override string ToString()
        {
            return $"{_name}#{_tag}";
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