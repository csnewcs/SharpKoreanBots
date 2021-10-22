using System;
using System.IO;
using SharpKoreanBots.Bot;

namespace _4._Check_If_User_Voted
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] info = File.ReadAllLines("../token.txt"); //[0]: bot token, [1]: bot id, [2] user id
            ulong botId = ulong.Parse(info[1]);
            string token = info[0];
            ulong userId = ulong.Parse(info[2]);
            DateTime dateTime;
            BotInfo bot = new BotInfo(botId, token);
            bool isVoted = bot.isVoted(userId, out dateTime);
            dateTime = dateTime.ToLocalTime();
            Console.WriteLine(isVoted);
            Console.WriteLine(dateTime);
        }
    }
}
