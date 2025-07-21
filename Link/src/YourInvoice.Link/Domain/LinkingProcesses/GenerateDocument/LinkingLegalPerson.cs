///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.GenerateDocument
{
    public class LinkingLegalPerson
    {
        public GetLegalGeneralInformationResponse GeneralInformation { get; set; }

        public GetLegalFinancialResponse FinancialInformation {  get; set; }

        public LegalRepresentativeTaxAuditor RepresentativeTaxAuditorInformation { get; set; }

        public GetSagrilaftResponse SagrilaftInformation { get; set; }

        public LegalCommercialAndBankReferenceResponse CommercialAndBankInformation {  get; set; }

        public List<GetLegalShareholderResponse> ShareholdersInformation { get; set; }

        public List<GetLegalBoardDirectorResponse> BoardDirectorInformation { get; set; }

        public GetLegalShareholderBoardDirectorResponse ShareholderBoardDirectorInformation {  get; set; }

        public LegalSignatureDeclaration SignatureInformation {  get; set; }
    }
}
