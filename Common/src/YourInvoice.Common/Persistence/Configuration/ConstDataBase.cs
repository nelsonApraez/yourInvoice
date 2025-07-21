///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Persistence.Configuration
{
    public static class ConstDataBase
    {
        public static string DateZonePacific => "(dateadd(minute,(-300)-datepart(tzoffset,sysdatetimeoffset()),getdate()))";
        public static string InvoiceStatus => "ESTADOFACTURA";
        public static string MoneyType => "TIPOMONEDA";
        public static string DocumentType => "TIPODOCUMENTO";
        public static string OfferState => "ESTADOOFERTA";
        public static string CodeInvoice => "CODIGOFACTURA";
        public static string ValidationDian => "VALIDATION_DIAN";
        public static string EventDian => "EVENTOS_DIAN";
        public static string ServiceBus => "SERVICEBUS";
        public static string FtpFyM => "FTP";
        public static string FtpFyMCodeFailedDIAN => "CODIGO_FALLO_ARCHIVO_FAL_DIAN";
        public static string Storage => "STORAGE";
        public static string Bank => "BANCOS";
        public static string IdType => "TIPO_IDENTIFICACION";
        public static string AccountType => "TIPO_CUENTA";
        public static string PersonType => "TIPO_PERSONA";
        public static string ZapSing => "ZAPSIGN";
        public static string Templates => "TEMPLATES";
        public static string DatayourInvoice => "DATAyourInvoice";
        public static string EmailyourInvoice => "EMAILyourInvoice";
        public static string InvoiceStatusDispersion => "INVOICE_STATUS_DISPERSION";
        public static string SettingAdmin => "SETTING_ADMIN";
        public static string FtpFactoring => "FTP_FACTORING";
        public static string UserRoleId => "ROLES_USUARIO";
        public static string UrlGeneral => "URL_GENERALES";
        public static string NotificationEvent => "NOTIFICATION_EVENT";
        public static string PreRegistrationStatus => "ESTADOS_PRE_REGISTROS";
        public static string Countries => "PAISES";
        public static string Terms => "TERMINOS";
        public static string ConctactBy => "CONTACTO";

        public static string GreatContributor => "GRAN_CONTRIBUYENTE";
        public static string IdselfRetaining => "ES_AUTORETENEDOR";
        public static string RequiredDeclareIncome => "OBLIGADO_DECLARAR_RENTA";
        public static string TaxLiability => "RESPONSABILIDAD_TRIBUTARIA";
        public static string TypesResponsibilities => "TIPOS_RESPONSABILIDADES";
        public static string Departments => "DEPARTAMENTOS";
        public static string StatusForm => "ESTADO_FORMULARIO";
        public static string QuestionExposure => "PREGUNTAS_EXPOSICION";
        public static string AnswerQuestionExposure => "RESPUESTAS_EXPOSICION";
        public static string City => "CIUDADES";
        public static string OperationType => "TIPO_OPERACION";
        public static string CompanyType => "TIPO_EMPRESA";
        public static string TypeOfCompany => "TIPO_SOCIEDAD";
        public static string EconomicActivity => "ACTIVIDAD_ECONOMICA";
        public static string QuestionLegalFinancial => "PREGUNTAS_LEGAL_FINANCIERA";
        public static string AnswerQuestionLegalFinancial => "RESPUESTAS_LEGAL_FINANCIERA";
        public static string QuestionLegalSagrilaft => "PREGUNTAS_LEGAL_SAGRILAFT";
        public static string AnswerQuestionLegalSagrilaft => "RESPUESTAS_LEGAL_SAGRILAFT";
        public static string Truora => "TRUORA";
        public static string ParagraphDeclarationLegalSignature => "PARRAFO_DECLARACION_FIRMA_JURIDICA";
        public static string ParagraphDeclarationSignature => "PARRAFO_DECLARACION_FIRMA";
        public static string LinkStatus => "ESTADO_VINCULACION";
    }
}