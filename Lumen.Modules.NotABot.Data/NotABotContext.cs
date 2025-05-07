using Lumen.Modules.NotABot.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.NotABot.Data {
    public class NotABotContext : DbContext {
        public const string SCHEMA_NAME = "NotABot";

        public NotABotContext(DbContextOptions<NotABotContext> options) : base(options) {
        }

        public DbSet<ServerStats> ServerStats { get; set; } = null!;
        public DbSet<ServerChannelStats> ServerChannelStats { get; set; } = null!;
        public DbSet<ServerReactionStats> ServerReactionStats { get; set; } = null!;
        public DbSet<ServerUserStats> ServerUserStats { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            var serverStatsTableBuilder = modelBuilder.Entity<ServerStats>();
            serverStatsTableBuilder.Property(x => x.Date)
                .HasColumnType("timestamp with time zone");

            serverStatsTableBuilder.HasKey(x => new { x.Id, x.Date });

            var serverChannelStatsTableBuilder = modelBuilder.Entity<ServerChannelStats>();
            serverChannelStatsTableBuilder.Property(x => x.Date)
                .HasColumnType("timestamp with time zone");

            serverChannelStatsTableBuilder.HasKey(x => new { x.Id, x.Date, x.ChannelId });

            var serverReactionStatsTableBuilder = modelBuilder.Entity<ServerReactionStats>();
            serverReactionStatsTableBuilder.Property(x => x.Date)
                .HasColumnType("timestamp with time zone");

            serverReactionStatsTableBuilder.HasKey(x => new { x.Id, x.Date, x.Reaction });

            var serverUserStatsTableBuilder = modelBuilder.Entity<ServerUserStats>();
            serverUserStatsTableBuilder.Property(x => x.Date)
                .HasColumnType("timestamp with time zone");

            serverUserStatsTableBuilder.HasKey(x => new { x.Id, x.Date, x.UserId });
        }
    }
}
