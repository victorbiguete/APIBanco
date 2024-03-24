using APIBanco.Domain.Models.DbContext;
using Microsoft.EntityFrameworkCore;

namespace APIBanco.Domain.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transactions> Transactions { get; set; }
    public DbSet<Investment> Investments { get; set; }

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

        builder.Entity<Investment>().HasKey(x => x.Id);
        builder.Entity<Investment>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Investment>().Property(x => x.Name).IsRequired();
        builder.Entity<Investment>().Property(x => x.MaintenanceFee).IsRequired();
        builder.Entity<Investment>().Property(x => x.MinContribution).IsRequired();
        builder.Entity<Investment>().Property(x => x.MinRedemptionTerm).IsRequired();
        builder.Entity<Investment>().Property(x => x.MaxRedemptionTerm).IsRequired();
        builder.Entity<Investment>().Property(x => x.MinRedemptionValue).IsRequired();
        builder.Entity<Investment>().Property(x => x.RateYield).IsRequired();
        builder.Entity<Investment>().Property(x => x.Status).IsRequired();
    }
}
