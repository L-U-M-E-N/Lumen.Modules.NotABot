using Dysnomia.Common.WebAPIWrapper.Helpers;

using System.Text.Json.Serialization;

namespace Lumen.Modules.NotABot.Module.Dto {
    public class NotABotAPIDto {
        public long Date { get; set; }
        public int MemberLeft { get; set; }
        public int MemberJoined { get; set; }
        public int MemberCount { get; set; }
        public int ReactionAdded { get; set; }
        public int ReactionRemoved { get; set; }
        public int MessageCount { get; set; }

        [JsonConverter(typeof(EmptyArrayToObjectConverter<Dictionary<string, NotABotAPIChannelDto>>))] // NaB can return [] here ...
        public IDictionary<string, NotABotAPIChannelDto> Channels { get; set; } = null!;
        [JsonConverter(typeof(EmptyArrayToObjectConverter<Dictionary<string, NotABotAPIReactionDto>>))] // NaB can return [] here ...
        public IDictionary<string, NotABotAPIReactionDto> Reactions { get; set; } = null!;
        [JsonConverter(typeof(EmptyArrayToObjectConverter<Dictionary<string, NotABotAPIUserDto>>))] // NaB can return [] here ...
        public IDictionary<string, NotABotAPIUserDto> Users { get; set; } = null!;
    }
}
