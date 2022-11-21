﻿// <auto-generated />
using System;
using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(MatchHistoryContext))]
    partial class MatchHistoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.GameEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("game_id");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("game_date");

                    b.Property<string>("LeagueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("league_id");

                    b.Property<short>("TeamBlueChampion1")
                        .HasColumnType("smallint")
                        .HasColumnName("team_blue_champion_1");

                    b.Property<short>("TeamBlueChampion2")
                        .HasColumnType("smallint")
                        .HasColumnName("team_blue_champion_2");

                    b.Property<short>("TeamBlueChampion3")
                        .HasColumnType("smallint")
                        .HasColumnName("team_blue_champion_3");

                    b.Property<short>("TeamBlueChampion4")
                        .HasColumnType("smallint")
                        .HasColumnName("team_blue_champion_4");

                    b.Property<short>("TeamBlueChampion5")
                        .HasColumnType("smallint")
                        .HasColumnName("team_blue_champion_5");

                    b.Property<string>("TeamBlueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("team_blue_id");

                    b.Property<short>("TeamBlueKills")
                        .HasColumnType("smallint")
                        .HasColumnName("team_ blue_kills");

                    b.Property<short>("TeamRedChampion1")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_champion_1");

                    b.Property<short>("TeamRedChampion2")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_champion_2");

                    b.Property<short>("TeamRedChampion3")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_champion_3");

                    b.Property<short>("TeamRedChampion4")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_champion_4");

                    b.Property<short>("TeamRedChampion5")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_champion_5");

                    b.Property<string>("TeamRedId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("team_red_id");

                    b.Property<short>("TeamRedKills")
                        .HasColumnType("smallint")
                        .HasColumnName("team_red_kills");

                    b.HasKey("Id");

                    b.HasIndex("GameDate");

                    b.HasIndex("LeagueId");

                    b.HasIndex("TeamBlueId");

                    b.HasIndex("TeamRedId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.LeagueEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("league_id");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("LastStoredMatchId")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_stored_match_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("slug");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.TeamEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("team_id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("code");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("region");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("slug");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.GameEntity", b =>
                {
                    b.HasOne("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.LeagueEntity", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.TeamEntity", "TeamBlue")
                        .WithMany()
                        .HasForeignKey("TeamBlueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities.TeamEntity", "TeamRed")
                        .WithMany()
                        .HasForeignKey("TeamRedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("TeamBlue");

                    b.Navigation("TeamRed");
                });
#pragma warning restore 612, 618
        }
    }
}
