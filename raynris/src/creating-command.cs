using Discord.Net;
using Discord;
using Newtonsoft.Json;
using Discord.WebSocket;
using src.command;

namespace src
{
    internal class creating_command
    {
        DiscordSocketClient? _client;
        lolinfo lolinfo = new lolinfo();

        internal async Task Client_Ready()
        {
            var globalCommand = new SlashCommandBuilder()
                .WithName("roll")
                .WithDescription("Генерирует случайное число c 0 до 100");
            var lolInfoCheck = new SlashCommandBuilder()
                .WithName("lolinfo")
                .WithDescription("Показывает статистику игрока в League of Legends")
                .AddOption("nickname", ApplicationCommandOptionType.String, "Ваш никнейм в LoL", isRequired: true, maxLength: 30);
            try
            {
                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(lolInfoCheck.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
        internal async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "roll":
                    Random rnd = new Random();
                    int number = rnd.Next(0, 100);
                    await command.RespondAsync(number.ToString());
                    break;
                case "lolinfo":
                    await lolinfo.InfoFunction(command);
                    break;
            }
        }
    }
}
