﻿// <auto-generated />
using System;
using DarkLibCW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DarkLibCW.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20230516114743_AddCreateWithList")]
    partial class AddCreateWithList
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthorCatalogCard", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CatalogCardsId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "CatalogCardsId");

                    b.HasIndex("CatalogCardsId");

                    b.ToTable("AuthorCatalogCard");
                });

            modelBuilder.Entity("DarkLibCW.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DarkLibCW.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CatalogCardId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CatalogCardId");

                    b.HasIndex("StatusId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DarkLibCW.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("DarkLibCW.Models.CatalogCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EditionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EditionId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Volume")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EditionId");

                    b.ToTable("CatalogCards");
                });

            modelBuilder.Entity("DarkLibCW.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DarkLibCW.Models.Edition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Editions");
                });

            modelBuilder.Entity("DarkLibCW.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LibrarianId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("LibrarianId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("DarkLibCW.Models.Librarian", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Librarians");
                });

            modelBuilder.Entity("DarkLibCW.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("DarkLibCW.Models.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("AuthorCatalogCard", b =>
                {
                    b.HasOne("DarkLibCW.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.CatalogCard", null)
                        .WithMany()
                        .HasForeignKey("CatalogCardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DarkLibCW.Models.Book", b =>
                {
                    b.HasOne("DarkLibCW.Models.CatalogCard", "CatalogCard")
                        .WithMany()
                        .HasForeignKey("CatalogCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("CatalogCard");
                });

            modelBuilder.Entity("DarkLibCW.Models.Booking", b =>
                {
                    b.HasOne("DarkLibCW.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.Subscriber", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("DarkLibCW.Models.CatalogCard", b =>
                {
                    b.HasOne("DarkLibCW.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Edition");
                });

            modelBuilder.Entity("DarkLibCW.Models.Issue", b =>
                {
                    b.HasOne("DarkLibCW.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.Librarian", "Librarian")
                        .WithMany()
                        .HasForeignKey("LibrarianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarkLibCW.Models.Subscriber", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Librarian");

                    b.Navigation("Subscriber");
                });
#pragma warning restore 612, 618
        }
    }
}
