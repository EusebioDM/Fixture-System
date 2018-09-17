﻿// <auto-generated />
using System;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EirinDuran.WebApi.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20180917165433_Updated repos and entities")]
    partial class Updatedreposandentities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EirinDuran.Entities.EncounterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<Guid?>("SportId");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Encounters");
                });

            modelBuilder.Entity("EirinDuran.Entities.SportEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EncounterEntityId");

                    b.Property<byte[]>("Logo");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("SportEntityId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("EncounterEntityId");

                    b.HasIndex("SportEntityId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("EirinDuran.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Mail");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("Surname");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EirinDuran.Entities.EncounterEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.SportEntity", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");
                });

            modelBuilder.Entity("EirinDuran.Entities.TeamEntity", b =>
                {
                    b.HasOne("EirinDuran.Entities.EncounterEntity")
                        .WithMany("Teams")
                        .HasForeignKey("EncounterEntityId");

                    b.HasOne("EirinDuran.Entities.SportEntity")
                        .WithMany("Teams")
                        .HasForeignKey("SportEntityId");
                });
#pragma warning restore 612, 618
        }
    }
}
