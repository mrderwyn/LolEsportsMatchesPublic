global using LolData.Services;
global using LolData.Services.Champions;
global using LolData.Services.DataDragon;
global using LolData.Services.Items;
global using LolData.Services.Runes;
global using LolEsportsMatches.Authorisation;
global using LolEsportsMatches.Authorisation.EntityFrameworkCore;
global using LolEsportsMatches.Authorisation.EntityFrameworkCore.Context;
global using LolEsportsMatches.DataAccess.EntityFrameworkCore;
global using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
global using LolEsportsMatches.Services.ErrorStorage;
global using LolEsportsMatches.Services.ErrorStorage.Logger;
global using LolEsportsMatches.Services.GameResults;
global using LolEsportsMatches.Services.Leagues;
global using LolEsportsMatches.Services.Teams;
global using LolEsportsMatchesApp;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

string? sqlStringConnection = builder.Configuration.GetConnectionString("SqlConnection");
string? authStringConnection = builder.Configuration.GetConnectionString("AuthConnection");
string errorStoragePath = builder.Configuration["Logging:ErrorJson:FilePath"];
string ddragonLocalPath = builder.Configuration["DataDragon:LocalPath"];


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/account/login");
        options.AccessDeniedPath = new PathString("/account/login");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Logging.AddFileLogger(builder.Configuration);

builder.Logging.AddErrorJsonLogger();
builder.Services.AddScoped<ErrorStorage>(provider => new ErrorStorage(errorStoragePath));

builder.Services.AddScoped<LolEsportsMatches.Services.LolesportsApiAccess.LolEsportsApiAccess>();
builder.Services.AddTransient<LolEsportsMatches.DataAccess.LolEsportsMatchesDataAccessFactory, EntityFrameworkDataAccessFactory>();
builder.Services.AddTransient<ILeaguesInfoService, LolEsportsMatches.Services.LifeTimeUpdated.LeaguesInfoService>();
builder.Services.AddTransient<IGameResultsService, LolEsportsMatches.Services.LifeTimeUpdated.GamesResultService>();
builder.Services.AddScoped<ITeamInfoService, LolEsportsMatches.Services.LifeTimeUpdated.TeamsInfoService>();
builder.Services.AddTransient<IAuthorisationService, AuthorisationService>();

builder.Services.AddSingleton<LolDataServicesFactory, DdragonServicesFactory>();
builder.Services.AddScoped<ILolDataChampionsService>(provider =>
{
    return provider.GetRequiredService<LolDataServicesFactory>().GetChampionsService();
});
builder.Services.AddScoped<ILolDataItemsService>(provider =>
{
    return provider.GetRequiredService<LolDataServicesFactory>().GetItemsService();
});
builder.Services.AddScoped<ILolDataRunesService>(provider =>
{
    return provider.GetRequiredService<LolDataServicesFactory>().GetRunesService();
});


builder.Services.AddDbContext<MatchHistoryContext>(opt => opt.UseSqlServer(sqlStringConnection));
builder.Services.AddDbContext<AuthContext>(opt => opt.UseSqlServer(authStringConnection));

WebApplication? app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
