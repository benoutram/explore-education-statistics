﻿// <auto-generated />
using System;
using GovUk.Education.ExploreEducationStatistics.Content.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GovUk.Education.ExploreEducationStatistics.Content.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190102152720_Slugs")]
    partial class Slugs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Publication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<Guid>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Publications");

                    b.HasData(
                        new { Id = new Guid("cbbd299f-8297-44bc-92ac-558bcf51f8ad"), Slug = "pupil-absence-in-schools-in-england", Title = "Pupil absence in schools in England", TopicId = new Guid("1003fa5c-b60a-4036-a178-e3a69a81b852") }
                    );
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Themes");

                    b.HasData(
                        new { Id = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Slug = "schools", Title = "Schools" },
                        new { Id = new Guid("6412a76c-cf15-424f-8ebc-3a530132b1b3"), Slug = "schools", Title = "Social Care" },
                        new { Id = new Guid("bc08839f-2970-4f34-af2d-29608a48082f"), Slug = "16+", Title = "16+" }
                    );
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<Guid>("ThemeId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Topics");

                    b.HasData(
                        new { Id = new Guid("1003fa5c-b60a-4036-a178-e3a69a81b852"), Slug = "absence-and-exclusions", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "Absence and Exclusions" },
                        new { Id = new Guid("22c52d89-88c0-44b5-96c4-042f1bde6ddd"), Slug = "school-and-pupil-numbers", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "School and Pupil Numbers" },
                        new { Id = new Guid("734820b7-f80e-45c3-bb92-960edcc6faa5"), Slug = "capacity-admissions", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "Capacity Admissions" },
                        new { Id = new Guid("17b2e32c-ed2f-4896-852b-513cdf466769"), Slug = "results", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "Results" },
                        new { Id = new Guid("66ff5e67-36cf-4210-9ad2-632baeb4eca7"), Slug = "school-finance", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "School Finance" },
                        new { Id = new Guid("d5288137-e703-43a1-b634-d50fc9785cb9"), Slug = "teacher-numbers", ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"), Title = "Teacher Numbers" },
                        new { Id = new Guid("0b920c62-ff67-4cf1-89ec-0c74a364e6b4"), Slug = "number-of-children", ThemeId = new Guid("6412a76c-cf15-424f-8ebc-3a530132b1b3"), Title = "Number of Children" },
                        new { Id = new Guid("3bef5b2b-76a1-4be1-83b1-a3269245c610"), Slug = "vulnerable-children", ThemeId = new Guid("6412a76c-cf15-424f-8ebc-3a530132b1b3"), Title = "Vulnerable Children" },
                        new { Id = new Guid("6a0f4dce-ae62-4429-834e-dd67cee32860"), Slug = "further-education", ThemeId = new Guid("bc08839f-2970-4f34-af2d-29608a48082f"), Title = "Further Education" },
                        new { Id = new Guid("4c658598-450b-4493-b972-8812acd154a7"), Slug = "higher-education", ThemeId = new Guid("bc08839f-2970-4f34-af2d-29608a48082f"), Title = "Higher Education" }
                    );
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Publication", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Topic", "Topic")
                        .WithMany("Publications")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Topic", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Content.Api.Models.Theme", "Theme")
                        .WithMany("Topics")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
