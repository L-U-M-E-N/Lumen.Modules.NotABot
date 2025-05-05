using Lumen.Modules.NotABot.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.NotABot.Data {
    public class NotABotContext : DbContext {
        public const string SCHEMA_NAME = "NotABot";

        public NotABotContext(DbContextOptions<NotABotContext> options) : base(options) {
        }

        public DbSet<NotABotPointInTime> NotABot { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            var NotABotModelBuilder = modelBuilder.Entity<NotABotPointInTime>();
            NotABotModelBuilder.Property(x => x.Time)
                .HasColumnType("timestamp with time zone");

            NotABotModelBuilder.Property(x => x.Value)
                .HasColumnType("integer");

            NotABotModelBuilder.HasKey(x => x.Time);
        }
    }
}
