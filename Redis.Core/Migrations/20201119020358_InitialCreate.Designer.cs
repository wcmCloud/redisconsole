﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Redis.Core;

namespace Redis.Core.Migrations
{
    [DbContext(typeof(SQLiteDBContext))]
    [Migration("20201119020358_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Redis.Core.RedisClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Auth")
                        .HasColumnType("TEXT");

                    b.Property<int>("ConnectionTimeout")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DatabaseScanLimit")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExecuteTimeout")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Host")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IgnoreSSLErrors")
                        .HasColumnType("INTEGER");

                    b.Property<string>("KeysPattern")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LuaKeysLoading")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamespaceSeparator")
                        .HasColumnType("TEXT");

                    b.Property<bool>("OverrideClusterHost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SshHost")
                        .HasColumnType("TEXT");

                    b.Property<string>("SshPassword")
                        .HasColumnType("TEXT");

                    b.Property<int>("SshPort")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SshPrivateKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("SshUser")
                        .HasColumnType("TEXT");

                    b.Property<string>("SslCaCertPath")
                        .HasColumnType("TEXT");

                    b.Property<bool>("SslEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SslLocalCertPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("SslPrivateKeyPath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Instances");
                });
#pragma warning restore 612, 618
        }
    }
}