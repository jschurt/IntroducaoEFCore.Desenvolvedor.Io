using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {

        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()   //Desta forma, consigo ver os parametros no log
                .UseSqlServer("Data source=jschurt;Initial Catalog=CursoEFCore; Integrated Security=true",
                p => p.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null)  //Criando resiliencia de conexao. Caso haja erro de conexao, havera 2 tentativas de conexao com um intervalo de 5 segunds.
                );
        } //OnConfiguring

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);

        } //OnModelCreating

        /// <summary>
        /// Metodo que ira mapear no banco todas as propriedades que nao tiverem sido configuradas via FluentAPI.
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {

            foreach (var entidade in modelBuilder.Model.GetEntityTypes())
            {
                
                //Pegando todas as propriedades que sao do tipo string
                var properties = entidade.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    {
                        property.SetColumnType("varchar(50)");
                    }
                }
            
            }

        } //MapearPropriedadesEsquecidas

    } //class
} //namespace
