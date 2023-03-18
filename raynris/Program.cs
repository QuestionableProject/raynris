using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using src;

namespace Rayanris
{
    class Program {
        class JsonInfo
        {
            public string? Token { get; set; }
        }

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        DiscordSocketClient? _client;   

        creating_command slashcommand = new creating_command();
        checkingMessage message = new checkingMessage();
        creating_command creatingCommand = new creating_command();
        private async Task MainAsync() {
            _client = new DiscordSocketClient();

            _client.SlashCommandExecuted += slashcommand.SlashCommandHandler;
            _client.Ready += creatingCommand.Client_Ready;
            _client.MessageReceived += message.checkMessage;

            var botToken = JsonConvert.DeserializeObject<JsonInfo>(File.ReadAllText("../../../../application.json"));
            await _client.LoginAsync(TokenType.Bot, botToken?.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}