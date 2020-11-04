using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {

            builder
                .ToTable("PedidoItens")
                .HasKey(pi => pi.Id);

            builder
                .Property(pi => pi.Quantidade)
                .HasDefaultValue(1)                 //Definindo um valor default para o campo na inclusao
                .IsRequired();

            builder
                .Property(pi => pi.Valor)
                .IsRequired();

            builder
                .Property(p => p.Desconto)
                .IsRequired();

        } //Configure
    } //class
} //namespace
