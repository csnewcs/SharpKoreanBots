using System;
using SharpKoreanBots; 
using SharpKoreanBots.Bot;

namespace _2._Update_Bot_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] info = System.IO.File.ReadAllLines("../token.txt");
            string token = info[0];
            KoreanBotsClient client = new KoreanBotsClient(token);
            BotInfo botInfo = client.GetBot(ulong.Parse(info[1]));
            botInfo.Token = token;
            botInfo.ServerCount = 2;
            botInfo.ShardCount = 1;
            botInfo.Update();
            Console.WriteLine("Done!");
        }
    }
}
