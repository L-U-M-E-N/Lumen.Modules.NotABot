namespace Lumen.Modules.NotABot.Common.Models {
    public class ServerChannelStats {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long ChannelId { get; set; }
        public int MessageCount { get; set; }
        public int ReactionCount { get; set; }
    }
}
