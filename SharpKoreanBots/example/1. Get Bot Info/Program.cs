using System;
using SharpKoreanBots;
using SharpKoreanBots.Bot;

namespace _1._Get_Bot_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            BotInfo botinfo = BotInfo.Get(ulong.Parse(System.IO.File.ReadAllLines("../token.txt")[1]));
            Console.WriteLine($"============================== 봇 정보 ==============================\n이름: {botinfo.Name}\n접두사: {botinfo.Prefix}\n라이브러리: {botinfo.Library}\n한줄 소개: {botinfo.Intro}\n\n===== 소유자 =====\n닉네임#태그: {botinfo.Owner[0]}");
        }
    }
}
