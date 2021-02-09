﻿// <auto-generated />
using System;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations
{
    [DbContext(typeof(StatisticsDbContext))]
    [Migration("20210209094711_EES1836_RemoveObservationRowFilterItemIdentityId")]
    partial class EES1836_RemoveObservationRowFilterItemIdentityId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Common.Model.Data.BoundaryLevel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Published")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BoundaryLevel");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Hint")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("SubjectId");

                    b.ToTable("Filter");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterFootnote", b =>
                {
                    b.Property<Guid>("FilterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FilterId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FilterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FilterId");

                    b.ToTable("FilterGroup");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroupFootnote", b =>
                {
                    b.Property<Guid>("FilterGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FilterGroupId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterGroupFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FilterGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FilterGroupId");

                    b.ToTable("FilterItem");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItemFootnote", b =>
                {
                    b.Property<Guid>("FilterItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FilterItemId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterItemFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Footnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DecimalPlaces")
                        .HasColumnType("int");

                    b.Property<Guid>("IndicatorGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IndicatorGroupId");

                    b.HasIndex("Name");

                    b.ToTable("Indicator");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorFootnote", b =>
                {
                    b.Property<Guid>("IndicatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IndicatorId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("IndicatorFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("IndicatorGroup");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CsvRow")
                        .HasColumnType("bigint");

                    b.Property<string>("GeographicLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Measures")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TimeIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GeographicLevel");

                    b.HasIndex("LocationId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TimeIdentifier");

                    b.HasIndex("Year");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ObservationFilterItem", b =>
                {
                    b.Property<Guid>("ObservationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FilterItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FilterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ObservationId", "FilterItemId");

                    b.HasIndex("FilterId");

                    b.HasIndex("FilterItemId");

                    b.ToTable("ObservationFilterItem");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Publication");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PreviousVersionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PublicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Published")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PreviousVersionId");

                    b.HasIndex("PublicationId");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ReleaseFootnote", b =>
                {
                    b.Property<Guid>("ReleaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReleaseId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("ReleaseFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ReleaseSubject", b =>
                {
                    b.Property<Guid>("ReleaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MetaGuidance")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReleaseId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("ReleaseSubject");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.SubjectFootnote", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FootnoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SubjectId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("SubjectFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Theme");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ThemeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("Filters")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("Filters")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany("FilterGroups")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroupFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", "FilterGroup")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("FilterGroups")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", "FilterGroup")
                        .WithMany("FilterItems")
                        .HasForeignKey("FilterGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItemFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", "FilterItem")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("FilterItems")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", "IndicatorGroup")
                        .WithMany("Indicators")
                        .HasForeignKey("IndicatorGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("Indicators")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", "Indicator")
                        .WithMany("Footnotes")
                        .HasForeignKey("IndicatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("IndicatorGroups")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", b =>
                {
                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Country", "Country", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Institution", "Institution", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthority", "LocalAuthority", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("OldCode")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.HasIndex("OldCode");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthorityDistrict", "LocalAuthorityDistrict", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalEnterprisePartnership", "LocalEnterprisePartnership", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Mat", "MultiAcademyTrust", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.MayoralCombinedAuthority", "MayoralCombinedAuthority", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.OpportunityArea", "OpportunityArea", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.ParliamentaryConstituency", "ParliamentaryConstituency", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.PlanningArea", "PlanningArea", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Region", "Region", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.RscRegion", "RscRegion", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Sponsor", "Sponsor", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Ward", "Ward", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("Observations")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ObservationFilterItem", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany()
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", "FilterItem")
                        .WithMany()
                        .HasForeignKey("FilterItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", "Observation")
                        .WithMany("FilterItems")
                        .HasForeignKey("ObservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", "Topic")
                        .WithMany("Publications")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", "Publication")
                        .WithMany("Releases")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ReleaseFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("Releases")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", "Release")
                        .WithMany("Footnotes")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ReleaseSubject", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", "Release")
                        .WithMany()
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.SubjectFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany("Subjects")
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("Footnotes")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Theme", "Theme")
                        .WithMany("Topics")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
