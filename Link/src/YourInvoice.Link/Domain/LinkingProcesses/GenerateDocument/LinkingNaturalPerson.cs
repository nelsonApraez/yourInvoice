///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.GenerateDocument
{
    public class LinkingNaturalPerson
    {
        public GeneralInformationResponse GeneralInformation { get; set; }

        public GetBankResponse BankInformation {  get; set; }

        public GetFinancialResponse FinancialInformation { get; set; }

        public GetExposureResponse ExposureInformation { get; set; }

        public GetWorkingResponse WorkingInformation {  get; set; }

        public GetReferenceResponse ReferenceInformation { get; set; }

        public GetSignatureDeclarationResponse SignatureDeclarationInformation { get; set; }
    }
}
