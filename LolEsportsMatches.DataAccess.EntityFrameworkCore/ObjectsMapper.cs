using LolData.Services.Champions;
using LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities;
using LolEsportsMatches.DataAccess.Games;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.DataAccess.Teams;
using DelegateDecompiler;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore
{
    internal static class ObjectsMapper
    {
        internal static ILolDataChampionsService? champService;

        internal static LeagueTransferObject MapLeague(LeagueEntity entity)
        {
            return new LeagueTransferObject
            {
                Id = entity.Id,
                Slug = entity.Slug,
                Name = entity.Name,
                Image = entity.Image,
                LastLoadedMatchId = entity.LastStoredMatchId,
            };
        }

        internal static LeagueEntity MapLeague(LeagueTransferObject dto)
        {
            return new LeagueEntity
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Name = dto.Name,
                Image = dto.Image,
                LastStoredMatchId = dto.LastLoadedMatchId,
            };
        }

        [Decompile]
        internal static TeamTransferObject MapTeam(this TeamEntity entity)
        {
            return new TeamTransferObject
            {
                Id = entity.Id,
                Slug = entity.Slug,
                Name = entity.Name,
                Code = entity.Code,
                Image = entity.Image,
                Region = entity.Region,
            };
        }

        internal static TeamEntity MapTeam(TeamTransferObject dto)
        {
            return new TeamEntity
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Name = dto.Name,
                Code = dto.Code,
                Image = dto.Image,
                Region = dto.Region,
            };
        }

        internal static GameEntity MapGame(GameTransferObject dto)
        {
            return new GameEntity
            {
                Id = dto.Id,
                LeagueId = dto.League.Id,
                TeamBlueId = dto.TeamBlue.Id,
                TeamRedId = dto.TeamRed.Id,
                TeamBlueKills = dto.KillsBlue,
                TeamRedKills = dto.KillsRed,
                GameDate = dto.GameDate,
                TeamBlueChampion1 = GetChampIndex(dto.ChampionsBlue[0]),
                TeamBlueChampion2 = GetChampIndex(dto.ChampionsBlue[1]),
                TeamBlueChampion3 = GetChampIndex(dto.ChampionsBlue[2]),
                TeamBlueChampion4 = GetChampIndex(dto.ChampionsBlue[3]),
                TeamBlueChampion5 = GetChampIndex(dto.ChampionsBlue[4]),
                TeamRedChampion1 = GetChampIndex(dto.ChampionsRed[0]),
                TeamRedChampion2 = GetChampIndex(dto.ChampionsRed[1]),
                TeamRedChampion3 = GetChampIndex(dto.ChampionsRed[2]),
                TeamRedChampion4 = GetChampIndex(dto.ChampionsRed[3]),
                TeamRedChampion5 = GetChampIndex(dto.ChampionsRed[4]),
            };
        }

        internal static GameTransferObject MapGame(GameEntity entity)
        {
            return new GameTransferObject
            {
                Id = entity.Id,
                KillsBlue = entity.TeamBlueKills,
                KillsRed = entity.TeamRedKills,
                GameDate = entity.GameDate,
                ChampionsBlue = new[]
                    {
                        GetChampId(entity.TeamBlueChampion1),
                        GetChampId(entity.TeamBlueChampion2),
                        GetChampId(entity.TeamBlueChampion3),
                        GetChampId(entity.TeamBlueChampion4),
                        GetChampId(entity.TeamBlueChampion5),
                    },
                ChampionsRed = new[]
                    {
                        GetChampId(entity.TeamRedChampion1),
                        GetChampId(entity.TeamRedChampion2),
                        GetChampId(entity.TeamRedChampion3),
                        GetChampId(entity.TeamRedChampion4),
                        GetChampId(entity.TeamRedChampion5),
                    },
                League = MapLeague(entity.League),
                TeamBlue = MapTeam(entity.TeamBlue),
                TeamRed = MapTeam(entity.TeamRed),
            };
        }

        internal static short GetChampIndex(string name) => champService!.GetChampion(name).Index;

        internal static string GetChampId(short index) => champService!.GetChampion(index).Id;
    }
}
