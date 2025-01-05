﻿// <auto-generated />
using System;
using IgnisWorkerService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IgnisWorkerService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241229021914_tag_logdate")]
    partial class tag_logdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IgnisWorkerService.Data.DbModel.AppLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Exception")
                        .HasColumnType("text")
                        .HasColumnName("exception");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("level");

                    b.Property<string>("Log_Event")
                        .HasColumnType("text")
                        .HasColumnName("log_event");

                    b.Property<string>("Message")
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<string>("Message_Template")
                        .HasColumnType("text")
                        .HasColumnName("message_template");

                    b.Property<string>("Properties")
                        .HasColumnType("text")
                        .HasColumnName("properties");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("pk_applogs");

                    b.ToTable("applogs");
                });

            modelBuilder.Entity("IgnisWorkerService.Data.DbModel.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("InDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("indate");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("logdate");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal?>("TagValue")
                        .HasColumnType("numeric")
                        .HasColumnName("tagvalue");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("Unit")
                        .HasColumnType("text")
                        .HasColumnName("unit");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("IgnisWorkerService.Data.DbModel.TagDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<string>("DataTag")
                        .HasColumnType("text")
                        .HasColumnName("datatag");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("InDate")
                        .HasColumnType("text")
                        .HasColumnName("indate");

                    b.Property<string>("SystemTag")
                        .HasColumnType("text")
                        .HasColumnName("systemtag");

                    b.Property<string>("Unit")
                        .HasColumnType("text")
                        .HasColumnName("unit");

                    b.Property<string>("UnitTag")
                        .HasColumnType("text")
                        .HasColumnName("unittag");

                    b.Property<string>("UnitType")
                        .HasColumnType("text")
                        .HasColumnName("unittype");

                    b.HasKey("Id")
                        .HasName("pk_tagsdefinitions");

                    b.ToTable("tagsdefinitions");
                });

            modelBuilder.Entity("IgnisWorkerService.Data.DbModel.TagValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Quality")
                        .HasColumnType("text")
                        .HasColumnName("quality");

                    b.Property<int>("TagId")
                        .HasColumnType("integer")
                        .HasColumnName("tagid");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.Property<decimal?>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_tagvalues");

                    b.ToTable("tagvalues");
                });
#pragma warning restore 612, 618
        }
    }
}
