///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Reflection;

namespace yourInvoice.Common.Constant
{
    public class ConstantCode_MimeType
    {
        public static readonly string ZIP = "application/x-zip-compressed";
    }

    public static class ConstantCode_MoneyType
    {
        public static readonly string USD = "USD";
        public static readonly string COP = "COP";

        public static bool Validate(string moneyType)
        {
            bool result = false;

            if (moneyType == USD || moneyType == COP)
                result = true;

            return result;
        }
    }

    public class ConstantCode_FileExtension
    {
        public static readonly string PDF = ".pdf";
        public static readonly string XML = ".xml";
        public static readonly string XSLX = ".xslx";
    }

    public class ConstantCode_PayType
    {
        public static readonly string Credit = "2";
        public static readonly string Debit = "1";
    }

    public static class AssemblyCommon
    {
        public static Assembly Assembly = Assembly.Load(new AssemblyName("yourInvoice.Common"));
    }

    public static class FtpConection
    {
        public static readonly string FtpFactoring = "FTP_FACTORING";
        public static readonly string FtpFyM = "FTP";
        public static readonly string FtpPathFile = "FTP-RUTA-ARCHIVO";
        public static readonly string FtpPort = "FTP-PUERTO";
        public static readonly string FtpHost = "FTP-HOST";
        public static readonly string FtpUser = "FTP-USUARIO";
        public static readonly string FtpPasProjrd = "FTP-PASProjRD";
        public static readonly string FtpPathFolderIN = "FTP-RUTA-ARCHIVO-ENTRADA";
        public static readonly string FtpPathFolderOUT = "FTP-RUTA-ARCHIVO-RESPUESTA";
    }

    public static class Excel
    {
        public static readonly string TypeFile = "application/vnp.openxmlformats-officedocument.spreadsheetml.sheet";
    }

    public static class ZapSignCatalog
    {
        public static readonly string CatalogName = "ZAPSIGN";
        public static readonly string ZapSignToken = "ZapSignToken";
        public static readonly string ZapSignUrl = "ZapSignUrl";
        public static readonly string ZapSignCreateUri = "ZapSignCreateUri";
        public static readonly string ZapSignAttachmentUri = "ZapSignAttachmentUri";
        public static readonly string ZapSignDetailDocUri = "ZapSignDetailDocUri";
    }

    public static class TruoraCatalog
    {
        public static readonly string CatalogName = "TRUORA";
        public static readonly string TruoraApiKey = "TruoraApiKey";
        public static readonly string TruoraUrlIdentity = "TruoraUrlIdentity";
        public static readonly string TruoraUrlAccount = "TruoraUrlAccount";
        public static readonly string TruoraFlowId = "TruoraFlowId";
        public static readonly string TruoraApiKeysUri = "TruoraApiKeysUri";
        public static readonly string TruoraProcessesUri = "TruoraProcessesUri";
        public static readonly string TruoraRedirectUrl = "TruoraRedirectUrl";
    }

    public static class EmailCatalog
    {
        public static readonly string CatalogName = "EMAILyourInvoice";
        public static readonly string EmailServer = "EMAIL Server";
        public static readonly string EmailPort = "EMAIL Port";
        public static readonly string EmailUser = "EMAIL User";
        public static readonly string EmailPwd = "EMAIL PasProjrd";
        public static readonly string EmailFrom = "EMAIL From";
        public static readonly string EmailSender = "EMAIL Sender";
    }

    public static class ConstantCode_Rellocation
    {
        public static char I = 'I';
        public static char A = 'A';
        public static char M = 'M';
        public static char R = 'R';
    }

    public class ConstantCode_Claims
    {
        public static readonly string OID = "oid";
        public static readonly string Name = "name";
        public static readonly string Sub = "sub";
    }

    public class ConstantCode_AppSection
    {
        public static readonly string AzureAdB2C = "AzureAdB2C";
    }

    public class ConstantCode_InvoicePercentage
    {
        public static readonly decimal InvoiceAllowedPercentage = new(19.5);
    }

    public static class ExposureCatalog
    {
        public static readonly string CatalogName = "PREGUNTAS_EXPOSICION";
    }
    public static class SAGRILAFTCatalog
    {
        public static readonly string CatalogName = "PREGUNTAS_LEGAL_SAGRILAFT";
    }

    public class ConstantCode_RoleUser
    {
        public static readonly string Administrator = "Administrador";
    }
}