﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicFy.Data;

#nullable disable

namespace MusicFy.Migrations
{
    [DbContext(typeof(MusicFyDbContext))]
    [Migration("20231123173415_UpdateSongsList")]
    partial class UpdateSongsList
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MF.Data.Song.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("isPublic")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("MF.Data.Song.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("FName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Followers")
                        .HasColumnType("int");

                    b.Property<string>("LName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("MonthlyListeners")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("MF.Data.Song.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("MF.Data.Song.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AlbumId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("AuthorId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Listeners")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("PlaylistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("MF.Data.Song.Album", b =>
                {
                    b.HasOne("MF.Data.Song.Author", "Author")
                        .WithMany("Albums")
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MF.Data.Song.Playlist", b =>
                {
                    b.HasOne("MF.Data.Song.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MF.Data.Song.Song", b =>
                {
                    b.HasOne("MF.Data.Song.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MF.Data.Song.Author", "Author")
                        .WithMany("Songs")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MF.Data.Song.Playlist", null)
                        .WithMany("Songs")
                        .HasForeignKey("PlaylistId");

                    b.Navigation("Album");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MF.Data.Song.Album", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MF.Data.Song.Author", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MF.Data.Song.Playlist", b =>
                {
                    b.Navigation("Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
