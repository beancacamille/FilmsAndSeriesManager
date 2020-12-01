﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class FilmsAndSeriesManagerContext : DbContext
    {
        public FilmsAndSeriesManagerContext()
        {
        }

        public FilmsAndSeriesManagerContext(DbContextOptions<FilmsAndSeriesManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<ShowGenre> ShowGenres { get; set; }
        public virtual DbSet<ShowStatus> ShowStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FilmsAndSeriesManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Episode).HasDefaultValueSql("((0))");

                entity.Property(e => e.Season).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.HasOne(d => d.Show)
                    .WithMany()
                    .HasForeignKey(d => d.ShowId)
                    .HasConstraintName("FK__Series__ShowID__33D4B598");
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Score).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Shows__Status__37A5467C");
            });

            modelBuilder.Entity<ShowGenre>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ShowGenre");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.HasOne(d => d.Genre)
                    .WithMany()
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowGenre__Genre__300424B4");

                entity.HasOne(d => d.Show)
                    .WithMany()
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowGenre__ShowI__34C8D9D1");
            });

            modelBuilder.Entity<ShowStatus>(entity =>
            {
                entity.ToTable("ShowStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}