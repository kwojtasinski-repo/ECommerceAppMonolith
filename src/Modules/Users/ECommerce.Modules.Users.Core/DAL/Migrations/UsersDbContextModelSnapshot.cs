﻿// <auto-generated />
using System;
using ECommerce.Modules.Users.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ECommerce.Modules.Users.Core.DAL.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    partial class UsersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("users")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ECommerce.Modules.Users.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Claims")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", "users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e70b6db8-f77a-4ce7-833f-977617cf1873"),
                            Claims = "{\"permissions\":[\"users\",\"items\",\"item-sale\",\"currencies\"]}",
                            CreatedAt = new DateTime(2022, 8, 13, 16, 59, 53, 177, DateTimeKind.Local).AddTicks(589),
                            Email = "admin@admin.com",
                            IsActive = true,
                            Password = "AQAAAAEAACcQAAAAEP/+MBJ+0Y0ditII5cclQrsBB8G7mJyZ+y3zBn0yfFoHiSF/RiZCWSdemZ+eQ70Vag==",
                            Role = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
