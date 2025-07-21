///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.EmailToRequestDocument
{
    public class EmailToRequestDocumentCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }        
        public string Email { get; set; }
    }
}
