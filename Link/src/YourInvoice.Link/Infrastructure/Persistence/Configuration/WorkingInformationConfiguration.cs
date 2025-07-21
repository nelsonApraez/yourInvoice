///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class WorkingInformationConfiguration : IEntityTypeConfiguration<WorkingInformation>
    {
        public void Configure(EntityTypeBuilder<WorkingInformation> builder)
        {
            builder.ToTable("WorkingInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Address).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.BusinessName).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.PhoneNumber).HasColumnType("numeric(15, 0)");
            builder.Property(e => e.Position).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Profession).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.WhatTypeProductServiceSell).HasMaxLength(250).IsUnicode(false);
        }
    }
}