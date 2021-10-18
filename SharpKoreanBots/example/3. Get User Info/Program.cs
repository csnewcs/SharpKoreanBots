using System;
using SharpKoreanBots.User;

namespace _3._Get_User_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = ulong.Parse(System.IO.File.ReadAllLines("../token.txt")[2]);
            UserInfo userInfo = UserInfo.Get(id);
            string bots = "";
            foreach(var bot in userInfo.Bots)
            {
                bots += bot.ToString() + "\n";
            }
            Console.WriteLine($"===============유저 정보===============\n이름#태그: {userInfo.Name}#{userInfo.Tag}\n깃헙: {userInfo.GitHub}\n봇들: {bots}");
        }
    }
}
