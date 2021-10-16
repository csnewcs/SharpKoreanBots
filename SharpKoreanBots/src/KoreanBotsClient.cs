using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SharpKoreanBots.Bot;
using SharpKoreanBots.User;

namespace SharpKoreanBots
{
    public class KoreanBotsClient
    {
        const string baseUrl = "https://koreanbots.dev/api/v2/";
        private readonly string _token;

        public KoreanBotsClient(string Token)
        {
            _token = Token;
        }
        public BotInfo GetBot(ulong BotID)
        {
            WebClient client = new WebClient();
            string download = client.DownloadString(baseUrl + "bots/" + BotID); //정보 얻어오기
            JObject json = JObject.Parse(download); //정보 파싱

            if((int)json["code"] != 200)
            {
                throw new Exception($"Server returned code {json["code"]}\nMessage: {json["message"]}");
            }

            json = json["data"] as JObject;

            // ====소유자====
            JObject owner = json["owners"][0] as JObject;
            JArray ownerBots = owner["bots"] as JArray;
            List<BotInfo> botinfos = new List<BotInfo>();

            foreach(var ownerBot in ownerBots) //유저의 봇들 얻음
            {
                botinfos.Add(new BotInfo((ulong)ownerBot));
            }
            UserInfo ownerInfo = new UserInfo((ulong)owner["id"], owner["username"].ToString(), (int)owner["tag"], owner["github"].ToString(), bots: botinfos.ToArray());

            // ====봇 정보====
            JObject bot = json;
            // System.IO.File.WriteAllText("getbot", bot.ToString());
            BotInfo info = new BotInfo((ulong)bot["id"], bot["name"].ToString(), null, (int)bot["tag"], ownerInfo, bot["prefix"]?.ToString(), (int)bot["votes"], bot["intro"]?.ToString(), bot["desc"]?.ToString(), bot["lib"]?.ToString(), BotInfo.getBotState(bot["state"]?.ToString()), bot["servers"].Type == JTokenType.Null ? 0 : (int)bot["servers"], bot["shards"].Type == JTokenType.Null ? 0 : (int)bot["shards"], bot["web"]?.ToString(), bot["github"]?.ToString(), bot["discord"]?.ToString(), bot["avatar"]?.ToString());
            // info.Update()
            return info;
        }
        public UserInfo GetUser(ulong UserID)
        {
            return new UserInfo(UserID);
        }
        public void UpdateBotInfo(BotInfo BotInfo)
        {
            
        }
    }
}
