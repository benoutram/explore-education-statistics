﻿// <auto-generated />
using System;
using GovUk.Education.ExploreEducationStatistics.Data.Api.Data;
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
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Api.Models.CharacteristicDataLa", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attributes");

                    b.Property<string>("Characteristic");

                    b.Property<string>("CharacteristicName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("JSON_VALUE(Characteristic, '$.characteristic_1')");

                    b.Property<string>("Country");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<string>("LocalAuthority");

                    b.Property<Guid>("PublicationId");

                    b.Property<string>("Region");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("ReleaseId");

                    b.Property<string>("SchoolType")
                        .IsRequired();

                    b.Property<string>("Term");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("CharacteristicName");

                    b.ToTable("CharacteristicDataLa");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Api.Models.CharacteristicDataNational", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attributes");

                    b.Property<string>("Characteristic");

                    b.Property<string>("CharacteristicName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("JSON_VALUE(Characteristic, '$.characteristic_1')");

                    b.Property<string>("Country");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<Guid>("PublicationId");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("ReleaseId");

                    b.Property<string>("SchoolType")
                        .IsRequired();

                    b.Property<string>("Term");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("CharacteristicName");

                    b.ToTable("CharacteristicDataNational");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Api.Models.GeographicData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attributes");

                    b.Property<string>("Country");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<string>("LocalAuthority");

                    b.Property<Guid>("PublicationId");

                    b.Property<string>("Region");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("ReleaseId");

                    b.Property<string>("School");

                    b.Property<string>("SchoolType")
                        .IsRequired();

                    b.Property<string>("Term");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("GeographicData");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Api.Models.Meta.AttributeMeta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Group");

                    b.Property<bool>("KeyIndicator");

                    b.Property<string>("Label");

                    b.Property<string>("Name");

                    b.Property<Guid>("PublicationId");

                    b.Property<int>("Unit");

                    b.HasKey("Id");

                    b.ToTable("AttributeMeta");
                });

            modelBuilder.Entity("GovUk.Education.ExploreEducationStatistics.Data.Api.Models.Meta.CharacteristicMeta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Group");

                    b.Property<string>("Label");

                    b.Property<string>("Name");

                    b.Property<Guid>("PublicationId");

                    b.HasKey("Id");

                    b.ToTable("CharacteristicMeta");
                });
#pragma warning restore 612, 618
        }
    }
}
