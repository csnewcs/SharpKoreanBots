namespace SharpKoreanBots.Server
{
    public struct Emoji
    {
        public string Name { get; }
        public string Id { get; }
        public string Url { get; }
        public Emoji(string name, string id, string url)
        {
            Name = name;
            Id = id;
            Url = url;
        }
    }
}