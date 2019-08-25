﻿// <auto-generated />
using System;
using ActivityCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActivityCenter.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ActivityCenter.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnName("duration");

                    b.Property<string>("DurationUnits")
                        .IsRequired()
                        .HasColumnName("duration_units")
                        .HasColumnType("VARCHAR(10)");

                    b.Property<string>("EventTitle")
                        .IsRequired()
                        .HasColumnName("event_title")
                        .HasColumnType("VARCHAR(45)")
                        .HasMaxLength(45);

                    b.Property<DateTime>("Time")
                        .HasColumnName("time")
                        .HasColumnType("DATETIME");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("created_at")
                        .HasColumnName("created_at")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("updated_at")
                        .HasColumnName("updated_at")
                        .HasColumnType("DATETIME");

                    b.HasKey("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ActivityCenter.Models.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("EventId")
                        .HasColumnName("event_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("ParticipantId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("ActivityCenter.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("Birthday")
                        .HasColumnName("birthday")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("VARCHAR(45)")
                        .HasMaxLength(45);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasColumnType("VARCHAR(45)")
                        .HasMaxLength(45);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<DateTime>("created_at")
                        .HasColumnName("created_at")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("updated_at")
                        .HasColumnName("updated_at")
                        .HasColumnType("DATETIME");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ActivityCenter.Models.Event", b =>
                {
                    b.HasOne("ActivityCenter.Models.User", "Creator")
                        .WithMany("CreatedEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ActivityCenter.Models.Participant", b =>
                {
                    b.HasOne("ActivityCenter.Models.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ActivityCenter.Models.User", "User")
                        .WithMany("Participants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
