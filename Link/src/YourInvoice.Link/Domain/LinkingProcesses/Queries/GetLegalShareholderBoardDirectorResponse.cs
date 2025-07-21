///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetLegalShareholderBoardDirectorResponse
    {
        public Guid? Id { get; set; }

        public Guid? Id_LegalGeneralInformation { get; set; }

        public bool? IsSoleProprietorship { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

        public bool? Status { get; set; }
    }
}