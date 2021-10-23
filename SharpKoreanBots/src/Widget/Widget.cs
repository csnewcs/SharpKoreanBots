using System.Net;
using SharpKoreanBots.Bot;

namespace SharpKoreanBots.Widget
{
    public struct Widget
    {
        const string baseUrl = "https://koreanbots.dev/api/";
        public static void DownloadWidget(string fileName, ulong botId, WidgetType widgetType, WidgetStyle widgetStyle = WidgetStyle.Flat, double widgetSize = 1.0, bool showIcon = true)
        {
            if(widgetSize < 0.5 || widgetSize > 3.0)
            {
                throw new WebException("Widget size must be between 0.5 and 3.0");
            }
            WebClient client = new WebClient();
            client.DownloadFile($"{baseUrl}widget/bots/{widgetType.ToString().ToLower()}/{botId}.svg?style={widgetStyle.ToString().ToLower()}&scale={widgetSize}&icon={showIcon.ToString().ToLower()}", fileName);
        }
        public static void DownloadWidget(string fileName, BotInfo botInfo, WidgetType widgetType, WidgetStyle widgetStyle = WidgetStyle.Flat, double widgetSize = 1.0, bool showIcon = true)
        {
            if(widgetSize < 0.5 || widgetSize > 3.0)
            {
                throw new WebException("Widget size must be between 0.5 and 3.0");
            }
            WebClient client = new WebClient();
            client.DownloadFile($"{baseUrl}widget/bots/{widgetType.ToString().ToLower()}/{botInfo.ID}.svg?style={widgetStyle.ToString().ToLower()}&scale={widgetSize}&icon={showIcon.ToString().ToLower()}", fileName);
        }
        public static string GetDownloadWidgetString(ulong botId, WidgetType widgetType, WidgetStyle widgetStyle = WidgetStyle.Flat, double widgetSize = 1.0, bool showIcon = true)
        {
            if(widgetSize < 0.5 || widgetSize > 3.0)
            {
                throw new WebException("Widget size must be between 0.5 and 3.0");
            }
            return $"{baseUrl}widget/bots/{widgetType.ToString().ToLower()}/{botId}.svg?style={widgetStyle.ToString().ToLower()}&scale={widgetSize}&icon={showIcon.ToString().ToLower()}";
        }
        public static string GetDownloadWidgetString(BotInfo botInfo, WidgetType widgetType, WidgetStyle widgetStyle = WidgetStyle.Flat, double widgetSize = 1.0, bool showIcon = true)
        {
            if(widgetSize < 0.5 || widgetSize > 3.0)
            {
                throw new WebException("Widget size must be between 0.5 and 3.0");
            }
            return $"{baseUrl}widget/bots/{widgetType.ToString().ToLower()}/{botInfo.ID}.svg?style={widgetStyle.ToString().ToLower()}&scale={widgetSize}&icon={showIcon.ToString().ToLower()}";
        }

    }
    public enum WidgetType
    {
        Votes,
        Servers,
        Status
    }
    public enum WidgetStyle
    {
        Classic,
        Flat
    }
}