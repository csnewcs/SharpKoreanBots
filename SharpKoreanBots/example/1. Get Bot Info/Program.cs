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
            string categories = "";
            string flags = "";
            foreach(BotCategory category in botinfo.Categories)
            {
                categories += category + ", ";
            }
            foreach(BotFlag flag in botinfo.Flags)
            {
                flags += flag + ", ";
            }
            Console.WriteLine($"============================== 봇 정보 ==============================\n이름: {botinfo.Name}\n접두사: {botinfo.Prefix}\n라이브러리: {botinfo.Library}\n한줄 소개: {botinfo.Intro}\n카테고리: {categories}\n플래그: {flags}\n아바타: {botinfo.Avatar}\n\n===== 소유자 =====\n닉네임#태그: {botinfo.Owner[0]}");
            
        }
    }
}
