///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Mvc;
using yourInvoice.Common.Controller;
using yourInvoice.Link.Application.LinkingProcess.CreateBank;
using yourInvoice.Link.Application.LinkingProcess.CreateExposure;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.CreateFinancial;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalBoardDirector;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalCommercialAndBankReference;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalFinancial;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalGeneralInformation;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholder;
using yourInvoice.Link.Application.LinkingProcess.CreatePersonalReference;
using yourInvoice.Link.Application.LinkingProcess.CreateSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.CreateWorking;
using yourInvoice.Link.Application.LinkingProcess.DeleteLegalBoardDirector;
using yourInvoice.Link.Application.LinkingProcess.DeleteLegalShareholder;
using yourInvoice.Link.Application.LinkingProcess.GetBank;
using yourInvoice.Link.Application.LinkingProcess.GetExposure;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.GetExposureQuestionResponse;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFTQuestionResponse;
using yourInvoice.Link.Application.LinkingProcess.GetFinancial;
using yourInvoice.Link.Application.LinkingProcess.GetGeneralInformation;
using yourInvoice.Link.Application.LinkingProcess.GetLegalBoardDirectorById;
using yourInvoice.Link.Application.LinkingProcess.GetLegalCommercialAndBankReference;
using yourInvoice.Link.Application.LinkingProcess.GetLegalFinancial;
using yourInvoice.Link.Application.LinkingProcess.GetLegalGeneralInformation;
using yourInvoice.Link.Application.LinkingProcess.GetLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderById;
using yourInvoice.Link.Application.LinkingProcess.GetPersonalReference;
using yourInvoice.Link.Application.LinkingProcess.GetSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.GetStatusForm;
using yourInvoice.Link.Application.LinkingProcess.GetStatusFormLegal;
using yourInvoice.Link.Application.LinkingProcess.GetWorking;
using yourInvoice.Link.Application.LinkingProcess.ListEconomicActivity;
using yourInvoice.Link.Application.LinkingProcess.UpdateBank;
using yourInvoice.Link.Application.LinkingProcess.UpdateExposure;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.UpdateFinancial;
using yourInvoice.Link.Application.LinkingProcess.UpdateGeneralInformation;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalCommercialAndBankReference;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalBoardDirector;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalFinancial;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalShareholder;
using yourInvoice.Link.Application.LinkingProcess.UpdatePersonalReference;
using yourInvoice.Link.Application.LinkingProcess.UpdateSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.UpdateWorking;
using System.ComponentModel.DataAnnotations;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholderBoardDirector;
using yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderBoardDirector;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.GetUrlTruora;
using yourInvoice.Link.Application.LinkingProcess.ValidateProcessTruora;
using yourInvoice.Link.Application.LinkingProcess.SignDocs;
using yourInvoice.Link.Application.LinkingProcess.SignSuccessDocs;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.GenerateDocuments;
using yourInvoice.Link.Application.LinkingProcess.GetDocumentsByRelatedId;
using yourInvoice.Link.Application.LinkingProcess.GetDocumentBase64ById;
using yourInvoice.Link.Application.LinkingProcess.GetLinkStatusEnable;
using yourInvoice.Link.Application.LinkingProcess.SendRequestUpdateDocuments;
using yourInvoice.Common.Entities;
using yourInvoice.Link.Application.LinkingProcess.ApproveLink;
using yourInvoice.Link.Application.LinkingProcess.RejectLink;

using yourInvoice.Link.Application.LinkingProcess.ListLinkingProcess;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Common.Business.CatalogModule;

namespace yourInvoice.Link.Controllers
{
    [Route("api/linkingprocess")]
    public class LinkingProcessController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMediator mediator;

