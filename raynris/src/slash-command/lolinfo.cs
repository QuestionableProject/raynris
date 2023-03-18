using Discord.WebSocket;
using Discord;
using Newtonsoft.Json;

namespace src.command
{
    internal class lolinfo
    {
        static readonly HttpClient clientHttp = new HttpClient();

        class JsonInfo
        {
            public string? lolapiprofile { get; set; }
            public string? lolapiaccount { get; set; }
            public string? testapideveloper { get; set; }
            public string? lolapiru { get; set; }
            public string? lolapieurope { get; set; }
            public string? lolapimatch { get; set; }
        }
        class UserAccount
        {
            public string? summonerLevel { get; set; }
            public string? puuid { get; set; }
            public string? id { get; set; }
        }
        class UserProfile
        {
            public string? tier { get; set; }
            public string? rank { get; set; }
            public string? wins { get; set; }
            public string? losses { get; set; }
            public string? leaguePoints { get; set; }
        }
        class Info
        {
            public List<Participant>? participants { get; set; }
        }
        class Participant
        {
            public int assists { get; set; }
            public string? championName { get; set; }
            public int deaths { get; set; }
            public int kills { get; set; }
            public string? lane { get; set; }
            public int pentaKills { get; set; }
            public string? summonerName { get; set; }
            public bool win { get; set; }
        }
        class UserMatch
        {
            public Info? info { get; set; }
        }

        class Rang
        {
            public Color ColorRang = 0x404040;
        }
        internal async Task InfoFunction(SocketSlashCommand command)
        {
            Rang summonerRang = new Rang();
            string urlName = Uri.EscapeDataString(command.Data.Options.First().Value.ToString());
            string nickName = command.Data.Options.First().Value.ToString().Replace(" ", "");
            var url = JsonConvert.DeserializeObject<JsonInfo>(File.ReadAllText("../../../../application.json"));
            try
            {
                string responseGA = await clientHttp.GetStringAsync(@$"{url?.lolapiru}{url?.lolapiaccount}{urlName}?api_key={url?.testapideveloper}");
                var responseJsonGA = JsonConvert.DeserializeObject<UserAccount>(responseGA);

                string responseGP = await clientHttp.GetStringAsync(@$"{url?.lolapiru}{url?.lolapiprofile}{responseJsonGA?.id}?api_key={url?.testapideveloper}");
                var responseJsonGP = JsonConvert.DeserializeObject<UserProfile[]>(responseGP);

                string responseMID = await clientHttp.GetStringAsync(@$"{url?.lolapieurope}{url?.lolapimatch}by-puuid/{responseJsonGA?.puuid}/ids?start=0&count=1&api_key={url?.testapideveloper}");
                var responseJsonMID = JsonConvert.DeserializeObject<string[]>(responseMID);

                string responseM = await clientHttp.GetStringAsync(@$"{url?.lolapieurope}{url?.lolapimatch}{responseJsonMID[0]}?api_key={url?.testapideveloper}");
                var responseJsonM = JsonConvert.DeserializeObject<UserMatch>(responseM);

                if (responseJsonGP?.Length == 0)
                {
                    foreach (var f in responseJsonM?.info?.participants)
                    {
                        string winn = f.win ? "Победа" : "Поражение";
                        var embedBuilder = new EmbedBuilder().WithAuthor(command.Data.Options.First().Value.ToString()).WithDescription(
                            $"Уровень:  **{responseJsonGA?.summonerLevel}**" +
                            $"\nРанг: **Нет ранга**" +
                            $"\nПоследняя игра:\n```{winn}\nKDA: {f.kills}/{f.deaths}/{f.assists}\n{f.lane}/{f.championName}```" +
                            $"\nВинрейт всего:" +
                            $"\nВинрейт одиночная:" +
                            $"\nKDA всего:").WithColor(summonerRang.ColorRang);
                        await command.RespondAsync(embed: embedBuilder.Build());
                    }
                    return;
                }
                else
                {
                    foreach (var i in responseJsonGP)
                    {
                        switch (i.tier)
                        {
                            case "IRON":
                                summonerRang.ColorRang = 0x5e4831;
                                break;
                            case "BRONZE":
                                summonerRang.ColorRang = 0x85531d;
                                break;
                            case "GOLD":
                                summonerRang.ColorRang = 0xd9a300;
                                break;
                            case "PLATINUM":
                                summonerRang.ColorRang = 0x1d8540;
                                break;
                            case "DIAMOND":
                                summonerRang.ColorRang = 0x13669e;
                                break;
                            case "MASTER":
                                summonerRang.ColorRang = 0x9e1397;
                                break;
                            case "MASGRANDMASTER":
                                summonerRang.ColorRang = 0x9e1313;
                                break;
                            case "CHALLENGER":
                                summonerRang.ColorRang = 0x00f2ff;
                                break;
                        }
                        if (responseGP != "")
                        {
                            foreach (var f in responseJsonM?.info?.participants)
                            {
                                string winn = f.win ? "Победа" : "Поражение";
                                if (f?.summonerName.Replace(" ", "") == nickName)
                                {

                                    var embedBuilder = new EmbedBuilder().WithAuthor(command.Data.Options.First().Value.ToString()).WithDescription(
                                        $"Уровень:  **{responseJsonGA?.summonerLevel}**" +
                                        $"\nРанг: **{i.tier}  {i.rank}**" +
                                        $"\nLP: **{i.leaguePoints}**" +
                                        $"\nПобед: **{i.wins}** Поражений: **{i.losses}**" +
                                        $"\nПоследняя игра:\n```{winn}\nKDA: {f.kills}/{f.deaths}/{f.assists}\n{f.lane}/{f.championName}```" +
                                        $"\nВинрейт всего:" +
                                        $"\nВинрейт одиночная:" +
                                        $"\nKDA всего:").WithColor(summonerRang.ColorRang);
                                    await command.RespondAsync(embed: embedBuilder.Build());
                                }
                            }
                        }
                        else
                        {
                            var embedBuilder = new EmbedBuilder().WithAuthor(command.Data.Options.First().Value.ToString()).WithDescription(
                                       $"Уровень:  **{responseJsonGA?.summonerLevel}**" +
                                       $"\nРанг: **{i.tier}  {i.rank}**" +
                                       $"\nLP: **{i.leaguePoints}**" +
                                       $"\nПобед: **{i.wins}** Поражений: **{i.losses}**" +
                                       $"\nПоследняя игра:\n```У призывателя нет последней игры" +
                                       $"\nВинрейт всего:" +
                                       $"\nВинрейт одиночная:" +
                                       $"\nKDA всего:").WithColor(summonerRang.ColorRang);
                            await command.RespondAsync(embed: embedBuilder.Build());
                        }
                    }
                }
            }
            catch
            {
                var errorEmbed = new EmbedBuilder().WithDescription($"Призыватель **{command.Data.Options.First().Value}** не найден").WithColor(Color.Red);
                await command.RespondAsync(embed: errorEmbed.Build());
            }
        }
    }
}
