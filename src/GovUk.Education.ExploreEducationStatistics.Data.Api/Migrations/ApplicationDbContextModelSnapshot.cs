﻿// <auto-generated />

using System;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.BoundaryLevel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Label");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<DateTime>("Published");

                    b.HasKey("Id");

                    b.ToTable("BoundaryLevel");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hint");

                    b.Property<string>("Label");

                    b.Property<string>("Name");

                    b.Property<long>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("SubjectId");

                    b.ToTable("Filter");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterFootnote", b =>
                {
                    b.Property<long>("FilterId");

                    b.Property<long>("FootnoteId");

                    b.HasKey("FilterId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FilterId");

                    b.Property<string>("Label");

                    b.HasKey("Id");

                    b.HasIndex("FilterId");

                    b.ToTable("FilterGroup");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroupFootnote", b =>
                {
                    b.Property<long>("FilterGroupId");

                    b.Property<long>("FootnoteId");

                    b.HasKey("FilterGroupId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterGroupFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FilterGroupId");

                    b.Property<string>("Label");

                    b.HasKey("Id");

                    b.HasIndex("FilterGroupId");

                    b.ToTable("FilterItem");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItemFootnote", b =>
                {
                    b.Property<long>("FilterItemId");

                    b.Property<long>("FootnoteId");

                    b.HasKey("FilterItemId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("FilterItemFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.HasKey("Id");

                    b.ToTable("Footnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("IndicatorGroupId");

                    b.Property<string>("Label");

                    b.Property<string>("Name");

                    b.Property<string>("Unit")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IndicatorGroupId");

                    b.HasIndex("Name");

                    b.ToTable("Indicator");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorFootnote", b =>
                {
                    b.Property<long>("IndicatorId");

                    b.Property<long>("FootnoteId");

                    b.HasKey("IndicatorId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("IndicatorFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Label");

                    b.Property<long>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("IndicatorGroup");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GeographicLevel")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<long>("LocationId");

                    b.Property<string>("Measures");

                    b.Property<string>("ProviderUrn");

                    b.Property<string>("SchoolLaEstab");

                    b.Property<long>("SubjectId");

                    b.Property<string>("TimeIdentifier")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("GeographicLevel");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProviderUrn");

                    b.HasIndex("SchoolLaEstab");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TimeIdentifier");

                    b.HasIndex("Year");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ObservationFilterItem", b =>
                {
                    b.Property<long>("ObservationId");

                    b.Property<long>("FilterItemId");

                    b.HasKey("ObservationId", "FilterItemId");

                    b.HasIndex("FilterItemId");

                    b.ToTable("ObservationFilterItem");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Provider", b =>
                {
                    b.Property<string>("Urn")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Ukprn");

                    b.Property<string>("Upin");

                    b.HasKey("Urn");

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.Property<Guid>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Publication");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PublicationId");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("PublicationId");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.School", b =>
                {
                    b.Property<string>("LaEstab")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AcademyOpenDate");

                    b.Property<string>("AcademyType");

                    b.Property<string>("Estab");

                    b.Property<string>("Name");

                    b.Property<string>("Postcode");

                    b.Property<string>("Urn");

                    b.HasKey("LaEstab");

                    b.ToTable("School");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<Guid>("ReleaseId");

                    b.HasKey("Id");

                    b.HasIndex("ReleaseId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.SubjectFootnote", b =>
                {
                    b.Property<long>("SubjectId");

                    b.Property<long>("FootnoteId");

                    b.HasKey("SubjectId", "FootnoteId");

                    b.HasIndex("FootnoteId");

                    b.ToTable("SubjectFootnote");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Theme");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<Guid>("ThemeId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany()
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany("FilterGroups")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroupFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", "FilterGroup")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany()
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", "FilterGroup")
                        .WithMany("FilterItems")
                        .HasForeignKey("FilterGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItemFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", "FilterItem")
                        .WithMany("Footnotes")
                        .HasForeignKey("FilterItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany()
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", "IndicatorGroup")
                        .WithMany("Indicators")
                        .HasForeignKey("IndicatorGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany()
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", "Indicator")
                        .WithMany("Footnotes")
                        .HasForeignKey("IndicatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", b =>
                {
                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Country", "Country", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("Country")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Country", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Institution", "Institution", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("Institution")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Institution", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthority", "LocalAuthority", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.Property<string>("Old_Code");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("LocalAuthority")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthority", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthorityDistrict", "LocalAuthorityDistrict", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("LocalAuthorityDistrict")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalAuthorityDistrict", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalEnterprisePartnership", "LocalEnterprisePartnership", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("LocalEnterprisePartnership")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.LocalEnterprisePartnership", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Mat", "MultiAcademyTrust", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("MultiAcademyTrust")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Mat", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.MayoralCombinedAuthority", "MayoralCombinedAuthority", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("MayoralCombinedAuthority")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.MayoralCombinedAuthority", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.OpportunityArea", "OpportunityArea", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("OpportunityArea")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.OpportunityArea", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.ParliamentaryConstituency", "ParliamentaryConstituency", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("ParliamentaryConstituency")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.ParliamentaryConstituency", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Region", "Region", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("Region")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Region", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.RscRegion", "RscRegion", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("RscRegion")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.RscRegion", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Sponsor", "Sponsor", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("Sponsor")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Sponsor", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Ward", "Ward", b1 =>
                        {
                            b1.Property<long>("LocationId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code");

                            b1.Property<string>("Name");

                            b1.HasKey("LocationId");

                            b1.HasIndex("Code");

                            b1.ToTable("Location");

                            b1.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location")
                                .WithOne("Ward")
                                .HasForeignKey("GovUk.Education.ExploreEducationStatistics.Data.Model.Ward", "LocationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Location", "Location")
                        .WithMany("Observations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Provider", "Provider")
                        .WithMany("Observations")
                        .HasForeignKey("ProviderUrn");

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.School", "School")
                        .WithMany("Observations")
                        .HasForeignKey("SchoolLaEstab");

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("Observations")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.ObservationFilterItem", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", "FilterItem")
                        .WithMany()
                        .HasForeignKey("FilterItemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Observation", "Observation")
                        .WithMany("FilterItems")
                        .HasForeignKey("ObservationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", "Topic")
                        .WithMany("Publications")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Publication", "Publication")
                        .WithMany("Releases")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Release", "Release")
                        .WithMany("Subjects")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.SubjectFootnote", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Footnote", "Footnote")
                        .WithMany()
                        .HasForeignKey("FootnoteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany("Footnotes")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Topic", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Theme", "Theme")
                        .WithMany("Topics")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