        public LinkingProcessController(ISender sender, IMediator mediator)
        {
            _mediator = sender ?? throw new ArgumentNullException(nameof(sender));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("Generate/Docs/{idGeneralInformation}")]
        public async Task<IActionResult> GenerateDocsAsync(Guid idGeneralInformation)
        {
            var createResult = await _mediator.Send(new GenerateDocumentsCommand(idGeneralInformation));

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("createexposure")]
        public async Task<IActionResult> AddExposureAsync([FromBody] CreateExposureCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("updateexposure")]
        public async Task<IActionResult> UpdateExposureAsync([FromBody] UpdateExposureCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getexposure/{idGeneralInformation}")]
        public async Task<IActionResult> GetExposureAsync([Required] Guid idGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetExposureQuery(idGeneralInformation));

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getexposure/questionanswer")]
        public async Task<IActionResult> GetExposureQuestionAnswerAsync()
        {
            var createResult = await _mediator.Send(new GetExposureQuestionAnswerQuery());

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("listEconomicActivities")]
        public async Task<IActionResult> ListEconomicActivitiesAsync()
        {
            var result = await _mediator.Send(new ListEconomicActivityQuery());
            return result.Match(
              list => Ok(list),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("createworking")]
        public async Task<IActionResult> CreateWorkingAsync([FromBody] CreateWorkingCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                working => Ok(working),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("updateworking")]
        public async Task<IActionResult> UpdateWorkingAsync([FromBody] UpdateWorkingCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                updateWorking => Ok(updateWorking),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getworking/{idGeneralInformation}")]
        public async Task<IActionResult> GetWorkingAsync([Required] Guid idGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetWorkingQuery(idGeneralInformation));

            return createResult.Match(
                getworking => Ok(getworking),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GeneralInformation/{Id}")]
        public async Task<IActionResult> GetInformationAsync([Required] Guid Id)
        {
            var result = await _mediator.Send(new GetGeneralInformationQuery(Id));
            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("updateGeneralInformation")]
        public async Task<IActionResult> UpdateGeneralInformationAsync([FromBody] UpdateGeneralInformationCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                generalInformationResponse => Ok(generalInformationResponse),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateBankInformation")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBankCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateBankInformation")]
        public async Task<IActionResult> UpdateBankInformationAsync([FromBody] UpdateBankCommand command)
        {
            var updateResult = await _mediator.Send(command);

            return updateResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetBankInformation/{idGeneralInformation}")]
        public async Task<IActionResult> GetbankInformationAsync([Required] Guid idGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetBankQuery(idGeneralInformation));

            return getResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateFinancialInformation")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFinancialCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateFinancialInformation")]
        public async Task<IActionResult> UpdateFinancialInformationAsync([FromBody] UpdateFinancialCommand command)
        {
            var updateResult = await _mediator.Send(command);

            return updateResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetFinancialInformation/{idGeneralInformation}")]
        public async Task<IActionResult> GetFinancialInformationAsync([Required] Guid idGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetFinancialQuery(idGeneralInformation));

            return getResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetPersonalReference/{idGeneralInformation}")]
        public async Task<IActionResult> GetPersonalReferenceAsync([Required] Guid idGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetPersonalReferenceQuery(idGeneralInformation));

            return getResult.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreatePersonalReference")]
        public async Task<IActionResult> CreatePersonalReferenceAsync([FromBody] CreatePersonalReferenceCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdatePersonalReference")]
        public async Task<IActionResult> UpdatePersonalReferenceAsync([FromBody] UpdatePersonalReferenceCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getstatusform/{idGeneralInformation}")]
        public async Task<IActionResult> GetStatusFormAsync([Required] Guid idGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetStatusFormQuery(idGeneralInformation));

            return createResult.Match(
                statusform => Ok(statusform),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getstatusformlegal/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetStatusFormLegalAsync([Required] Guid idLegalGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetStatusFormLegalQuery(idLegalGeneralInformation));

            return createResult.Match(
                statusform => Ok(statusform),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getlegalgeneralinformation/{id}")]
        public async Task<IActionResult> GetLegalGeneralInformationAsync([Required] Guid id)
        {
            var createResult = await _mediator.Send(new GetLegalGeneralInformationQuery(id));

            return createResult.Match(
                legalGeneral => Ok(legalGeneral),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("createlegalgeneralinformation")]
        public async Task<IActionResult> CreateLegalGeneralInformationAsync([FromBody] CreateLegalGeneralInformationCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                legalGeneral => Ok(legalGeneral),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalFinancialInformation")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLegalFinancialCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                id => Ok(id),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateLegalFinancialInformation")]
        public async Task<IActionResult> UpdateLegalFinancialInformationAsync([FromBody] UpdateLegalFinancialCommand command)
        {
            var updateResult = await _mediator.Send(command);

            return updateResult.Match(
                id => Ok(id),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLegalFinancialInformation/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalFinancialInformationAsync([Required] Guid idLegalGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetLegalFinancialQuery(idLegalGeneralInformation));

            return getResult.Match(
                id => Ok(id),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetSignatureDeclaration/{idGeneralInformation}")]
        public async Task<IActionResult> GetSignatureDeclarationAsync([Required] Guid idGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetSignatureDeclarationQuery(idGeneralInformation));

            return createResult.Match(
                statusform => Ok(statusform),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateSignatureDeclaration")]
        public async Task<IActionResult> UpdateSignatureDeclarationAsync([FromBody] UpdateSignatureDeclarationCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateSignatureDeclaration")]
        public async Task<IActionResult> CreateSignatureDeclarationAsync([FromBody] CreateSignatureDeclarationCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getlegalrepresentativetaxauditor/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalRepresentativeTaxAuditorAsync([Required] Guid idLegalGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetLegalRepresentativeTaxAuditorQuery(idLegalGeneralInformation));
            return createResult.Match(
                getLegalRepresentative => Ok(getLegalRepresentative),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("createlegalrepresentativetaxauditor")]
        public async Task<IActionResult> CreateLegalRepresentativeTaxAuditorAsync([FromBody] CreateLegalRepresentativeTaxAuditorCommand command)
        {
            var createResult = await _mediator.Send(command);
            return createResult.Match(
                legalRepresentative => Ok(legalRepresentative),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalShareholder")]
        public async Task<IActionResult> CreateLegalShareholderAsync([FromBody] CreateLegalShareholderCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLegalShareholdersById/{id_LegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalShareholdersByIdAsync([Required] Guid id_LegalGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetLegalShareholderByIdQuery(id_LegalGeneralInformation));

            return getResult.Match(
                id => Ok(id),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateLegalShareholder")]
        public async Task<IActionResult> UpdateLegalShareholderAsync([FromBody] UpdateLegalShareholderCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [Route("DeleteLegalShareholder")]
        public async Task<IActionResult> DeleteLegalShareholderAsync([Required] Guid id, [Required] Guid id_LegalGeneralInformation)
        {
            var result = await _mediator.Send(new DeleteLegalShareholderCommand(id, id_LegalGeneralInformation));

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalBoardDirector")]
        public async Task<IActionResult> CreateLegalBoardDirectorAsync([FromBody] CreateLegalBoardDirectorCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLegalBoardDirectorsById/{id_LegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalBoardDirectorByIdAsync([Required] Guid id_LegalGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetLegalBoardDirectorByIdQuery(id_LegalGeneralInformation));

            return getResult.Match(
                id => Ok(id),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLegalCommercialAndBankReference/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalCommercialAndBankReferenceAsync([Required] Guid idLegalGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetLegalCommercialAndBankReferenceQuery(idLegalGeneralInformation));

            return getResult.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalCommercialAndBankReference")]
        public async Task<IActionResult> CreateLegalCommercialAndBankReferenceAsync([FromBody] CreateLegalCommercialAndBankReferenceCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateLegalCommercialAndBankReference")]
        public async Task<IActionResult> UpdateLegalCommercialAndBankReferenceAsync([FromBody] UpdateLegalCommercialAndBankCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("UpdateLegalBoardDirector")]
        public async Task<IActionResult> UpdateLegalBoardDirectorAsync([FromBody] UpdateLegalBoardDirectorCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [Route("DeleteLegalBoardDirector")]
        public async Task<IActionResult> DeleteLegalBoardDirectorAsync([Required] Guid id, [Required] Guid id_LegalGeneralInformation)
        {
            var result = await _mediator.Send(new DeleteLegalBoardDirectorCommand(id, id_LegalGeneralInformation));

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalShareholderBoardDirector")]
        public async Task<IActionResult> CreateLegalShareholderBoardDirectorAsync([FromBody] CreateLegalShareholderBoardDirectorCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLegalShareholderBoardDirector/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetLegalShareholderBoardDirectorAsync([Required] Guid idLegalGeneralInformation)
        {
            var getResult = await _mediator.Send(new GetLegalShareholderBoardDirectorQuery(idLegalGeneralInformation));

            return getResult.Match(
                data => Ok(data),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("createSagrilaft")]
        public async Task<IActionResult> AddSagrilaftAsync([FromBody] CreateSagrilaftCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("updateSagrilaft")]
        public async Task<IActionResult> UpdateSagrilaftAsync([FromBody] UpdateSagrilaftCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getSagrilaft/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetSagrilafAsync([Required] Guid idLegalGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetSagrilaftQuery(idLegalGeneralInformation));

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("getSagrilaft/questionanswer")]
        public async Task<IActionResult> GetSagrilaftQuestionAnswerAsync()
        {
            var createResult = await _mediator.Send(new GetSagrilaftQuestionAnswerQuery());

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("Sign/Docs/{relatedId}")]
        public async Task<IActionResult> GetDocumentsByRelated([Required] Guid relatedId)
        {
            var result = await _mediator.Send(new GetDocumentsByRelatedIdQuery(relatedId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("Sign/DocsById/{idDocument}")]
        public async Task<IActionResult> GetDocumentsBase64ById([Required] Guid idDocument)
        {
            var result = await _mediator.Send(new GetDocumentBase64ByIdQuery(idDocument));

            return result.Match(
              docs => Ok(new Dictionary<string, string> { { "base64", docs[0] }, { "type", docs[1] } }),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("{generalInformationId}/truora/getUrl")]
        public async Task<IActionResult> GetUrlTruoraAsync([Required] Guid generalInformationId)
        {
            var createResult = await _mediator.Send(new GetUrlTruoraCommand(generalInformationId));

            return createResult.Match(
                url => Ok(url),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Este servicio se llama desde el iframe de truora
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("truora/validateProcess")]
        public async Task<IActionResult> ValidateProcessTruoraAsync([FromBody] ValidateProcessTruoraCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("{generalInformationId}/sign/confirm")]
        public async Task<IActionResult> SignDocs([Required] Guid generalInformationId)
        {
            var result = await _mediator.Send(new SignDocsLinkCommand(generalInformationId));
            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("{generalInformationId}/sign/success")]
        public async Task<IActionResult> SignSuccessDocs([Required] Guid generalInformationId)
        {
            var result = await _mediator.Send(new SignSuccessDocsLinkCommand(generalInformationId));
            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("GetLegalSignatureDeclaration/{idLegalGeneralInformation}")]
        public async Task<IActionResult> GetSagrilaftQuestionAnswerAsync([Required] Guid idLegalGeneralInformation)
        {
            var createResult = await _mediator.Send(new GetLegalSignatureDeclarationQuery(idLegalGeneralInformation));

            return createResult.Match(
                legalSignatureDeclaration => Ok(legalSignatureDeclaration),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("CreateLegalSignatureDeclaration")]
        public async Task<IActionResult> GetSagrilaftQuestionAnswerAsync([FromBody] CreateLegalSignatureDeclarationCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                legalSignatureDeclaration => Ok(legalSignatureDeclaration),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetLinkStatusDisabled/{idUserLink}")]
        public async Task<IActionResult> GetLinkStatusDisabledAsync([Required] Guid idUserLink)
        {
            var createResult = await _mediator.Send(new GetLinkStatusEnableQuery(idUserLink));

            return createResult.Match(
                statusLink => Ok(statusLink),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("request/documents/sendmail/{accountId}")]
        public async Task<IActionResult> SendRequestUpdateDocuments([Required] Guid accountId, [FromBody] RequestUpdateDocuments request)
        {
            var result = await _mediator.Send(new SendRequestUpdateDocumentCommand(accountId, request));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("listLinkingProcesses")]
        public async Task<IActionResult> ListLinkingProcessesAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListLinkingProcessQuery(pagination));
            return result.Match(
              list => Ok(list),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("ApproveLink/{idUserLink}")]
        public async Task<IActionResult> ApproveLinkAsync([Required] Guid idUserLink)
        {
            var createResult = await _mediator.Send(new ApproveLinkCommand(idUserLink));

            return createResult.Match(
                approveLink => Ok(approveLink),
                errors => Problem(errors)
            );
        }


        [HttpPost]
        [Route("RejectLink/{idUserLink}")]
        public async Task<IActionResult> RejectLinkAsync([Required] Guid idUserLink)
        {
            var createResult = await _mediator.Send(new RejectLinkCommand(idUserLink));

            return createResult.Match(
                rejectLink => Ok(rejectLink),
                errors => Problem(errors)
            );
        }


        [HttpPost]
        [Route("truora/ChangeLinkStatusToValidationRejected/{idUserLink}")]
        public async Task<IActionResult> ChangeLinkStatusToValidationRejectedAsync([Required] Guid idUserLink)
        {
            await mediator.Publish(new ChangeLinkStatusCommand() { IdUserLink = idUserLink, StatusLinkId = CatalogCodeLink_LinkStatus.ValidationRejected });

            return Ok(true);
        }

    }
}