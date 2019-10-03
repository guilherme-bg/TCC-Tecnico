﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCC.Models;

namespace TCC.Migrations
{
    [DbContext(typeof(TCCContext))]
    [Migration("20190930160601_criandobd")]
    partial class criandobd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099");

            modelBuilder.Entity("TCC.Models.Cidade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Cidade");
                });

            modelBuilder.Entity("TCC.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CidadeId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Moradia")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Protecao")
                        .IsRequired();

                    b.Property<int>("QtAnimais");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(223);

                    b.Property<string>("Telefone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CidadeId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("TCC.Models.Usuario", b =>
                {
                    b.HasOne("TCC.Models.Cidade", "Cidade")
                        .WithMany("Usuarios")
                        .HasForeignKey("CidadeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
