using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder
                .ToTable("Clientes")
                .HasKey(c => c.Id);
                
            //Indico quais indices eu quero criar (alem de PK e FK)
            builder
                .HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");

            builder.Property(c => c.Nome)
                    .HasColumnType("varchar(80)")
                    .IsRequired();

            builder.Property(c => c.Telefone)
                    .HasColumnType("char(11)") //Char promove maior desempenho
                    .IsRequired();

            builder.Property(c => c.CEP)
                    .HasColumnType("char(8)")
                    .IsRequired();

            builder.Property(c => c.Estado)
                    .HasColumnType("char(2)")
                    .IsRequired();

            builder.Property(c => c.Estado)
                    .HasMaxLength(60)
                    .IsRequired();

        } //Configure
    } //class
} //namespace
