using LolEsportsMatches.DataAccess.Games;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.DataAccess.Teams;
using LolEsportsMatches.Services.GameResults;
using LolEsportsMatches.Services.Leagues;
using LolEsportsMatches.Services.Teams;

namespace LolEsportsMatches.Services.LifeTimeUpdated
{
    internal static class ObjectsMapper
    {
        internal static TeamInfo MapTeam(TeamTransferObject dto) =>
            new()
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Name = dto.Name,
                Code = dto.Code,
                Image = dto.Image,
                Region = dto.Region,
            };

        internal static TeamTransferObject MapTeam(TeamInfo team) =>
            new()
            {
                Id = team.Id,
                Slug = team.Slug,
                Name = team.Name,
                Code = team.Code,
                Image = team.Image,
                Region = team.Region,
            };

        internal static TeamDetailedInfo MapTeam(LolesportsApiAccess.Entities.TeamInfoTransferObject dto) =>
            new()
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Name = dto.Name,
                Code = dto.Code,
                Image = dto.Image,
                HomeLeague = dto.HomeLeague,
                Region = dto.Region,
                Players = dto.Players.Select(p => new PlayerInfo
                {
                    SummonerName = p.SummonerName,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Image = p.Image,
                    Role = p.Role,
                }).ToList(),
            };

        internal static LeagueTransferObject MapLeague(LeagueInfo info) =>
            new()
            {
                Id = info.Id,
                Slug = info.Slug,
                Name = info.Name,
                Image = info.Image,
            };

        internal static LeagueInfo MapLeague(LeagueTransferObject dto) =>
            new()
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Name = dto.Name,
                Image = dto.Image,
            };

        internal static ChampionIngameStat MapChampStat(LolesportsApiAccess.Entities.ChampsStatTransferObject? dto) =>
            dto is not null
                ? new()
                {
                    Level = dto.Level,
                    Kills = dto.Kills,
                    Deaths = dto.Deaths,
                    Assists = dto.Assists,
                    Gold = dto.TotalGoldEarned,
                    ItemsId = dto.Items,
                    FirstMainPerkId = dto.FirstMainPerkId,
                    SubPerkId = dto.SubPerkId,
                    Abilities = dto.Abilities,
                }
                : new()
                {
                    Level = 1,
                    Kills = 0,
                    Deaths = 0,
                    Assists = 0,
                    Gold = 0,
                    ItemsId = Enumerable.Repeat<int>(0, 6).ToList(),
                    FirstMainPerkId = 0,
                    SubPerkId = 0,
                    Abilities = new(),
                };

        internal static GameResult MapGame(GameTransferObject dto) =>
            new()
            {
                Id = dto.Id,
                ChampionsBlue = dto.ChampionsBlue,
                ChampionsRed = dto.ChampionsRed,
                KillsBlue = dto.KillsBlue,
                KillsRed = dto.KillsRed,
                GameDate = dto.GameDate,
                BlueTeam = MapTeam(dto.TeamBlue),
                RedTeam = MapTeam(dto.TeamRed),
                League = MapLeague(dto.League),
            };

        internal static GameTransferObject MapGame(GameResult game) =>
            new()
            {
                Id = game.Id,
                ChampionsBlue = game.ChampionsBlue,
                ChampionsRed = game.ChampionsRed,
                KillsBlue = (short)game.KillsBlue,
                KillsRed = (short)game.KillsRed,
                GameDate = game.GameDate,
                League = MapLeague(game.League),
                TeamBlue = MapTeam(game.BlueTeam),
                TeamRed = MapTeam(game.RedTeam),
            };

        internal static GameTransferObject MapGame(LolesportsApiAccess.Entities.GameResultTransferObject e, string leagueId)
        {
            return new GameTransferObject
            {
                Id = e.Id,
                League = new LeagueTransferObject { Id = leagueId },
                ChampionsBlue = e.TeamBlueChampions,
                ChampionsRed = e.TeamRedChampions,
                TeamBlue = new TeamTransferObject { Id = e.TeamBlueId },
                TeamRed = new TeamTransferObject { Id = e.TeamRedId },
                KillsBlue = e.TeamBlueKills,
                KillsRed = e.TeamRedKills,
                GameDate = e.GameDate,
            };
        }

        internal static GameDetailedResult MapGame(GameResult g, LolesportsApiAccess.Entities.GameDetailedStatTransferObject? stat) =>
            new()
            {
                Id = g.Id,
                League = g.League,
                BlueTeam = g.BlueTeam,
                RedTeam = g.RedTeam,
                ChampionsBlue = g.ChampionsBlue,
                ChampionsRed = g.ChampionsRed,
                KillsBlue = g.KillsBlue,
                KillsRed = g.KillsRed,
                GameDate = g.GameDate,
                ChampionsBlueStat =
                    stat?.ChampsDetails.Take(5).Select(dto => MapChampStat(dto)).ToArray()
                    ?? Enumerable.Repeat<LolesportsApiAccess.Entities.ChampsStatTransferObject?>(null, 5).Select(dto => MapChampStat(dto)).ToArray(),
                ChampionsRedStat =
                    stat?.ChampsDetails.Skip(5).Select(dto => MapChampStat(dto)).ToArray()
                    ?? Enumerable.Repeat<LolesportsApiAccess.Entities.ChampsStatTransferObject?>(null, 5).Select(dto => MapChampStat(dto)).ToArray(),
            };
    }
}
