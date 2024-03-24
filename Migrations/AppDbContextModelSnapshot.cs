﻿// <auto-generated />
using System;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIBanco.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("APIBanco.Domain.Models.Adress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("ZipCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("Adresses");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.CardTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreditCardId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreditCardId");

                    b.ToTable("CardTransactions");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BornDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("Cpf");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("CardNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("HolderName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalLimit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UsedLimit")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Insurance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Coverage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("InsuranceType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PolicyNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Premium")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Insurances");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ContractDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LoanAmount")
                        .HasColumnType("TEXT");

                    b.Property<int>("LoanTermMonths")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Transactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BankAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Adress", b =>
                {
                    b.HasOne("APIBanco.Domain.Models.Client", "Client")
                        .WithOne("Adress")
                        .HasForeignKey("APIBanco.Domain.Models.Adress", "ClientId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.BankAccount", b =>
                {
                    b.HasOne("APIBanco.Domain.Models.Client", "Client")
                        .WithOne("BankAccount")
                        .HasForeignKey("APIBanco.Domain.Models.BankAccount", "ClientId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.CardTransaction", b =>
                {
                    b.HasOne("APIBanco.Domain.Models.CreditCard", "CreditCard")
                        .WithMany("CardTransactions")
                        .HasForeignKey("CreditCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreditCard");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Transactions", b =>
                {
                    b.HasOne("APIBanco.Domain.Models.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountId");

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.BankAccount", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("APIBanco.Domain.Models.Client", b =>
                {
                    b.Navigation("Adress")
                        .IsRequired();

                    b.Navigation("BankAccount")
                        .IsRequired();
                });

            modelBuilder.Entity("APIBanco.Domain.Models.CreditCard", b =>
                {
                    b.Navigation("CardTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
