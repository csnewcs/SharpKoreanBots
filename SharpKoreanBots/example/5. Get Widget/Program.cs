using System;
using System.IO;
using SharpKoreanBots.Widget;

namespace _5._Get_Widget
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong id = ulong.Parse(File.ReadAllLines("../token.txt")[1]);
            Widget.DownloadWidget("test.svg", id, WidgetType.Servers);
        }
    }
}
