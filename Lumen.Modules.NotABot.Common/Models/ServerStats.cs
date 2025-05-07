namespace Lumen.Modules.NotABot.Common.Models {
    public class ServerStats {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int MemberLeft { get; set; }
        public int MemberJoined { get; set; }
        public int MemberCount { get; set; }
        public int MessageCount { get; set; }
        public int ReactionAdded { get; set; }
        public int ReactionRemoved { get; set; }
    }
}
