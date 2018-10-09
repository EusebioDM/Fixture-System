﻿// <auto-generated />
using System;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EirinDuran.WebApi.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EirinDuran.Entities.CommentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EncounterEntityId");

                    b.Property<string>("Message");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("EncounterEntityId");

                    b.HasIndex("UserName");

                    b.ToTable("CommentEntity");
                });

            modelBuilder.Entity("EirinDuran.Entities.EncounterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AwayTeamName");

                    b.Property<string>("AwayTeamSportName");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("HomeTeamName");

                    b.Property<string>("HomeTeamSportName");

                    b.Property<string>("SportName");

                    b.HasKey("Id");

                    b.HasIndex("SportName");

                    b.HasIndex("AwayTeamName", "AwayTeamSportName");

                    b.HasIndex("HomeTeamName", "HomeTeamSportName");

                    b.ToTable("Encounters");
                });

            modelBuilder.Entity("EirinDuran.Entities.SportEntity", b =>
                {
                    b.Property<string>("SportName")
                        .ValueGeneratedOnAdd();

                    b.HasKey("SportName");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamEntity", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("SportName");

                    b.Property<byte[]>("Logo");

                    b.HasKey("Name", "SportName");

                    b.HasIndex("SportName");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamUserEntity", b =>
                {
                    b.Property<string>("TeamName");

                    b.Property<string>("UserNamee");

                    b.Property<string>("SportName");

                    b.Property<string>("UserName");

                    b.HasKey("TeamName", "UserNamee");

                    b.HasIndex("UserName");

                    b.HasIndex("TeamName", "SportName")
                        .IsUnique()
                        .HasFilter("[SportName] IS NOT NULL");

                    b.ToTable("TeamUsers");
                });

            modelBuilder.Entity("EirinDuran.Entities.UserEntity", b =>
                {
                    b.Property<string>("UserName")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Mail");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("Surname");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EirinDuran.Entities.CommentEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.EncounterEntity")
                        .WithMany("Comments")
                        .HasForeignKey("EncounterEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EirinDuran.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EirinDuran.Entities.EncounterEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.SportEntity", "Sport")
                        .WithMany()
                        .HasForeignKey("SportName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EirinDuran.Entities.TeamEntity", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamName", "AwayTeamSportName");

                    b.HasOne("EirinDuran.Entities.TeamEntity", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamName", "HomeTeamSportName");
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.SportEntity", "Sport")
                        .WithMany()
                        .HasForeignKey("SportName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamUserEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.UserEntity", "User")
                        .WithMany("TeamUsers")
                        .HasForeignKey("UserName");

                    b.HasOne("EirinDuran.Entities.TeamEntity", "Team")
                        .WithOne()
                        .HasForeignKey("EirinDuran.Entities.TeamUserEntity", "TeamName", "SportName");
                });
#pragma warning restore 612, 618
        }
    }
}
