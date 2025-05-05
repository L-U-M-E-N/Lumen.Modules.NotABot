using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lumen.Modules.NotABot.Data {
    public class NotABotDbContextFactory : IDesignTimeDbContextFactory<NotABotContext> {
        public NotABotContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<NotABotContext>();
            optionsBuilder.UseNpgsql();

            return new NotABotContext(optionsBuilder.Options);
        }
    }
}
