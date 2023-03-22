using Discord.WebSocket;
using Discord;

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
                /*if (arg.Content == "")
                {
                    Console.WriteLine();
                }
                using (StreamWriter writer = new StreamWriter("../../../log.txt", true))
                {
                    writer.WriteLine($"Чат: {arg.Channel.Name};\n\nИмя: {arg.Author};\nСообщение: {arg.Content};\nВремя: {arg.CreatedAt.AddHours(3)}\n");
                }*/
            }
        }
    }
}
