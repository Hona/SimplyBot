using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimplyBotUI.Modules
{
    public class DiscordModule : ExtraModuleBase
    {
        // TODO: Add functions to get all discord info (user info, server info, role info, channel)
        [Command("roleinfo")]
        public async Task GetRoleInfo(string roleParam)
        {
            var roles = Context.Guild.Roles.Where(x => x.Name.ToLower().Contains(roleParam.ToLower()));
            foreach (var role in roles.Take(5))
            {
                var builder = new EmbedBuilder{Title = "@"+role.Name};
                builder.AddInlineField("Separate from @everyone", role.IsHoisted);
                builder.AddInlineField("Mentionable", role.IsMentionable);
                builder.AddInlineField("Hierarchical position", role.Position);
                builder.AddInlineField("Guild", role.Guild);
                builder.AddInlineField("Created at", role.CreatedAt.ToString("d"));
                builder.AddInlineField("Permissions", PermissionsToString(role.Permissions));
                builder.WithColor(role.Color);
                builder.WithFooter(role.Id.ToString());
                await ReplyEmbed(builder);
            }
        }
        [Command("userinfo")]
        public async Task GetUserInfo(SocketGuildUser userParam)
        {

                var builder = new EmbedBuilder { Title = "@" + userParam.Username };
            builder.AddInlineField("Username", userParam.Username + "#" + userParam.DiscriminatorValue + NicknameString(userParam.Nickname));
            builder.AddInlineField("Bot", userParam.IsBot);
            if (userParam.JoinedAt != null) builder.AddInlineField("Joined Server", userParam.JoinedAt.Value.ToString("d"));
            builder.AddInlineField("Created Account", userParam.CreatedAt.ToString("d"));
            if (userParam.Game != null) builder.AddInlineField("Game", userParam.Game.Value.Name);
            builder.AddInlineField("Roles", userParam.Roles.Aggregate("", (currentString, nextRole) => currentString + nextRole.Mention + ", "));
            builder.AddInlineField("Permissions", PermissionsToString(userParam.GuildPermissions));
            if (userParam.Hierarchy == int.MaxValue)
            {
                builder.WithColor(Context.Guild.Roles.OrderByDescending(x=>x.Position).First().Color);
            }
            else
            {
                var role = Context.Guild.Roles.First(x => x.Position == userParam.Hierarchy);
                if (role != null)
                {
                    builder.WithColor(role.Color);
                }
            }
                builder.WithFooter(userParam.Id.ToString());
                await ReplyEmbed(builder);
           
        }

        [Command("serverinfo")]
        public async Task GetServerInfo()
        {
            if (Context.Guild is SocketGuild server)
            {
                var builder = new EmbedBuilder {Title = server.Name};
                builder.AddInlineField("Channels", server.Channels.Count);
                builder.AddInlineField("Created", server.CreatedAt.ToString("d"));
                builder.AddInlineField("Default Notifications", server.DefaultMessageNotifications.ToString());
                builder.AddInlineField("Members", server.MemberCount);
                builder.AddInlineField("2FA", server.MfaLevel.ToString());
                builder.AddInlineField("Owner", server.Owner.Username);
                builder.WithFooter(server.Id.ToString());
                await ReplyEmbed(builder);
            }
        }
        private string NicknameString(string nickname)=> string.IsNullOrWhiteSpace(nickname) ? string.Empty:$" ({nickname})";
        private string PermissionsToString(GuildPermissions perms) => perms.ToList().Aggregate("", (currentString, nextPermission) => currentString + nextPermission.ToString() + ", ").TrimEnd(' ', ',');
    }
}
