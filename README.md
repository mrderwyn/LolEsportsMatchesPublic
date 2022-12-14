# LolEsportsMatches
 Published at: [lolesportmatches.somee.com](http://lolesportmatches.somee.com/)
 
This resource is designed for convenient, simple and quick viewing of champion picks on the League of Legends pro scene. You can also see the details of builds in a specific game.

| Project name | Description |
|--------------|-------------|
| LolEsportMatchesApp | Web App for viewing picks on the LoL pro scene |
| LolData.Services | Interfaces for LoL data services (champions, items, runes) |
| LolData.Services.DataDragon | Implementation of LolData.Services interfaces using Riot DataDragon Api |
| LolEsportMatches.Services | Interfaces for Esport results services (leagues, teams, games) |
| LolEsportMatches.Services.LifeTimeUpdated | Implementation of LolEsportMatches.Services |
| LolEsportMatches.Services.ErrorStorage | Important errors storage (such as unparsed game results, unsaved team information, etc) |
| LolEsportMatches.Services.LolesportApiAccess | Provides access to LoL Live Esport API |
| LolEsportMatches.Authorization | Interfaces for authorization |
| LolEsportMatches.Authorisation.EntityFrameworkCore | Simple admin authorization |
| LolEsportMatches.DataAccess | DAO for leagues, teams and games information |
| LolEsportMatches.DataAccess.EntityFrameworkCore | DAO implementation using EF Core |