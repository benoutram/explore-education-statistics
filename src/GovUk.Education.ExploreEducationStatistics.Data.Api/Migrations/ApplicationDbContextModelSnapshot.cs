﻿// <auto-generated />
using System;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hint");

                    b.Property<string>("Label");

                    b.Property<long>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Filter");
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

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("IndicatorGroupId");

                    b.Property<string>("Label");

                    b.Property<string>("Unit")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IndicatorGroupId");

                    b.ToTable("Indicator");
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("cbbd299f-8297-44bc-92ac-558bcf51f8ad"),
                            Slug = "pupil-absence-in-schools-in-england",
                            Title = "Pupil absence in schools in England",
                            TopicId = new Guid("67c249de-1cca-446e-8ccb-dcdac542f460")
                        },
                        new
                        {
                            Id = new Guid("bf2b4284-6b84-46b0-aaaa-a2e0a23be2a9"),
                            Slug = "permanent-and-fixed-period-exclusions-in-england",
                            Title = "Permanent and fixed-period exclusions in England",
                            TopicId = new Guid("77941b7d-bbd6-4069-9107-565af89e2dec")
                        },
                        new
                        {
                            Id = new Guid("66c8e9db-8bf2-4b0b-b094-cfab25c20b05"),
                            Slug = "secondary-and-primary-schools-applications-and-offers",
                            Title = "Secondary and primary schools applications and offers",
                            TopicId = new Guid("1a9636e4-29d5-4c90-8c07-f41db8dd019c")
                        },
                        new
                        {
                            Id = new Guid("fcda2962-82a6-4052-afa2-ea398c53c85f"),
                            Slug = "early-years-foundation-stage-profile-results",
                            Title = "Early years foundation stage profile results",
                            TopicId = new Guid("17b2e32c-ed2f-4896-852b-513cdf466769")
                        });
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5"),
                            PublicationId = new Guid("cbbd299f-8297-44bc-92ac-558bcf51f8ad"),
                            ReleaseDate = new DateTime(2018, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "2016-17",
                            Title = "2016 to 2017"
                        },
                        new
                        {
                            Id = new Guid("47299b78-a4a6-4f7e-a86f-4713f4a0599a"),
                            PublicationId = new Guid("fcda2962-82a6-4052-afa2-ea398c53c85f"),
                            ReleaseDate = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "2017-18",
                            Title = "2017 to 2018"
                        },
                        new
                        {
                            Id = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278"),
                            PublicationId = new Guid("bf2b4284-6b84-46b0-aaaa-a2e0a23be2a9"),
                            ReleaseDate = new DateTime(2018, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "2016-17",
                            Title = "2016 to 2017"
                        },
                        new
                        {
                            Id = new Guid("63227211-7cb3-408c-b5c2-40d3d7cb2717"),
                            PublicationId = new Guid("66c8e9db-8bf2-4b0b-b094-cfab25c20b05"),
                            ReleaseDate = new DateTime(2019, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "2018",
                            Title = "2018"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Absence by characteristic",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Absence by geographic level",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Absence by term",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 4L,
                            Name = "Absence for four year olds",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 5L,
                            Name = "Absence in prus",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 6L,
                            Name = "Absence number missing at least one session by reason",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 7L,
                            Name = "Absence rate percent bands",
                            ReleaseId = new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5")
                        },
                        new
                        {
                            Id = 8L,
                            Name = "ELG underlying data 2013 - 2018",
                            ReleaseId = new Guid("47299b78-a4a6-4f7e-a86f-4713f4a0599a")
                        },
                        new
                        {
                            Id = 9L,
                            Name = "Areas of learning underlying data 2013 - 2018",
                            ReleaseId = new Guid("47299b78-a4a6-4f7e-a86f-4713f4a0599a")
                        },
                        new
                        {
                            Id = 10L,
                            Name = "APS GLD ELG underlying data 2013 - 2018",
                            ReleaseId = new Guid("47299b78-a4a6-4f7e-a86f-4713f4a0599a")
                        },
                        new
                        {
                            Id = 11L,
                            Name = "Exclusions by characteristic",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 12L,
                            Name = "Exclusions by geographic level",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 13L,
                            Name = "Exclusions by reason",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 14L,
                            Name = "Duration of fixed exclusions",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 15L,
                            Name = "Number of fixed exclusions",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 16L,
                            Name = "Total days missed due to fixed period exclusions",
                            ReleaseId = new Guid("e7774a74-1f62-4b76-b9b5-84f14dac7278")
                        },
                        new
                        {
                            Id = 17L,
                            Name = "Applications and offers by school phase",
                            ReleaseId = new Guid("63227211-7cb3-408c-b5c2-40d3d7cb2717")
                        });
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Theme");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ee1855ca-d1e1-4f04-a795-cbd61d326a1f"),
                            Slug = "pupils-and-schools",
                            Title = "Pupils and schools"
                        },
                        new
                        {
                            Id = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"),
                            Slug = "children-and-early-years",
                            Title = "Children, early years and social care"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("67c249de-1cca-446e-8ccb-dcdac542f460"),
                            Slug = "pupil-absence",
                            ThemeId = new Guid("ee1855ca-d1e1-4f04-a795-cbd61d326a1f"),
                            Title = "Pupil absence"
                        },
                        new
                        {
                            Id = new Guid("77941b7d-bbd6-4069-9107-565af89e2dec"),
                            Slug = "exclusions",
                            ThemeId = new Guid("ee1855ca-d1e1-4f04-a795-cbd61d326a1f"),
                            Title = "Exclusions"
                        },
                        new
                        {
                            Id = new Guid("1a9636e4-29d5-4c90-8c07-f41db8dd019c"),
                            Slug = "school-applications",
                            ThemeId = new Guid("ee1855ca-d1e1-4f04-a795-cbd61d326a1f"),
                            Title = "School applications"
                        },
                        new
                        {
                            Id = new Guid("17b2e32c-ed2f-4896-852b-513cdf466769"),
                            Slug = "early-years-foundation-stage-profile",
                            ThemeId = new Guid("cc8e02fd-5599-41aa-940d-26bca68eab53"),
                            Title = "Early years foundation stage profile"
                        });
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.Filter", "Filter")
                        .WithMany("FilterGroups")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterItem", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.FilterGroup", "FilterGroup")
                        .WithMany("FilterItems")
                        .HasForeignKey("FilterGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Model.Indicator", b =>
                {
                    b.HasOne("GovUk.Education.ExploreEducationStatistics.Data.Model.IndicatorGroup", "IndicatorGroup")
                        .WithMany("Indicators")
                        .HasForeignKey("IndicatorGroupId")
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
