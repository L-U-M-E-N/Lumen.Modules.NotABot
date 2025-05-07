namespace Lumen.Modules.NotABot.Common.Models {
    public class ServerReactionStats {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reaction { get; set; } = null!;
        public int Amount { get; set; }
    }
}
