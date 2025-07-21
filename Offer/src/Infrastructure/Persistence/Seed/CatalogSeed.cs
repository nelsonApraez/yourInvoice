///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Persistence.Configuration;

namespace yourInvoice.Offer.Infrastructure.Persistence.Seed
{
    public class CatalogSeed : IEntityTypeConfiguration<CatalogInfo>
    {
        private readonly Guid createdBy = Guid.Parse("B3822949-5D72-4016-BE39-840F124D95AD");
        private readonly Guid modifiedBy = Guid.Parse("EC036BDD-D5DD-4433-B70C-4CE4D0E2809B");

        public void Configure(EntityTypeBuilder<CatalogInfo> builder)
        {
            builder.HasData(
                new CatalogInfo { Id = Guid.Parse("C9ABB9F7-77EE-46FD-8668-DE26D99FE00B"), Name = ConstDataBase.InvoiceStatus, Descripton = "Estados de la factura", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("00673510-F486-4EF9-993E-578FC17A9990"), Name = ConstDataBase.MoneyType, Descripton = "Tipo de moneda", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("8580D31C-81CB-4BE8-8B2A-282A788A838B"), Name = ConstDataBase.DocumentType, Descripton = "Tipo de documento", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("F9B0B38C-DCA9-4475-9F93-0D3EC85CDC61"), Name = ConstDataBase.OfferState, Descripton = "Estado de la oferta", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("AD495AC8-798E-45E8-803B-E3482A99E259"), Name = ConstDataBase.CodeInvoice, Descripton = "Códigos de factura", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("030B7939-39F6-4078-AFEB-0855D0BE32A2"), Name = ConstDataBase.ValidationDian, Descripton = "Validación DIAN", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("A4E3D554-433A-441F-8BAE-42CC091F6EBF"), Name = ConstDataBase.EventDian, Descripton = "Eventos DIAN 1 al 7", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("429BF7C3-AE04-4C86-B7EF-02BAC67BC32C"), Name = ConstDataBase.ServiceBus, Descripton = "Parametros service bus", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("F41C13FD-D805-4B1B-8BEF-1FB201877CE6"), Name = ConstDataBase.Storage, Descripton = "Parametros storage", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("D6CCAD20-5498-4458-87B8-3291E86F727B"), Name = ConstDataBase.FtpFyM, Descripton = "Información FTP", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("683E30AD-A2E3-41BE-9C68-3CFA12901394"), Name = ConstDataBase.FtpFyMCodeFailedDIAN, Descripton = "Descripción de los código de fallo archivo respuesta FTP", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("918D55A6-AA07-4583-AE66-6F4D1B988D0F"), Name = ConstDataBase.Bank, Descripton = "Codigos de bancos", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("7ADB5A6C-4947-4067-8F38-EBBA301B32B5"), Name = ConstDataBase.PersonType, Descripton = "Tipos de persona", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("DC213B34-5F90-4C38-9A4B-97DACF893298"), Name = ConstDataBase.IdType, Descripton = "Tipos de identificacion", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("16D2A851-2E09-4CE9-9980-C9229944177D"), Name = ConstDataBase.AccountType, Descripton = "Tipos de cuenta.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("C0AF0122-1E26-409B-A830-D2B1194A616F"), Name = ConstDataBase.ZapSing, Descripton = "Parametros para Zapsing", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("B4284C7B-3861-4854-9F4B-00802E2F407F"), Name = ConstDataBase.Templates, Descripton = "Plantillas de correos y documentos", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("94630965-BC29-442F-A869-D11716B6863E"), Name = ConstDataBase.DatayourInvoice, Descripton = "Informacion relacionada a su Factura para plantillas.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("9040C0B7-F0DA-4A7D-A86F-771F00F51A32"), Name = ConstDataBase.EmailyourInvoice, Descripton = "Informacion relacionada al email de notificaciones de su factura.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("0E63B33A-D742-431C-A843-0EAA619535F2"), Name = ConstDataBase.InvoiceStatusDispersion, Descripton = "Informacion estados de las facturas proceso factoring.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("972FB882-6543-4238-AF31-A954174E7524"), Name = ConstDataBase.SettingAdmin, Descripton = "Informacion envio correo.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("F43F92BE-C44B-43CA-BB52-253A50A3EADF"), Name = ConstDataBase.FtpFactoring, Descripton = "Información FTP de factoring.", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("18EEAC02-A739-4105-81AE-273CBC14D06D"), Name = ConstDataBase.UserRoleId, Descripton = "Información roles usuarios aplicación", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("815E2762-27CA-4ABD-8A2D-D40D9DF5CEB0"), Name = ConstDataBase.UrlGeneral, Descripton = "Información de las url de la plataforma", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("717D58B3-770B-4229-9614-8650D8DF4BD4"), Name = ConstDataBase.NotificationEvent, Descripton = "Notificacion de eventos", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("F7E22330-DCF5-48F3-8EC2-FEA4041627C1"), Name = ConstDataBase.PreRegistrationStatus, Descripton = "Estados pre-registros", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("1BF8532C-106E-4A35-82EB-046C0F46DF26"), Name = ConstDataBase.Countries, Descripton = "Paises", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("D31CD635-952E-40AF-8781-2F55F0B66F36"), Name = ConstDataBase.Terms, Descripton = "Terminos", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("EC5657BA-1F2A-480A-BE02-886E570614AD"), Name = ConstDataBase.ConctactBy, Descripton = "Contacto por", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("094EF5C5-954D-48E8-A42A-227799FD6ED8"), Name = ConstDataBase.GreatContributor, Descripton = "Indica si la Persona es Gran Contribuyente", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("C06D2634-BB18-43F9-A818-AF73824F9EF6"), Name = ConstDataBase.IdselfRetaining, Descripton = "Indica si la Persona es Autoretenedor", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("C4094ABF-3EF9-45DC-A7F7-5F5408D6FEE8"), Name = ConstDataBase.RequiredDeclareIncome, Descripton = "Indica si la Persona es Obligada a Declarar Renta", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("C0CD8374-1F3E-4B55-874B-0D3A0290185E"), Name = ConstDataBase.TaxLiability, Descripton = "Indica Responsabilidades u Obligaciones Tributarias", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("708DBD32-901D-4700-896F-3CE3BC20C288"), Name = ConstDataBase.TypesResponsibilities, Descripton = "Indica Tipos de Responsabilidades Tributarias", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"), Name = ConstDataBase.Departments, Descripton = "Lista de Departamentos de Colombia", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("43665671-4239-4D0E-A9AA-67A7712A987E"), Name = ConstDataBase.StatusForm, Descripton = "Estado de los formularios (SinIniciar=Rojo; EnProceso=Naranja; Completado=Verde)", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("F7F7E12F-B1D4-443F-946B-91C3D78170E9"), Name = ConstDataBase.QuestionExposure, Descripton = "Preguntas Dinámicas para el Formulario de Exposición", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("62E13AF1-6C5F-402D-9DE1-8FFA24E74D0C"), Name = ConstDataBase.AnswerQuestionExposure, Descripton = "Preguntas Dinámicas para el Formulario de Exposición", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"), Name = ConstDataBase.City, Descripton = "Lista de Ciudades de Colombia", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("2F15EFF1-336F-40BE-8CEE-FDEBB2096E26"), Name = ConstDataBase.OperationType, Descripton = "Tipo de operacion", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("7AD13CA7-BAC8-4C74-85FD-9D6761EC9D3A"), Name = ConstDataBase.CompanyType, Descripton = "Tipos de empresas", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("2510072B-4C9D-4D07-B871-BA31AF9AA626"), Name = ConstDataBase.TypeOfCompany, Descripton = "Tipos de sociedad", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("DA560951-0F8B-4FCF-B90E-5929481C95E0"), Name = ConstDataBase.EconomicActivity, Descripton = "Actividad ecónomica", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("EEBE3B24-D459-4F30-ADDC-E23352EBA05D"), Name = ConstDataBase.QuestionLegalFinancial, Descripton = "Preguntas Dinámicas para el Formulario de legal financiera", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("20F1AC44-81C8-44F1-B4D3-76C91400FD4A"), Name = ConstDataBase.AnswerQuestionLegalFinancial, Descripton = "Preguntas Dinámicas para el Formulario de legal financiera", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("33C41AC9-19FE-4103-B72B-0DE6A09E881D"), Name = ConstDataBase.QuestionLegalSagrilaft, Descripton = "Preguntas Dinámicas para el Formulario de legal SAGRILAFT", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("38A471EE-2658-4C6A-BE02-CB60707231B4"), Name = ConstDataBase.AnswerQuestionLegalSagrilaft, Descripton = "Preguntas Dinámicas para el Formulario de legal SAGRILAFT", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("A2B1F37E-0460-4F73-8610-DBB720204D0C"), Name = ConstDataBase.Truora, Descripton = "Parametros para Truora", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("576A1DE7-4507-497D-96DC-BB1A3C9A8D54"), Name = ConstDataBase.ParagraphDeclarationSignature, Descripton = "Parrafos declaración de firmas persona natural", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("7DB41C95-ED8C-4C2D-8CA3-A9081983222F"), Name = ConstDataBase.ParagraphDeclarationLegalSignature, Descripton = "Parrafos declaración de firmas persona jurídica", CreatedBy = createdBy, ModifiedBy = modifiedBy },
                new CatalogInfo { Id = Guid.Parse("C77EF3F9-90EF-4549-87F0-C5A4582F4810"), Name = ConstDataBase.LinkStatus, Descripton = "Estados generales del proceso de vinculación", CreatedBy = createdBy, ModifiedBy = modifiedBy }
                );
        }
    }
}