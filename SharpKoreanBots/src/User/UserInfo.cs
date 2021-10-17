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
        public static UserInfo Get(ulong UserID)
        {
            WebClient client = new WebClient();
            string download = client.DownloadString(baseUrl + "users/" + UserID); //정보 얻어오기
            JObject json = JObject.Parse(download); //정보 파싱

            if((int)json["code"] != 200)
            {
                throw new Exception($"Server returned code {json["code"]}\nMessage: {json["message"]}");
            }
            return new UserInfo(UserID);
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