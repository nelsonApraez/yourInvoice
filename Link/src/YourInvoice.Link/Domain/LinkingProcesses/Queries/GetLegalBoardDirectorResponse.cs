///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetLegalBoardDirectorResponse
    {
        public Guid? Id { get; set; }

        public Guid? Id_LegalGeneralInformation { get; set; }

        public string? FullNameCompanyName { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string? DocumentTypeName { get; set; }

        public string? DocumentNumber { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

        public bool? Status { get; set; }
    }
}

