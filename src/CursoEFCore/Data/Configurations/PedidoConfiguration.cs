using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {

            builder
                .ToTable("Pedidos")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.IniciadoEm)
                .HasDefaultValueSql("GetDate()")    //Definindo um valor default para o campo na inclusao
                .ValueGeneratedOnAdd();             //de um novo registro

            builder
                .Property(p => p.Status)
                .HasConversion<string>();

            builder
                .Property(p => p.TipoFrete)
                .HasConversion<int>();

            builder
                .Property(p => p.Observacao)
                .HasColumnType("varchar(512)");


            //Configurando o relacionamento entre as tabelas. 
            //A classe PAI eh quem define a configuracao de relacionamento
            builder
                .HasMany(p => p.Items)
                    .WithOne(i => i.Pedido)
                .OnDelete(DeleteBehavior.Cascade);

        } //Configure
    } //class
} //namespace
