namespace SharpKoreanBots.Server
{
    using SharpKoreanBots.Bot;
    using SharpKoreanBots.User;
    public struct ServerInfo
    {
        string ID {get;}
        string Name {get;}
        string Icon {get;}
        UserInfo? Owner {get;}
        ServerFlag[] Flags {get;}
        int Votes {get;}
        int? Members {get;}
        int? BoostTier {get;}
        Emoji[] Emojis {get;}
        string Description {get;}
        ServerCategory[] Categories {get;}
        string Invite {get;}
        BotInfo[] Bots {get;}
        string Vanity {get;}
        string Background {get;}
        string Banner {get;}
        ServerState State {get;}
    }
    public enum ServerState
    {
        Ok,
        Reported,
        Blocked,
        Unreachable,
        Null
    }
    public enum ServerCategory
    {
        Community,
        Friendship, //친목
        Music,
        Technology,
        Education,
        Game,
        Overwatch,
        LeagueOfLegends,
        PUBG,
        Minecraft
    }
    public enum ServerFlag
    {
        General,
        Official,
        KoreanBotsVerified,
        KoreanBotsPartner,
        DiscordVerified,
        DiscordPartner
    }
}