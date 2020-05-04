﻿// <auto-generated />
using System;
using CrudUserProfileFuncionality.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrudUserProfileFuncionality.Migrations
{
    [DbContext(typeof(DataAppContext))]
    [Migration("20200503200620_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("TechnicalProofCrudCore.Models.Funcionalidade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Funcionalidades");
                });

            modelBuilder.Entity("TechnicalProofCrudCore.Models.Perfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Perfis");
                });

            modelBuilder.Entity("TechnicalProofCrudCore.Models.PerfilFuncionalidade", b =>
                {
                    b.Property<int>("PerfilId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FuncionalidadeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PerfilId", "FuncionalidadeId");

                    b.HasIndex("FuncionalidadeId");

                    b.ToTable("PerfilFuncionalidade");
                });

            modelBuilder.Entity("TechnicalProofCrudCore.Models.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PerfilId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PerfilId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("TechnicalProofCrudCore.Models.PerfilFuncionalidade", b =>
                {
                    b.HasOne("TechnicalProofCrudCore.Models.Funcionalidade", "Funcionalidade")
                        .WithMany("PerfilFuncionalidade")
                        .HasForeignKey("FuncionalidadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnicalProofCrudCore.Models.Perfil", "Perfil")
                        .WithMany("PerfilFuncionalidade")
                        .HasForeignKey("PerfilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TechnicalProofCrudCore.Models.Usuario", b =>
                {
                    b.HasOne("TechnicalProofCrudCore.Models.Perfil", "Perfil")
                        .WithMany()
                        .HasForeignKey("PerfilId");
                });
#pragma warning restore 612, 618
        }
    }
}
