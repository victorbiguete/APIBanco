﻿using APIBanco.Model;
using Microsoft.EntityFrameworkCore;

namespace APIBanco.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    
        public DbSet<Cliente> Clientes { get; set;}
        public DbSet<ContaPoupanca> ContaPoupanca { get; set;}
        public DbSet<ContaEspecial> ContaEspecial { get; set;}
        public DbSet<ContaCorrente> ContaCorrente { get; set;}
    }
}
