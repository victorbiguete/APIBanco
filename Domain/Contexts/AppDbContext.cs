using APIBanco.Domain.Models;
using APIBanco.Domain.Models.DbContext;
using Microsoft.EntityFrameworkCore;

namespace APIBanco.Domain.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transactions> Transactions { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Insurance> Insurances { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<CardTransaction> CardTransactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Client>().HasKey(x => x.Id);
        builder.Entity<Client>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Client>().Property(x => x.Name).IsRequired();
        builder.Entity<Client>().Property(x => x.Email).IsRequired();
        builder.Entity<Client>().Property(x => x.Password).IsRequired();
        builder.Entity<Client>().Property(x => x.Cpf).IsRequired();
        builder.Entity<Client>().Property(x => x.Cpf).HasMaxLength(11);
        builder.Entity<Client>().HasAlternateKey(x => x.Cpf);
        builder.Entity<Client>().HasOne(x => x.Adress).WithOne(x => x.Client).HasForeignKey<Adress>(x => x.ClientId);
        builder.Entity<Client>().HasOne(x => x.BankAccount).WithOne(x => x.Client).HasForeignKey<BankAccount>(x => x.ClientId);


        builder.Entity<Adress>().HasKey(x => x.Id);
        builder.Entity<Adress>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Adress>().Property(x => x.Cpf).IsRequired();

        builder.Entity<BankAccount>().HasKey(x => x.Id);
        builder.Entity<BankAccount>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<BankAccount>().Property(x => x.Cpf).IsRequired();
        builder.Entity<BankAccount>().Property(x => x.Balance).IsRequired();
        builder.Entity<BankAccount>().Property(x => x.Status).IsRequired();
        builder.Entity<BankAccount>().HasMany(x => x.Transactions).WithOne(x => x.BankAccount).HasForeignKey(x => x.BankAccountId);

        builder.Entity<Transactions>().HasKey(x => x.Id);
        builder.Entity<Transactions>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Transactions>().Property(x => x.Value).IsRequired();
        builder.Entity<Transactions>().Property(propertyExpression: x => x.Cpf).IsRequired();
        builder.Entity<Transactions>().Property(propertyExpression: x => x.Type).IsRequired();

        builder.Entity<Loan>().HasKey(x => x.Id);
        builder.Entity<Loan>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Loan>().Property(x => x.LoanAmount).IsRequired();
        builder.Entity<Loan>().Property(x => x.InterestRate).IsRequired();
        builder.Entity<Loan>().Property(x => x.LoanTermMonths).IsRequired();
        builder.Entity<Loan>().Property(x => x.ContractDate).IsRequired();

        builder.Entity<Insurance>().HasKey(x => x.Id);
        builder.Entity<Insurance>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Insurance>().Property(x => x.InsuranceType).IsRequired();
        builder.Entity<Insurance>().Property(x => x.Coverage).IsRequired();
        builder.Entity<Insurance>().Property(x => x.Company).IsRequired();
        builder.Entity<Insurance>().Property(x => x.PolicyNumber).IsRequired();
        builder.Entity<Insurance>().Property(x => x.StartDate).IsRequired();
        builder.Entity<Insurance>().Property(x => x.EndDate).IsRequired();
        builder.Entity<Insurance>().Property(x => x.Premium).IsRequired();

        builder.Entity<CreditCard>().HasKey(x => x.Id);
        builder.Entity<CreditCard>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<CreditCard>().Property(x => x.CardNumber).IsRequired();
        builder.Entity<CreditCard>().Property(x => x.HolderName).IsRequired();
        builder.Entity<CreditCard>().Property(x => x.ExpiryDate).IsRequired();
        builder.Entity<CreditCard>().Property(x => x.CVV).IsRequired();
        builder.Entity<CreditCard>().Property(x => x.TotalLimit).IsRequired();
        builder.Entity<CreditCard>().Property(x => x.UsedLimit).IsRequired();
        builder.Entity<CreditCard>().HasMany(x => x.CardTransactions).WithOne(x => x.CreditCard).HasForeignKey(x => x.CreditCardId);
    }
}
