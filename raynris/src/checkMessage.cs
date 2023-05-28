using Discord;
using Discord.WebSocket;

namespace src
{
    internal class checkingMessage
    {
        internal async Task checkMessage(SocketMessage arg)
        {
            var user = (IGuildUser)arg.Author;
            var userRoles = (SocketGuildUser)arg.Author;
            if (!user.IsBot)
            {
                if (arg.Channel.Name == "botchat")
                {
                    foreach (var use in userRoles.Roles)
                    {
                        if (use.ToString() != "Бот")
                        {   
                            await arg.DeleteAsync();
                        }   
                    }
                }

                foreach (var use in userRoles.Roles)
                {
                    if (use.ToString() == "poop")
                    {
                        var emoji = new Emoji("\ud83d\udca9");
                        await arg.AddReactionAsync(emoji);
                    }
                }


                if (arg.Content != "")
                {
                    var guild = arg.Channel as SocketGuildChannel;
                    var channel = guild.Guild.GetChannel(1110566281062666380) as IMessageChannel;

                    var embedBuilder = new EmbedBuilder().WithAuthor($"Автор: {arg.Author.Username}").WithDescription(
                                       $"**Канал**: {arg.Channel.Name}" +
                                       $"\n**Сообщение**: {arg.Content}" +
                                       $"\n \n **Время отправки**: {arg.CreatedAt.AddHours(3)}"
                                       ).WithColor(Color.Blue);
                    await channel.SendMessageAsync(embed: embedBuilder.Build());
                }
            }
        }
    }
}
