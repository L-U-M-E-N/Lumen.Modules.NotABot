namespace Lumen.Modules.NotABot.Common.Models {
    public class ServerUserStats {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
        public int MessageCount { get; set; }
        public int ReactionCount { get; set; }
    }
}
