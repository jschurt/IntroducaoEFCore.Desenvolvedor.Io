using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder
                .ToTable("Produtos")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.CodigoBarras)
                .HasColumnType("varchar(14)")
                .IsRequired();

            builder
                .Property(p => p.Descricao)
                .HasColumnType("varchar(60)");

            builder
                .Property(p => p.Valor)
                .IsRequired();

            builder
                .Property(p => p.TipoProduto)
                .HasConversion<string>()            //Importante. Com esta configuracao, sera gravada na base a "string" do valor enum
                .IsRequired();

        } //Configure
    } //class
} //namespace
