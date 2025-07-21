///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.DIAN.Function.Constant
{
    public static class EventNotificationType
    {
        public static string MessageSuccess => "exitoso";
        public static string MessageFailed => "fallido";
        public static string SearchFileFtpTitle => "Busqueda archivo DIAN FTP";
        public static string ProccesFileFtpTitle => "Procesar archivo DIAN FTP";
        public static string CreatedFileFtpTitle => "Archivo DIAN enviado FTP";
        public static string CreatedFileTitle => "Archivo DIAN Creado";
        public static Guid CreatedFileId => Guid.Parse("E885D4D3-D858-4899-9A0C-227655BAB9C1");
        public static string CreatedFileBlobStorageTitle => "Archivo DIAN Creado Blob Storage";
        public static Guid CreatedFileBlobStorageId => Guid.Parse("D4EC997C-DF61-48BE-9C80-DF7F1F6ACC98");
    }

    public static class StateInvoice
    {
        public static Guid Approved => Guid.Parse("7FDDF6C1-F14D-4254-81DE-BA0759360C82");
        public static Guid Rejection => Guid.Parse("8BC21F26-518D-4B71-BC48-D899E5886682");
        public static Guid InProgress => Guid.Parse("27614A95-911A-4204-B49F-E6D5B9D530B4");
    }
}