using Lumen.Modules.NotABot.Data;
using Lumen.Modules.NotABot.Module.Dto;
using Lumen.Modules.Sdk;

using Microsoft.EntityFrameworkCore;

using System.Net;
using System.Text.Json;

namespace Lumen.Modules.NotABot.Module {
    public class NotABotModule(IEnumerable<ConfigEntry> configEntries, ILogger<LumenModuleBase> logger, IServiceProvider provider) : LumenModuleBase(configEntries, logger, provider) {
        public const string NAB_SERVER_LIST = nameof(NAB_SERVER_LIST);

        private IEnumerable<ConfigObject> GetServerList() {
            var configEntry = configEntries.FirstOrDefault(x => x.ConfigKey == NAB_SERVER_LIST);
            if (configEntry is null || configEntry.ConfigValue is null) {
                logger.LogError($"[{nameof(NotABotModule)}] Config key \"{NAB_SERVER_LIST}\" is missing!");
                throw new NullReferenceException(nameof(configEntry));
            }

            return JsonSerializer.Deserialize<IEnumerable<ConfigObject>>(configEntry.ConfigValue)!;
        }

        public override Task InitAsync(LumenModuleRunsOnFlag currentEnv) {
            return RunAsync(currentEnv, DateTime.UtcNow);
        }

        private async Task<NotABotAPIDto> FetchData(string serverURL, string apiKey, long serverId, DateTime date) {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", $"L.U.M.E.N. stats gatherer - By Elanis - https://github.com/L-U-M-E-N/");

            var res = await httpClient.GetAsync($"{serverURL}/stats/server/{serverId}/day/{date:yyyy-MM-dd}");
            if (res is null || res.StatusCode != HttpStatusCode.OK) {
                throw new Exception($"Error when retrieving data from NotABot on server {serverId} for {date:yyyy-MM-dd}.");
            }

            var data = JsonSerializer.Deserialize<NotABotAPIDto>(await res.Content.ReadAsStringAsync());
            if (data is null) {
                throw new Exception($"Error when deserializing data from NotABot on server {serverId} for {date:yyyy-MM-dd}.");
            }

            return data;
        }

        public DateTime GetLatestStatDateForServer(NotABotContext context, long serverId) {
            if (!context.ServerStats.Any(x => x.Id == serverId)) {
                return new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            return context.ServerStats.Where(x => x.Id == serverId).Select(x => x.Date).Max();
        }

        public override async Task RunAsync(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            try {
                using var scope = provider.CreateScope();
                var context = provider.GetRequiredService<NotABotContext>();

                logger.LogTrace($"[{nameof(NotABotModule)}] Running tasks ...");
                var config = GetServerList();
                foreach (var configEntry in config) {
                    var serverId = long.Parse(configEntry.ServerId);
                    var maxDate = GetLatestStatDateForServer(context, serverId);
                    for (var selectedDate = maxDate.AddDays(1); selectedDate < date; selectedDate.AddDays(1)) {
                        var data = await FetchData(configEntry.ServerUrl, configEntry.AuthToken, serverId, date);

                        await context.ServerStats.AddAsync(new Common.Models.ServerStats {
                            Id = serverId,
                            Name = configEntry.ServerName,
                            MemberLeft = data.MemberLeft,
                            MemberJoined = data.MemberJoined,
                            MemberCount = data.MemberCount,
                            MessageCount = data.MessageCount,
                            ReactionAdded = data.ReactionAdded,
                            ReactionRemoved = data.ReactionRemoved,
                            Date = selectedDate,
                        });

                        await context.ServerReactionStats.AddRangeAsync(data.Reactions.Select(r => new Common.Models.ServerReactionStats {
                            Id = serverId,
                            Date = selectedDate,
                            Reaction = r.Key,
                            Amount = r.Value.ReactionCount,
                        }));

                        await context.ServerChannelStats.AddRangeAsync(data.Channels.Select(r => new Common.Models.ServerChannelStats {
                            Id = serverId,
                            Date = selectedDate,
                            ChannelId = long.Parse(r.Key),
                            ReactionCount = r.Value.ReactionCount,
                            MessageCount = r.Value.MessageCount,
                        }));

                        await context.ServerUserStats.AddRangeAsync(data.Users.Select(r => new Common.Models.ServerUserStats {
                            Id = serverId,
                            Date = selectedDate,
                            UserId = long.Parse(r.Key),
                            ReactionCount = r.Value.ReactionCount,
                            MessageCount = r.Value.MessageCount,
                        }));

                        await context.SaveChangesAsync();
                    }
                }
            } catch (Exception ex) {
                logger.LogError(ex, $"[{nameof(NotABotModule)}] Error when running tasks.");
            }
        }

        public override bool ShouldRunNow(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            return currentEnv switch {
                LumenModuleRunsOnFlag.API => date.Second == 0 && date.Minute == 30 && date.Hour == 5,
                _ => false,
            };
        }

        public override Task ShutdownAsync() {
            // Nothing to do
            return Task.CompletedTask;
        }

        public static new void SetupServices(LumenModuleRunsOnFlag currentEnv, IServiceCollection serviceCollection, string? postgresConnectionString) {
            if (currentEnv == LumenModuleRunsOnFlag.API) {
                serviceCollection.AddDbContext<NotABotContext>(o => o.UseNpgsql(postgresConnectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", NotABotContext.SCHEMA_NAME)));
            }
        }

        public override Type GetDatabaseContextType() {
            return typeof(NotABotContext);
        }
    }
}
