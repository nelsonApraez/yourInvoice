///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.IdentityModel.Tokens;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.PdfModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Persistence.Configuration;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.GenerateDocument;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using System.Globalization;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GenerateDocuments
{
    public sealed class GenerateDocumentsCommandHandler : IRequestHandler<GenerateDocumentsCommand, ErrorOr<bool>>
    {
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IAccountRepository _accountRepository;

        private readonly IGeneralInformationRepository _generalInformationRepository;
        private readonly IBankInformationRepository _bankInformationRepository;
        private readonly IFinancialInformationRepository _financialInformationRepository;
        private readonly IExposureInformationRepository _exposureInformationRepository;
        private readonly IWorkingInformationRepository _workingInformationRepository;
        private readonly IPersonalReferenceRepository _personalReferenceRepository;
        private readonly ISignatureDeclarationRepository _signatureRespository;

        private readonly ILegalGeneralInformationRepository _legalGeneralInformationRepository;
        private readonly ILegalFinancialInformationRepository _legalFinancialInformationRepository;
        private readonly ILegalRepresentativeTaxAuditorRepository _legalRepresentativeTaxAuditorRepository;
        private readonly ILegalSAGRILAFTRepository _legalSAGRILAFTRepository;
        private readonly ILegalCommercialAndBankReferenceRepository _legalCommercialAndBankReferenceRepository;
        private readonly ILegalShareholderRepository _legalShareholderRepository;
        private readonly ILegalBoardDirectorRepository _legalBoardDirectorRepository;
        private readonly ILegalShareholderBoardDirectorRepository _legalShareholderBoardDirectorRepository;
        private readonly ILegalSignatureDeclarationRepository _legalSignatureDeclarationRepository;

        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWorkLink _unitOfWorkLink;

        private LinkingNaturalPerson naturalPerson;
        private LinkingLegalPerson legalPerson;

        private Table _tableLinking;
        private Table _tableSignature;
        private PdfFont pdfFontText;
        private PdfFont pdfFontTitle;

        private const string PathFontTitlePDF = "Assets/Fonts/DaxlinePro-Bold.otf";
        private const string PathFontTextPDF = "Assets/Fonts/Inter-VariableFont_opsz,wght.ttf";
        private const string PathImagePDF = "Assets/Images/Logo.png";
        private const string PathImageDocumentsPDF = "Assets/Images/Logo Documentos.png";
        private const string HeaderText = "Formato de vinculación de terceros";
        private const string TextNaturalPerson = "PERSONA NATURAL";
        private const string TextLegalPerson = "PERSONA JURÍDICA";
        private const string TextWatermark = "ESPACIO VACÍO:\nLa información contenida en este espacio\nno será tenida en cuenta.";

        private const string DocumentLinkingName = "Formato de vinculación.pdf";
        private const string DocumentRegistrationAuth = "Autorización Proveedor Tecnologico Vendedores.pdf";
        private const string DocumentBrokerContractBuyer = "Contrato de Corretaje Comprador.pdf";
        private const string Storage = "storage";

        private CultureInfo colombianCulture = new CultureInfo("es-CO");

        public GenerateDocumentsCommandHandler(IStorage storage, ICatalogBusiness catalogBusiness, IAccountRepository accountRepository,
            IGeneralInformationRepository generalInformationRepository, IBankInformationRepository bankInformationRepository,
            IFinancialInformationRepository financialInformationRepository, IExposureInformationRepository exposureInformationRepository,
            IWorkingInformationRepository workingInformationRepository, IPersonalReferenceRepository personalReferenceRepository,
            ISignatureDeclarationRepository signatureRespository, IDocumentRepository documentRepository, IUnitOfWorkLink unitOfWorkLink,
            ILegalGeneralInformationRepository legalGeneralInformationRepository, ILegalFinancialInformationRepository legalFinancialInformationRepository,
            ILegalRepresentativeTaxAuditorRepository legalRepresentativeTaxAuditorRepository, ILegalSAGRILAFTRepository legalSAGRILAFTRepository,
            ILegalCommercialAndBankReferenceRepository legalCommercialAndBankReferenceRepository, ILegalShareholderRepository legalShareholderRepository,
            ILegalBoardDirectorRepository legalBoardDirectorRepository, ILegalShareholderBoardDirectorRepository legalShareholderBoardDirectorRepository,
            ILegalSignatureDeclarationRepository legalSignatureDeclarationRepository
            )
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _generalInformationRepository = generalInformationRepository ?? throw new ArgumentNullException(nameof(generalInformationRepository));
            _bankInformationRepository = bankInformationRepository ?? throw new ArgumentNullException(nameof(bankInformationRepository));
            _financialInformationRepository = financialInformationRepository ?? throw new ArgumentNullException(nameof(financialInformationRepository));
            _exposureInformationRepository = exposureInformationRepository ?? throw new ArgumentNullException(nameof(exposureInformationRepository));
            _workingInformationRepository = workingInformationRepository ?? throw new ArgumentNullException(nameof(workingInformationRepository));
            _personalReferenceRepository = personalReferenceRepository ?? throw new ArgumentNullException(nameof(personalReferenceRepository));
            _signatureRespository = signatureRespository ?? throw new ArgumentNullException(nameof(signatureRespository));

            _legalGeneralInformationRepository = legalGeneralInformationRepository ?? throw new ArgumentNullException(nameof(legalGeneralInformationRepository));
            _legalFinancialInformationRepository = legalFinancialInformationRepository ?? throw new ArgumentNullException(nameof(legalFinancialInformationRepository));
            _legalRepresentativeTaxAuditorRepository = legalRepresentativeTaxAuditorRepository ?? throw new ArgumentNullException(nameof(legalRepresentativeTaxAuditorRepository));
            _legalSAGRILAFTRepository = legalSAGRILAFTRepository ?? throw new ArgumentNullException(nameof(legalSAGRILAFTRepository));
            _legalCommercialAndBankReferenceRepository = legalCommercialAndBankReferenceRepository ?? throw new ArgumentNullException(nameof(legalCommercialAndBankReferenceRepository));
            _legalShareholderRepository = legalShareholderRepository ?? throw new ArgumentNullException(nameof(legalShareholderRepository));
            _legalBoardDirectorRepository = legalBoardDirectorRepository ?? throw new ArgumentNullException(nameof(legalBoardDirectorRepository));
            _legalShareholderBoardDirectorRepository = legalShareholderBoardDirectorRepository ?? throw new ArgumentNullException(nameof(legalShareholderBoardDirectorRepository));
            _legalSignatureDeclarationRepository = legalSignatureDeclarationRepository ?? throw new ArgumentNullException(nameof(legalSignatureDeclarationRepository));

            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(unitOfWorkLink));
        }

        public async Task<ErrorOr<bool>> Handle(GenerateDocumentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Verify that the user exists before proceeding.
                var account = await _accountRepository.GetAccountIdAsync(request.idGeneralInformation);
                if (account == null)
                    return Error.Validation(MessageCodes.AccountNotExist, GetErrorDescription(MessageCodes.AccountNotExist));

                bool isNatural = account.PersonTypeId.Equals(CatalogCode_PersonType.Natural);
                bool isSeller = account.RoleId.Equals(CatalogCode_UserRole.Seller);

                //Data linkage is loaded according to the type of person, and it is validated that the status is completed for all its forms
                var isCompleted = await GetStatusAndDataLinkingProcess(isNatural, request.idGeneralInformation);
                if (!isCompleted)
                    return Error.Validation(MessageCodes.StatusLinkingNotCompleted, GetErrorDescription(MessageCodes.StatusLinkingNotCompleted));

                //Validate the existing documents for the user and check if they are signed.
                var documents = await _documentRepository.GetAllDocumentsByRelatedIdAsync(request.idGeneralInformation);
                var isSigned = documents.Any(x => x.IsSigned == true && x.RelatedId == request.idGeneralInformation);
                if (!isSigned)
                {
                    string storagePath = $"{Storage}/linking/{account.Id}/";

                    //Document generation with info.
                    MemoryStream pdfLinkingFormat = await GenerateLinkingFormatDocument(request.idGeneralInformation, isNatural);
                    //Upload in blob storage
                    await SaveInBlobAndDB(request.idGeneralInformation, pdfLinkingFormat, storagePath, DocumentLinkingName, documents, CatalogCode_DocumentType.LinkingFormat, cancellationToken);

                    #region BROKER CONTRACT BUYER
                    if (!isSeller)
                    {
                        //Document generation with info.
                        MemoryStream pdfBrokerContract = await GenerateBrokerContractDocument(request.idGeneralInformation, isNatural);
                        //Upload in blob and save in db.
                        await SaveInBlobAndDB(request.idGeneralInformation, pdfBrokerContract, storagePath, DocumentBrokerContractBuyer, documents, CatalogCode_DocumentType.BrokerContract, cancellationToken);
                    }
                    #endregion

                    #region REGISTRATION AUTH
                    if (!isNatural && isSeller)
                    {
                        //Document generation with info.
                        MemoryStream pdfRegistrationAuthorization = await GenerateDianRegistrationAuthorizationDocument(request.idGeneralInformation);
                        //Upload in blob and save in db.
                        await SaveInBlobAndDB(request.idGeneralInformation, pdfRegistrationAuthorization, storagePath, DocumentRegistrationAuth, documents, CatalogCode_DocumentType.DianRegistrationAuthorization, cancellationToken);
                    }
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                return Error.Validation(MessageCodes.ErrorInGenerateFile, GetErrorDescription(MessageCodes.ErrorInGenerateFile, ex.Message));
            }
        }

        private async Task<bool> GetStatusAndDataLinkingProcess(bool isNatural, Guid id)
        {
            var completeStatus = CatalogCodeLink_StatusForm.Complete;

            if (isNatural)
            {
                naturalPerson = await GetNaturalPersonAsync(id);

                return naturalPerson.GeneralInformation.Completed.Equals(completeStatus)
                    && naturalPerson.BankInformation.Completed.Equals(completeStatus)
                    && naturalPerson.FinancialInformation.Completed.Equals(completeStatus)
                    && naturalPerson.ExposureInformation.Completed.Equals(completeStatus)
                    && naturalPerson.WorkingInformation.Completed.Equals(completeStatus)
                    && naturalPerson.ReferenceInformation.Completed.Equals(completeStatus)
                    && naturalPerson.SignatureDeclarationInformation.Completed.Equals(completeStatus);
            }
            else
            {
                legalPerson = await GetLegalPersonAsync(id);

                return legalPerson.GeneralInformation.Completed.Equals(completeStatus)
                    && legalPerson.FinancialInformation.Completed.Equals(completeStatus)
                    && legalPerson.RepresentativeTaxAuditorInformation.Completed.Equals(completeStatus)
                    && legalPerson.SagrilaftInformation.Completed.Equals(completeStatus)
                    && legalPerson.ShareholderBoardDirectorInformation.Completed.Equals(completeStatus)
                    && legalPerson.SignatureInformation.Completed.Equals(completeStatus);
            }
        }

        private async Task<MemoryStream> GenerateLinkingFormatDocument(Guid id, bool isNatural)
        {
            LoadFonts();
            InitializeTables();

            if (isNatural)
                await AddNaturalPersonSections(id);
            else
                await AddLegalPersonSections(id);

            MemoryStream document = PDFTableBusiness.TableToPdf(_tableLinking, _tableSignature, PageSize.LETTER, this.pdfFontTitle, TextWatermark, isNatural);
            
            return document;
        }

        private async Task<MemoryStream> GenerateBrokerContractDocument(Guid id, bool isNatural)
        {
            CatalogItemInfo template = null;
            Dictionary<string, string> dataKey = null;

            var catalogs = await GetCatalogsAsync(new[] { ConstDataBase.Departments, ConstDataBase.City, ConstDataBase.IdType });
            if (isNatural)
            {
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.BrokerageContractDocumentPN);
                dataKey = GetDataBrokerContractNaturalPerson(catalogs);
            }
            else
            {
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.BrokerageContractDocumentPJ);
                dataKey = GetDataBrokerContractLegalPerson(catalogs);
            }

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, dataKey);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, PageSize.LETTER, PathImagePDF);

            return pdfTemplate;
        }

        private Dictionary<string, string> GetDataBrokerContractNaturalPerson(Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            var generalData = naturalPerson.GeneralInformation;

            var date = DateTime.Now;
            var name = ""
                + generalData.FirstName
                + (generalData.SecondName.IsNullOrEmpty() ? " " : $" {generalData.SecondName} ")
                + generalData.LastName
                + (generalData.SecondLastName.IsNullOrEmpty() ? " " : $" {generalData.SecondLastName}");

            var expeditionCity = !generalData.ExpeditionCountry.ToString().IsNullOrEmpty() ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.ExpeditionCountry) : new CatalogItemInfo();
            var textExpedition = expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : string.Empty;

            var city = ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.City);

            Dictionary<string, string> dataKey = new()
            {
                { "{{Nombre}}"                  , name },
                { "{{TipoDocumento}}"           , generalData.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)generalData.DocumentTypeId).Descripton : "" },
                { "{{Documento}}"               , ValidityEmpty(generalData.DocumentNumber) },
                { "{{LugarExpedición}}"         , textExpedition != string.Empty ? $" expedida en {textExpedition}" : "" },
                { "{{Dirección}}"               , ValidityEmpty(generalData.Address) },
                { "{{Telefono}}"                , ValidityEmpty(generalData.MovilPhoneNumber) },
                { "{{Correo}}"                  , ValidityEmpty(generalData.Email) },
                { "{{CiudadDepto}}"             , city.Descripton != string.Empty ? $"{city.Descripton}-{city.Name}" : " " },
                { "{{Ciudad}}"                  , generalData.City != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.City).Descripton : "" },
                { "{{Fecha}}"                   , $"{ExtensionFormat.GetNumberInLetters(date.Day)} ({date.Day}) días de {ExtensionFormat.GetNameMonth()} del año dos mil {ExtensionFormat.GetNumberInLetters(int.Parse(date.ToString("yy")))} ({date.Year})" },
                { "{{ExpediciónDocumento}}"     , textExpedition != string.Empty ? $" de {textExpedition}" : " " }
            };

            return dataKey;
        }

        private Dictionary<string, string> GetDataBrokerContractLegalPerson(Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            var generalData = legalPerson.GeneralInformation;
            var representativeTaxAuditor = legalPerson.RepresentativeTaxAuditorInformation;
            var date = DateTime.Now;

            var name = ""
                + representativeTaxAuditor.FirstName
                + (representativeTaxAuditor.SecondName.IsNullOrEmpty() ? " " : $" {representativeTaxAuditor.SecondName} ")
                + representativeTaxAuditor.LastName
                + (representativeTaxAuditor.SecondLastName.IsNullOrEmpty() ? " " : $" {representativeTaxAuditor.SecondLastName}");
            var expeditionCity = representativeTaxAuditor.ExpeditionCountry != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], Guid.Parse(representativeTaxAuditor.ExpeditionCountry)) : new CatalogItemInfo();
            var textExpedition = expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : string.Empty;
            var cityMunicipality = ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.CityId);
            var city = ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.CityId);


            Dictionary<string, string> dataKey = new()
            {
                  { "{{fullName}}"                  , name.ToUpper() },
                  { "{{documentType}}"              , representativeTaxAuditor.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)representativeTaxAuditor.DocumentTypeId).Descripton : "" },
                  { "{{identificationDocument}}"    , ValidityEmpty(representativeTaxAuditor.DocumentNumber) },
                  { "{{expeditionCity}}"            , textExpedition != string.Empty ? $" expedida en {textExpedition}" : "" },
                  { "{{legalRepresentative}}"       , "Representante Legal" },
                  { "{{companyLegalName}}"          , ValidityEmpty(generalData.CompanyName.ToUpper()) },
                  { "{{taxIdentificationNumber}}"   , ValidityEmpty(generalData.Nit) },
                  { "{{commercialRegistration}}"    , "" },
                  { "{{cityMunicipality}}"          , cityMunicipality.Descripton != string.Empty ? ($"{city.Descripton} ({city.Name})") : " " },
                  { "{{cityChamberCommerce}}"       , expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : " " },
                  { "{{cityDepto}}"                 , city.Descripton != string.Empty ? $"{city.Descripton} - {city.Name}" : " " },
                  { "{{address}}"                   , ValidityEmpty(generalData.Address) },
                  { "{{phone}}"                     , ValidityEmpty(generalData.PhoneNumber) },
                  { "{{email}}"                     , ValidityEmpty(representativeTaxAuditor.Email) },
                  { "{{city}}"                      , generalData.CityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.CityId).Descripton : "" },
                  { "{{date}}"                      ,  $"{ExtensionFormat.GetNumberInLetters(date.Day)} ({date.Day}) días de {ExtensionFormat.GetNameMonth()} del año dos mil {ExtensionFormat.GetNumberInLetters(int.Parse(date.ToString("yy")))} ({date.Year})" },
                  { "{{placeIssue}}"                , textExpedition != string.Empty ? $" de {textExpedition}" : "" }
            };

            return dataKey;
        }

        private async Task<MemoryStream> GenerateDianRegistrationAuthorizationDocument(Guid id)
        {
            var template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.DianRegistrationAuthorizationDocument);
            var catalogs = await GetCatalogsAsync(new[] { ConstDataBase.Departments, ConstDataBase.City, ConstDataBase.IdType });

            var generalData = legalPerson.GeneralInformation;
            var representativeData = legalPerson.RepresentativeTaxAuditorInformation;
            var date = DateTime.Now;
            var name = ""
                + representativeData.FirstName
                + (representativeData.SecondName.IsNullOrEmpty() ? " " : $" {representativeData.SecondName} ")
                + representativeData.LastName
                + (representativeData.SecondLastName.IsNullOrEmpty() ? " " : $" {representativeData.SecondLastName}");
            var expeditionCity = representativeData.ExpeditionCountry != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], Guid.Parse(representativeData.ExpeditionCountry)) : new CatalogItemInfo();
            var textExpedition = expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : string.Empty;

            Dictionary<string, string> dataKey = new()
            {
                { "{{Ciudad}}"                  , generalData.CityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)generalData.CityId).Descripton : "" },
                { "{{FechaCompleta}}"           , $"{date.Day} de {ExtensionFormat.GetNameMonth()} de {date.Year}" },
                { "{{NombreRepresentanteLegal}}", name.ToUpper() },
                { "{{TipoDocumento}}"           , representativeData.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)representativeData.DocumentTypeId).Descripton : "" },
                { "{{Documento}}"               , ValidityEmpty(representativeData.DocumentNumber) },
                { "{{LugarExpedición}}"         , expeditionCity.Descripton != string.Empty ? ($" de {expeditionCity.Descripton} ({expeditionCity.Name})") : " " },
                { "{{NombreEmpresa}}"           , ValidityEmpty(generalData.CompanyName) },
                { "{{NitEmpresa}}"              , $"{generalData.Nit}-{generalData.CheckDigit}" },
            };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, dataKey);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, PageSize.LETTER, PathImagePDF, true);
            return pdfTemplate;
        }

        private async Task SaveInBlobAndDB(Guid id, MemoryStream document, string storagePath, string documentName, List<Document> documents, Guid DocumentType, CancellationToken cancellationToken)
        {
            //Upload in blob
            object urlFormat = await _storage.UploadAsync(document.ToArray(), storagePath + documentName);

            //Save or update in db
            await SaveInDB(id, urlFormat, document.ToArray().Length, documentName, documents, DocumentType, cancellationToken);
        }

        private async Task SaveInDB(Guid relatedId, object urlFormat, long sizeLinkingFormat, string documentName, List<Document> documents, Guid DocumentType, CancellationToken cancellationToken)
        {
            var document = documents.Where(x => x.TypeId == DocumentType && x.RelatedId == relatedId).FirstOrDefault();

            //New document 
            if (document == null)
            {
                Document newDocument = new Document(
                    Guid.NewGuid(),
                    relatedId,
                    documentName,
                    DocumentType,
                    false,
                    urlFormat.ToString(),
                    sizeLinkingFormat.ToMegaByte());

                _documentRepository.Add(newDocument);
            }
            else //Document exist
            {
                document.FileSize = sizeLinkingFormat.ToMegaByte();
                _documentRepository.Update(document);
            }

            await _unitOfWorkLink.SaveChangesAsync(cancellationToken);
        }

        #region LINKING FORMAT
        private void LoadFonts()
        {
            pdfFontTitle = PdfFontFactory.CreateFont(PathFontTitlePDF, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            pdfFontText = PdfFontFactory.CreateFont(PathFontTextPDF, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
        }

        private void InitializeTables()
        {
            _tableLinking = new Table(new float[] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 })
                .SetBorderCollapse(BorderCollapsePropertyValue.SEPARATE);
            _tableSignature = new Table(48).SetBorderCollapse(BorderCollapsePropertyValue.SEPARATE);
        }

        #region NATURAL PERSON
        private async Task<LinkingNaturalPerson> GetNaturalPersonAsync(Guid id)
        {
            return new LinkingNaturalPerson
            {
                GeneralInformation = await _generalInformationRepository.GetGeneralInformationIdAsync(id),
                BankInformation = await _bankInformationRepository.GetbankInformationAsync(id),
                FinancialInformation = await _financialInformationRepository.GetFinancialInformationAsync(id),
                ExposureInformation = await _exposureInformationRepository.GetExposureAsync(id),
                WorkingInformation = await _workingInformationRepository.GetWorkingAsync(id),
                ReferenceInformation = await _personalReferenceRepository.GetPersonalReferenceAsync(id),
                SignatureDeclarationInformation = await _signatureRespository.GetSignatureDeclarationAsync(id)
            };
        }

        private async Task AddNaturalPersonSections(Guid id)
        {
            var catalogsKeys = new[] { ConstDataBase.Departments, ConstDataBase.City, ConstDataBase.IdType, ConstDataBase.Bank };
            var catalogs = await GetCatalogsAsync(catalogsKeys);

            var economicActivity = await _exposureInformationRepository.GetEconomicActitiesListAsync();

            var deptoCatalog = await _catalogBusiness.ListByCatalogAsync(ConstDataBase.Departments);
            var cityCatalog = await _catalogBusiness.ListByCatalogAsync(ConstDataBase.City);
            var bankCatalog = await _catalogBusiness.ListByCatalogAsync(ConstDataBase.Bank);
            var docTypeCatalog = await _catalogBusiness.ListByCatalogAsync(ConstDataBase.IdType);

            AddHeader(TextNaturalPerson, true);
            AddGeneralInformationSection(naturalPerson.GeneralInformation, catalogs, economicActivity.Data);
            AddBankingAndFinancialSection(naturalPerson.BankInformation, naturalPerson.FinancialInformation, catalogs);
            AddExposureSection(naturalPerson.ExposureInformation);
            AddWorkingSection(naturalPerson.WorkingInformation, catalogs);
            AddReferenceSection(naturalPerson.ReferenceInformation, catalogs);

            AddHeader(TextNaturalPerson, false);
            AddDeclarationsNaturalSection(naturalPerson.GeneralInformation, naturalPerson.ExposureInformation);
        }

        private void AddGeneralInformationSection(GeneralInformationResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs, List<ListEconomicActivityResponse> listEconomicActivity)
        {
            #region SECCIÓN INFORMACIÓN PERSONAL
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN PERSONAL")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer nombre")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                   .Add(new Paragraph("Segundo nombre")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.FirstName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(this.ValidityEmpty(data.SecondName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer Apellido")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                   .Add(new Paragraph("Segundo Apellido")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.LastName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(this.ValidityEmpty(data.SecondLastName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Num. documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Fecha expedición")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)data.DocumentTypeId).Descripton)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.DocumentNumber)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.ExpeditionDate != null ? ExtensionFormat.DateddMMyyyy(data.ExpeditionDate) : "")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Lugar expedición")));
            #endregion
            #region ROW
            var expeditionCity = data.ExpeditionCountry != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.ExpeditionCountry) : new CatalogItemInfo();

            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : " ")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Actividad económica (Codigo CIIU)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(listEconomicActivity.SingleOrDefault(x => x.Id == data.EconomicActivity).Description)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Actividad económica secundaria (Codigo CIIU)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.SecondaryEconomicActivity.ToString().IsNullOrEmpty() ? " " : listEconomicActivity.SingleOrDefault(x => x.Id == data.SecondaryEconomicActivity).Description)));
            #endregion
            #endregion

            #region SECCIÓN INFORMACIÓN DE CONTACTO
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN DE CONTACTO")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Correo electrónico")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.Email)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                   .Add(new Paragraph("Teléfono fijo")));
            _tableLinking
              .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                  .Add(new Paragraph("Teléfono celular")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.PhoneNumber))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(data.MovilPhoneNumber)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentState).Descripton)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.City).Descripton)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Dirección")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.Address)));
            #endregion
            #endregion

            #region SECCIÓN INFORMACIÓN DE CORRESPONDENCIA
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN DE CORRESPONDENCIA")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Teléfono")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.PhoneCorrespondence))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentStateCorrespondence != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentStateCorrespondence).Descripton : " ")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentStateCorrespondence != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.CityCorrespondence).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Dirección")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.AddressCorrespondence))));
            #endregion
            #endregion
        }

        private void AddBankingAndFinancialSection(GetBankResponse bankData, GetFinancialResponse financialData, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN INFORMACIÓN BANCARIA
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN BANCARIA")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                   .Add(new Paragraph("Referencia Bancaria")));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                   .Add(new Paragraph("Teléfono/Celular del banco")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Bank], (Guid)bankData.BankReference).Descripton)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(bankData.PhoneNumber)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Producto")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(bankData.BankProduct)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)bankData.DepartmentState).Descripton)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)bankData.City).Descripton)));
            #endregion
            #endregion

            #region SECCIÓN INFORMACIÓN FINANCIERA
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN FINANCIERA")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                   .Add(new Paragraph("Total Activos")));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Total Pasivos")));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Total Patrimonio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(Decimal.Parse(financialData.TotalAssets).ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(Decimal.Parse(financialData.TotalLiabilities).ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(Decimal.Parse(financialData.TotalWorth).ToString("C0", colombianCulture))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                   .Add(new Paragraph("Ingresos Mensuales")));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Egresos Mensuales")));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Otros ingresos")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(Decimal.Parse(financialData.MonthlyIncome).ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(Decimal.Parse(financialData.MonthlyExpenditures).ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(Decimal.Parse(financialData.OtherIncome).ToString("C0", colombianCulture))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Concepto de otros ingresos mensuales")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(financialData.DescribeOriginIncome))));
            #endregion
            #endregion
        }

        private void AddExposureSection(GetExposureResponse data)
        {
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN DE EXPOSICIÓN")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                   .Add(new Paragraph(data.ExposureAnswers.ElementAt(0).QuestionIdentifierDescription)));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph(data.ExposureAnswers.ElementAt(1).QuestionIdentifierDescription)));
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph(data.ExposureAnswers.ElementAt(2).QuestionIdentifierDescription)));
            #endregion
            #region ROW
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.ExposureAnswers.ElementAt(0).ResponseIdentifierDescription.ToUpper())));
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.ExposureAnswers.ElementAt(1).ResponseIdentifierDescription.ToUpper())));
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.ExposureAnswers.ElementAt(2).ResponseIdentifierDescription.ToUpper())));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph(data.ExposureAnswers.ElementAt(3).QuestionIdentifierDescription)));
            #endregion
            #region ROW
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.ExposureAnswers.ElementAt(4).ResponseIdentifierDescription.ToUpper())));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Detalle")));
            #endregion
            #region ROW
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.ExposureAnswers.ElementAt(3).ResponseDetail))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph(data.ExposureAnswers.ElementAt(4).QuestionIdentifierDescription)));
            #endregion
            #region ROW
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.ExposureAnswers.ElementAt(4).ResponseIdentifierDescription.ToUpper())));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Detalle")));
            #endregion
            #region ROW
            _tableLinking
            .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.ExposureAnswers.ElementAt(4).ResponseDetail))));
            #endregion
        }

        private void AddWorkingSection(GetWorkingResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN LABORAL")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ocupación")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.Profession)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Empresa donde trabaja")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.BusinessName))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Cargo")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.Position))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Télefono (Oficina)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.PhoneNumber.ToString()))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Departamento (Oficina)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentState != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentState).Descripton : " ")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ciudad/Municipio (Oficina)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.City != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.City).Descripton : " ")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Dirección (Oficina)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.Address))));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("¿Qué tipo de producto y/ servicio comercializa? (Independientes o comerciantes)")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityEmpty(data.WhatTypeProductServiceSell))));
            #endregion
        }

        private void AddReferenceSection(GetReferenceResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("REFERENCIA PERSONAL")));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Referencia personal")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.NamePersonalReference)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.PhoneNumber)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Empresa")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.NameBussines)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentState).Descripton)));
            #endregion

            #region ROW
            _tableLinking
               .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                   .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.City).Descripton)));
            #endregion
        }

        private void AddDeclarationsNaturalSection(GeneralInformationResponse data, GetExposureResponse exposure)
        {
            Paragraph title;
            Paragraph content;

            #region SECCIÓN DECLARACIÓN ORIGEN DE FONDOS
            title = new Paragraph("DECLARACIÓN ORIGEN DE FONDOS");
            content = new Paragraph("Declaro bajo la gravedad del juramento que mi patrimonio y los recursos con los que realizo mis actividades económicas, asi como con los que realizo las operaciones por intermedio de yourInvoice S.A., provienen de actividades licitas, en especial de las siguientes fuentes:"
                + $"\n {ValidityEmpty(exposure.DeclarationOriginFunds)}");

            AddDeclaration(title, content, 24, false, 90f);
            #endregion

            #region SECCIÓN DECLARACIONES
            title = new Paragraph("DECLARACIONES");
            content = new Paragraph($"Yo, {data.FirstName.ToUpper()} {data.LastName.ToUpper()} obrando en nombre propio, declaro, bajo la gravedad del juramento:"
                + "\n* Que toda la documentación e información aportada es veraz y exacta y no existe falsedad alguna en la misma, estando yourInvoice S.A., o a quien esta designe, facultada para efectuar las verificaciones que considere pertinentes y para dar por terminada cualquier relación comercial o jurídica, si verifica que ello no es así."
                + "\n* Que los recursos que serán comprometidos en la operación, no provendrán de ninguna actividad ilícita, ni los recursos que resulten de esta, serán destinados para actividades delictivas"
                + "\n* Que no me encuentro reportado en las listas internacionales vinculantes para Colombia, de conformidad con el derecho internacional (listas de lasNaciones Unidas) o en las listas de la OFAC y otras listas de control, estando yourInvoice S.A. facultada para efectuar las verificaciones y consultas a las listas que considere pertinentes y para dar por terminada cualquier relación comercial o jurídica si verifica que figuro en dichas listas."
                + "\n* Que no existen contra mí, investigaciones o procesos penales por delitos dolosos, estando yourInvoice S.A. facultada para efectuar las verificaciones que considere pertinentes en bases de datos y centrales de riesgo informaciones públicas nacionales o internacionales y para dar por terminada cualquier relación comercial o jurídica, si verifica que existen investigaciones o procesos o existen informaciones en dichas bases de datos públicas que puedan colocar a yourInvoice S.A. frente a un riesgo legal o reputacional."
                + "\n* Que en el evento en que suceda alguna de las circunstancias descritas en los párrafos anteriores, me comprometo a comunicarlo de inmediato a la yourInvoice S.A."
                + "\n* Que autorizo a yourInvoice S.A. a comunicar o reportar a las autoridades nacionales, cualquiera de las situaciones acá descritas, en caso de presentarse, así como a suministrar a dichas autoridades las informaciones que ellas requieran, exonerando a yourInvoice S.A. de toda responsabilidad por tal hecho."
                + "\n* Que ninguna otra persona natural o jurídica, tiene interés no legítimo en el contrato o relación jurídica de la entidad que represento con yourInvoice S.A."
                + "\n* Qué conozco que yourInvoice S.A. está en la obligación legal de solicitar las aclaraciones que estime pertinentes, en el evento en que pueda tener dudas razonables sobre mis operaciones o sobre el origen de mis activos, evento en el cual me comprometo a suministrar las respectivas aclaraciones. Si estas no son satisfactorias, autorizo a yourInvoice S.A., o quien esta designe, para dar por terminada cualquier relación jurídica"
                + "\n* Que he leído y acepto la Política de Tratamiento de Datos personales de yourInvoice SA, disponible en www.yourInvoice.co")
                .SetMultipliedLeading(0.75f)
                .SetPadding(0.25f);

            AddDeclaration(title, content, 24, false, 120f);
            #endregion

            #region SECCIÓN AUTORIZACIÓN DE VISITA, VERIFICACIÓN DE LA INFORMACIÓN, CONSULTA Y REPORTE
            title = new Paragraph("AUTORIZACIÓN DE VISITA, VERIFICACIÓN DE LA INFORMACIÓN, CONSULTA Y REPORTE");
            content = new Paragraph("Autorizo a yourInvoice S.A. o a la entidad que ella designe, a realizar la visita correspondiente, con el fin de verificar los datos que entrego en este documento y sus anexos, así mismo autorizo a verificar por cualquier medio la información que ha sido aportada"
                        + "\nPara estos efectos autorizo a yourInvoice S.A. a realizar consultas en las fuentes de información que estime necesarias."
                        + "\nAutorizo de manera irrevocable a yourInvoice S.A. a realizar las consultas que estime conveniente para conocer los antecedentes disciplinarios, penales,contractuales, fiscales, comerciales, financieros y de cualquier otra naturaleza, incluida la consulta a las centrales de riesgo o a cualquier otra entidad pública o privada que maneje o administre bases de datos de cualquier índole o a las listas nacionales o internacionales que estime pertinente.")
                        .SetMultipliedLeading(0.75f)
                        .SetPadding(0.25f);

            AddDeclaration(title, content, 24, false, 80f);
            #endregion
        }
        #endregion

        #region LEGAL PERSON
        private async Task<LinkingLegalPerson> GetLegalPersonAsync(Guid id)
        {
            return new LinkingLegalPerson
            {
                GeneralInformation = await _legalGeneralInformationRepository.GetLegalGeneralInformationAsync(id),
                FinancialInformation = await _legalFinancialInformationRepository.GetLegalFinancialInformationAsync(id),
                RepresentativeTaxAuditorInformation = await _legalRepresentativeTaxAuditorRepository.GetLegalRepresentativeTaxAuditorAsync(id),
                SagrilaftInformation = await _legalSAGRILAFTRepository.GetSagrilaftAsync(id),
                CommercialAndBankInformation = await _legalCommercialAndBankReferenceRepository.GetLegalCommercialAndBankReferenceAsync(id),
                ShareholdersInformation = await _legalShareholderRepository.GetLegalShareholdersById(id),
                BoardDirectorInformation = await _legalBoardDirectorRepository.GetLegalBoardDirectorById(id),
                ShareholderBoardDirectorInformation = await _legalShareholderBoardDirectorRepository.GetLegalShareholderBoardDirectorById(id),
                SignatureInformation = await _legalSignatureDeclarationRepository.GetLegalSignatureDeclarationAsync(id)
            };
        }

        private async Task AddLegalPersonSections(Guid id)
        {
            var catalogsKeys = new[] { ConstDataBase.Departments, ConstDataBase.City, ConstDataBase.IdType, ConstDataBase.CompanyType,
                ConstDataBase.TypeOfCompany, ConstDataBase.EconomicActivity, ConstDataBase.GreatContributor, ConstDataBase.IdselfRetaining,
                ConstDataBase.QuestionLegalFinancial, ConstDataBase.OperationType, ConstDataBase.AnswerQuestionLegalFinancial };
            var catalogs = await GetCatalogsAsync(catalogsKeys);

            var economicActivity = await _exposureInformationRepository.GetEconomicActitiesListAsync();

            AddHeader(TextLegalPerson, true);
            AddLegalGeneralInformationSection(legalPerson.GeneralInformation, catalogs, economicActivity.Data);
            AddLegalFinancialSection(legalPerson.FinancialInformation, catalogs);
            AddLegalTaxAuditorSection(legalPerson.RepresentativeTaxAuditorInformation, catalogs);
            AddLegalSagrilaftSection(legalPerson.SagrilaftInformation, catalogs);
            AddLegalCommercialAndBankSection(legalPerson.CommercialAndBankInformation, catalogs);
            AddLegalShareholderSection(legalPerson.ShareholdersInformation, catalogs);
            AddLegalBoardDirectorsSection(legalPerson.BoardDirectorInformation, catalogs);

            AddHeader(TextLegalPerson, false);
            AddDeclarationsLegalSection(legalPerson.GeneralInformation, legalPerson.RepresentativeTaxAuditorInformation, catalogs);
        }

        private void AddLegalGeneralInformationSection(GetLegalGeneralInformationResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs, List<ListEconomicActivityResponse> listEconomicActivity)
        {
            #region SECCIÓN INFORMACIÓN EMPRESARIAL
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN EMPRESARIAL")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("NIT")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                   .Add(new Paragraph("Dígito de verificación")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.Nit)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(data.CheckDigit)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Nombre razón social")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.CompanyName)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de empresa")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.CompanyTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.CompanyType], (Guid)data.CompanyTypeId).Descripton : "NO DEFINIDO")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de sociedad")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.SocietyTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.TypeOfCompany], (Guid)data.SocietyTypeId).Descripton : "NO DEFINIDO")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Detalle")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.SocietyTypeDetail))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Actividad económica")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.EconomicActivityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.EconomicActivity], (Guid)data.EconomicActivityId).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Detalle")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.EconomicActivityDetail))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Codigo CIIU")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(listEconomicActivity.SingleOrDefault(x => x.Id == data.CIIUCode).Description)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("¿Gran contribuyente?")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("¿Es autoretenedor?")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Tarifa")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.GreatContributorId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.GreatContributor], (Guid)data.GreatContributorId).Descripton : "")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.IsSelfRetaining != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdselfRetaining], (Guid)data.IsSelfRetaining).Descripton : "")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph((!data.Fee.ToString().IsNullOrEmpty() || data.Fee > 0) ? $"{data.Fee} %" : data.Fee.ToString())));
            #endregion
            #endregion

            #region SECCIÓN INFORMACIÓN DE CONTACTO
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN DE CONTACTO")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Correo electrónico corporativo")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.EmailCorporate)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Correo electrónico de facturas electrónicas")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.ElectronicInvoiceEmail)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Teléfono/Celular/Fax")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.PhoneNumber)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentId).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.CityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.CityId).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Dirección empresa oficina principal")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.Address)));
            #endregion
            #endregion

            #region SECCIÓN CONTACTO Y SUCURSAL
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("CONTACTO Y SUCURSAL")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 12, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 12, this.pdfFontTitle, false)
                    .Add(new Paragraph("Num. documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Teléfono/Celular/Fax")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 12, this.pdfFontText)
                    .Add(new Paragraph(data.BranchDocumentNumberTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)data.BranchDocumentNumberTypeId).Descripton : "")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 12, this.pdfFontText, false)
                    .Add(new Paragraph(data.BranchDocumentNumber)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.BranchPhoneNumber))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Dirección sucursal")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.BranchAddress))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.BranchDepartmentId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.BranchDepartmentId).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.BranchCityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.BranchCityId).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Correo electrónico de contacto")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.BranchEmailContact)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Cargo")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.BranchPosition)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Nombre contacto empresa")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Télefono de contacto")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.BranchContactName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(data.BranchContactPhone)));
            #endregion
            #endregion
        }

        private void AddLegalFinancialSection(GetLegalFinancialResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN INFORMACIÓN FINANCIERA
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("INFORMACIÓN FINANCIERA")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("Total Activos")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Total Pasivos")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Total egresos mensuales")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.TotalAssets.ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.TotalLiabilities.ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.TotalMonthlyIncome.ToString("C0", colombianCulture))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("Ingresos mensuales")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Otros ingresos mensuales")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                   .Add(new Paragraph("Total ingresos mensuales")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.MonthlyIncome.ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.OtherIncome.ToString("C0", colombianCulture))));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.TotalMonthlyExpenditures.ToString("C0", colombianCulture))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Concepto de otros ingresos mensuales")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.DescribeOriginIncome))));
            #endregion
            #endregion

            #region SECCIÓN OPERACIONES FINANCIERAS
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("OPERACIONES FINANCIERAS")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("¿Realiza operaciones en moneda extranjera?")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 32, this.pdfFontTitle, false)
                    .Add(new Paragraph("Tipo de operación")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.OperationsForeignCurrency != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.AnswerQuestionLegalFinancial], (Guid)data.OperationsForeignCurrency).Descripton : " ")));

            var textOperationType = "";
            int i = 0;
            foreach (var item in data.OperationsTypes)
            {
                textOperationType += item != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.OperationType], (Guid)item).Descripton : " ";
                if (item != null && i < data.OperationsTypes.Count() -1)
                    textOperationType += ", ";
                i++;
            }

            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 32, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(textOperationType))));
           
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Detalle")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.OperationTypeDetail))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("¿Posee cuentas corrientes en moneda extranjera?")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 32, this.pdfFontTitle, false)
                    .Add(new Paragraph("Número de cuenta")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.AccountsForeignCurrency != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.AnswerQuestionLegalFinancial], (Guid)data.AccountsForeignCurrency).Descripton : " ")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 32, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.AccountNumber))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 32, this.pdfFontTitle)
                    .Add(new Paragraph("Banco")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Monto")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 32, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.Bank))));
            var amount = (long)(data.Amount.HasValue ? data.Amount : 0);
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(amount > 0 ? amount.ToString("C0", colombianCulture) : amount.ToString())));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("País/Ciudad")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.City))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Moneda")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(ValidityEmpty(data.Currency))));
            #endregion
            #endregion

        }

        private void AddLegalTaxAuditorSection(LegalRepresentativeTaxAuditor data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN REPRESENTANTE LEGAL
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("REPRESENTANTE LEGAL")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer nombre")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Segundo nombre")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.FirstName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.SecondName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer Apellido")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Segundo Apellido")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.LastName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.SecondLastName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Num. documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Fecha expedición")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)data.DocumentTypeId).Descripton : "")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.DocumentNumber)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.ExpeditionDate != null ? ExtensionFormat.DateddMMyyyy(data.ExpeditionDate) : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Lugar expedición")));
            #endregion
            #region ROW
            var expeditionCity = data.ExpeditionCountry != null ? this.ValidityDescriptionCatalog(catalogs[ConstDataBase.City], Guid.Parse(data.ExpeditionCountry)) : new CatalogItemInfo();

            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(expeditionCity.Descripton != string.Empty ? ($"{expeditionCity.Descripton} ({expeditionCity.Name})") : " ")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Correo electrónico")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.Email)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Dirección domicilio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.HomeAddress)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Teléfono")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.Phone)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentState != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentState).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.City != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.City).Descripton : "")));
            #endregion
            #endregion

            #region SECCIÓN REVISOR FISCAL
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("REVISOR FISCAL")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer nombre")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Segundo nombre")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.TaxAuditorFirstName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.TaxAuditorSecondName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle)
                    .Add(new Paragraph("Primer Apellido")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 24, this.pdfFontTitle, false)
                    .Add(new Paragraph("Segundo Apellido")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText)
                    .Add(new Paragraph(data.TaxAuditorLastName)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 24, this.pdfFontText, false)
                    .Add(new Paragraph(ValidityEmpty(data.TaxAuditorSecondLastName))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Num. documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText)
                    .Add(new Paragraph(data.TaxAuditorDocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)data.TaxAuditorDocumentTypeId).Descripton : "")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.TaxAuditorDocumentNumber)));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                    .Add(new Paragraph(data.TaxAuditorPhoneNumber)));
            #endregion
            #endregion
        }

        private void AddLegalSagrilaftSection(GetSagrilaftResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN INFORMACIÓN SAGRILAFT 
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph($"INFORMACIÓN SAGRILAFT\n(SISTEMA DE AUTOCONTROL Y GESTIÓN DEL RIESGO INTEGRAL DE LAVADO DE ACTIVOS Y FINANCIACIÓN DEL TERRORISMO)")
                        .SetMultipliedLeading(1f)));
            #endregion

            foreach (var item in data.SagrilaftAnswers)
            {
                #region ROW
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                        .Add(new Paragraph(item.QuestionIdentifierDescription).SetMultipliedLeading(1f)));
                #endregion
                #region ROW
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(item.ResponseIdentifierDescription.ToUpper())));
                #endregion
                if (item.ResponseDetail != null)
                {
                    #region ROW
                    _tableLinking
                        .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                            .Add(new Paragraph("Detalle")));
                    #endregion
                    #region ROW
                    _tableLinking
                        .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                            .Add(new Paragraph(ValidityEmpty(item.ResponseDetail))));
                    #endregion
                }

            }
            #endregion
        }

        private void AddLegalCommercialAndBankSection(LegalCommercialAndBankReferenceResponse data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN REFERENCIAS COMERCIALES
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("REFERENCIAS COMERCIALES")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Referencia comercial")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.CommercialReference)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.PhoneNumberCommercial)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentStateCommercial != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentStateCommercial).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.CityCommercial != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.CityCommercial).Descripton : "")));
            #endregion
            #endregion

            #region SECCIÓN REFERENCIAS BANCARIAS
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("REFERENCIAS BANCARIAS")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Referencia bancaria")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(data.BankReference)));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                        .Add(new Paragraph(ValidityEmpty(data.PhoneNumberBank))));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Departamento")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.DepartmentStateBank != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.Departments], (Guid)data.DepartmentStateBank).Descripton : "")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("Ciudad/Municipio")));
            #endregion
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(3, 1, 48, this.pdfFontText)
                    .Add(new Paragraph(data.CityBank != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.CityBank).Descripton : "")));
            #endregion
            #endregion
        }

        private void AddLegalShareholderSection(List<GetLegalShareholderResponse> data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN ACCIONISTAS
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("ACCIONISTAS O SOCIOS")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 12, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 10, this.pdfFontTitle, false)
                    .Add(new Paragraph("Documento de identidad")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Nombre completo o razón social")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 10, this.pdfFontTitle, false)
                    .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            foreach (var item in data)
            {
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 12, this.pdfFontText)
                            .Add(new Paragraph(item.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)item.DocumentTypeId).Descripton : " ")));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 10, this.pdfFontText, false)
                            .Add(new Paragraph(item.DocumentNumber)));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                            .Add(new Paragraph(item.FullNameCompanyName)));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 10, this.pdfFontText, false)
                            .Add(new Paragraph(ValidityEmpty(item.PhoneNumber))));
            }
            #endregion
            #endregion
        }

        private void AddLegalBoardDirectorsSection(List<GetLegalBoardDirectorResponse> data, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            #region SECCIÓN JUNTA DIRECTIVA, CONSEJO DE ADMINISTRACIÓN O SU EQUIVALENTE
            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(new Paragraph("JUNTA DIRECTIVA, CONSEJO DE ADMINISTRACIÓN O SU EQUIVALENTE")));
            #endregion

            #region ROW
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 12, this.pdfFontTitle)
                    .Add(new Paragraph("Tipo de documento")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 10, this.pdfFontTitle, false)
                    .Add(new Paragraph("Documento de identidad")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 16, this.pdfFontTitle, false)
                    .Add(new Paragraph("Nombre completo o razón social")));
            _tableLinking
                .AddCell(PDFTableBusiness.SetCell(2, 1, 10, this.pdfFontTitle, false)
                    .Add(new Paragraph("Teléfono/Celular")));
            #endregion
            #region ROW
            foreach (var item in data)
            {
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 12, this.pdfFontText)
                            .Add(new Paragraph(item.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)item.DocumentTypeId).Descripton : " ")));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 10, this.pdfFontText, false)
                            .Add(new Paragraph(item.DocumentNumber)));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 16, this.pdfFontText, false)
                            .Add(new Paragraph(item.FullNameCompanyName)));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(3, 1, 10, this.pdfFontText, false)
                            .Add(new Paragraph(ValidityEmpty(item.PhoneNumber))));
            }
            #endregion
            #endregion
        }

        private void AddDeclarationsLegalSection(GetLegalGeneralInformationResponse data, LegalRepresentativeTaxAuditor representative, Dictionary<string, IEnumerable<CatalogItemInfo>> catalogs)
        {
            Paragraph title;
            Paragraph content;

            #region SECCIÓN DECLARACIÓN ORIGEN DE FONDOS
            title = new Paragraph("DECLARACIÓN ORIGEN DE FONDOS");
            content = new Paragraph("Declaro bajo la gravedad del juramento que mi patrimonio y los recursos con los que realizo mis actividades económicas, asi como con los que realizo las operaciones por intermedio de yourInvoice S.A., provienen de actividades licitas, en especial de las siguientes fuentes:"
                + $"\n {ValidityEmpty(data.OriginResources)}")
                .SetFontSize(7f);

            AddDeclaration(title, content, 16, true, 40f);
            #endregion

            #region SECCIÓN SAGRILAFT
            title = new Paragraph("COMPROMISO DE ADOPTAR EL SISTEMA DE GESTIÓN DE RIESGO DE VALADO DE ACTIVOS/FINANCIACIÓN DE TERRORISMO QUE DEFINA yourInvoice S.A.").SetMultipliedLeading(0.75f);
            content = new Paragraph("Manifestamos nuestro compromiso para adoptar y acoger integralmente las políticas y procedimientos que yourInvoice S.A defina para la administración y prevención de los riesgos asociados al Lavado de Activos/Financiación de Terrorismo.")
                .SetFontSize(7f);

            AddDeclaration(title, content, 24, false, 20f, false);
            #endregion

            #region SECCIÓN INFORMACIÓN APORTADA
            title = new Paragraph("RESPONSABILIDA POR LA INFORMACIÓN APORTADA");
            content = new Paragraph("Certificamos que la información presentada en este fomulario y los documentos anexos es verídica y corresponde a la realidad. En caso de inexactitud, falsedad o inconsistencia en la información aportada, la entidad que represento será civilmente responsable ante yourInvoice S.A y terceros afectados, por los perjuicios que esta circustancia pudiera ocasionarles."
                + "\nAcepto que en el evento en que la empresa, los socios o el representante legal se encuentren reportados en algunas de las listas restricitivas definidas por yourInvoice S.A, al momento de la vinculación o durante la relación contractual, yourInvoice S.A podrá para dar por terminado unilateralmente el contrato en cualquier momento y sin previo aviso,por configurarse una causal objetiva de terminación del mismo."
                + "\nAceptamos y damos por entendido que cualquier irregularidad en el proceso de vinculación, hará que este no continúe y constituirá una causal de terminación anticipada del vínculo contractual que se llegara a tener con yourInvoice S.A., sin que por ello se genere el reconocimiento de indemnizaciones o pagos de perjuicios.")
                .SetFontSize(7f);

            AddDeclaration(title, content, 24, false, 20f, false);
            #endregion

            #region SECCIÓN AUTORIZACIÓN DE VISITA, VERIFICACIÓN DE LA INFORMACIÓN, CONSULTA Y REPORTE
            title = new Paragraph("AUTORIZACIÓN DE VISITA, VERIFICACIÓN DE LA INFORMACIÓN, CONSULTA Y REPORTE");
            content = new Paragraph("Autorizamos a yourInvoice S.A. o a la entidad que ella designe, a visitar nuestras instalaciones con el fin de verificar los datos que entregamos en este documento y sus anexos,asi mismo autorizamos verificar por cualquier medio la información que ha sido apartada por nosotros. Para estos efectos autorizamos a yourInvoice S.A. a realizar consultas en las fuentes de información que estime necesarias.\n"
                        + "Autorizamos de manera irrevocable a yourInvoice S.A. a realizar las consultas que estime convenientes para conocer los antecedentes disciplinarios, penales, contractuales, fiscales, comerciales, financieros y de cualquier otra naturaleza, de la empresa, sus administradores, representantes legales, accionistas o socios con más del 5% departicipación en el capital social, incluida la consulta a las centrales de riesgo o a cualquier otra entidad pública o privada que maneje o administre bases de datos de cualquier índole o las listas nacionales o internacionales que estime pertinente.")
                .SetFontSize(7f);

            AddDeclaration(title, content, 16, true, 40f);
            #endregion

            #region SECCIÓN DECLARACIONES
            var nameRepresentative = ""
                + representative.FirstName
                + (representative.SecondName.IsNullOrEmpty() ? " " : $" {representative.SecondName} ")
                + representative.LastName
                + (representative.SecondLastName.IsNullOrEmpty() ? " " : $" {representative.SecondLastName} ");
            var document = (representative.DocumentTypeId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.IdType], (Guid)representative.DocumentTypeId).Descripton + " " : "") + representative.DocumentNumber;
            var date = DateTime.Now;
            var city = data.CityId != null ? ValidityDescriptionCatalog(catalogs[ConstDataBase.City], (Guid)data.CityId).Descripton : " ";
            var declaration = $"Yo, {nameRepresentative.ToUpper()}con {document} obrando en representación legal de {data.CompanyName.ToUpper()}, {data.Nit}-{data.CheckDigit} declaro, bajo la gravedad del juramento:";
            var declarationDay = $"Como constancia de haber leido, entendido y aceptado lo anterior, declaro que la información que he suministrado es exacta en todas sus partes y firmo el presente documento "
                + $"a los {date.Day} dias del mes de {ExtensionFormat.GetNameMonth().ToUpper()} del año {date.Year}, en la ciudad de {city.ToUpper()}.";
            title = new Paragraph("DECLARACIONES");
            content = new Paragraph(declaration
                + "\n  1. Que toda la documentación e información aportada es veraz y exacta y no existe falsedad alguna en la misma, estando yourInvoice S.A., o a quien esta designe, facultada para efectuar las verificaciones que considere pertinentes y para dar por terminada cualquier relación comercial o jurídica, si verifica que ello no es así."

                + "\n\n" + declarationDay).SetFontSize(7f);


            AddDeclaration(title, content, 16, true, 40f);
            #endregion            
        }
        #endregion

        private void AddHeader(string person, bool isLinking)
        {
            #region HEADER
            ImageData img = ImageDataFactory.Create(PathImagePDF);

            if (isLinking)
            {
                _tableLinking
                .AddHeaderCell(PDFTableBusiness.SetCell(0, 5, 12, this.pdfFontTitle)
                    .Add(new Image(img).SetAutoScale(true)));
                _tableLinking
                   .AddHeaderCell(PDFTableBusiness.SetCell(0, 5, 36, this.pdfFontTitle, false)
                       .Add(new Paragraph($"{HeaderText}\n{person}")
                            .SetMultipliedLeading(1f)));

                #region ROW
                var date = ExtensionFormat.DateddMMyyyy(ExtensionFormat.DateTimeCO()).Split("/");

                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 13, this.pdfFontTitle, true, false)
                        .Add(new Paragraph("Fecha de diligenciamiento:"))
                        .SetFontColor(new DeviceRgb(0, 74, 160))
                        .SetPaddingLeft(1f));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 2, this.pdfFontTitle, false, false)
                        .Add(new Paragraph("Día"))
                        .SetFontColor(new DeviceRgb(0, 74, 160))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 1, this.pdfFontText, false, false)
                        .Add(new Paragraph(date[0]))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 2, this.pdfFontTitle, false, false)
                        .Add(new Paragraph("Mes"))
                        .SetFontColor(new DeviceRgb(0, 74, 160))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 1, this.pdfFontText, false, false)
                        .Add(new Paragraph(date[1]))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 2, this.pdfFontTitle, false, false)
                        .Add(new Paragraph("Año"))
                        .SetFontColor(new DeviceRgb(0, 74, 160))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 3, this.pdfFontText, false, true)
                        .Add(new Paragraph(date[2]))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingLeft(0)
                        .SetPaddingRight(0));

                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 10, this.pdfFontTitle, false, false)
                        .Add(new Paragraph("Tipo de registro"))
                        .SetFontColor(new DeviceRgb(0, 74, 160))
                        .SetPaddingLeft(1f));
                _tableLinking
                    .AddCell(PDFTableBusiness.SetCell(4, 1, 14, this.pdfFontText, false, true)
                        .Add(new Paragraph("NUEVO"))
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetPadding(0f));
                #endregion
            }
            else
            {
                _tableSignature
                        .AddHeaderCell(PDFTableBusiness.SetCell(0, 5, 2, this.pdfFontTitle)
                            .Add(new Image(img).SetMaxWidth(500f).SetAutoScale(true)));
                _tableSignature
                   .AddHeaderCell(PDFTableBusiness.SetCell(0, 5, 46, this.pdfFontTitle, false)
                       .Add(new Paragraph($"{HeaderText}\n{person}")
                            .SetMultipliedLeading(1f)));
            }

            #endregion
        }

        private void AddDeclaration(Paragraph title, Paragraph content, int col, bool isLegal, float heightSignature, bool isSignature = true)
        {
            #region ROW
            _tableSignature
                .AddCell(PDFTableBusiness.SetCell(1, 1, 48, this.pdfFontTitle)
                    .Add(title));
            #endregion

            if (isSignature)
            {
                #region ROW
                _tableSignature
                   .AddCell(PDFTableBusiness.SetCell(5, 1, 48, this.pdfFontText)
                        .Add(content.SetMultipliedLeading(0.75f).SetPaddings(3f, 3f, 0f, 3f))
                        .SetBorderBottom(Border.NO_BORDER));
                #endregion
                #region ROW
                _tableSignature
                   .AddCell(PDFTableBusiness.SetCell(5, 1, col, this.pdfFontText, true, false)
                        .Add(new Paragraph(isLegal ? "NOMBRE REPRESENTANTE LEGAL:" : "FIRMA:")
                            .SetPaddings(0f, 0f, 3f, 3f)
                            )
                        .SetHeight(heightSignature)
                        .SetVerticalAlignment(VerticalAlignment.BOTTOM));

                if (isLegal)
                {
                    _tableSignature
                        .AddCell(PDFTableBusiness.SetCell(5, 1, col, this.pdfFontText, false, false)
                            .Add(new Paragraph("FIRMA:")
                                .SetPaddings(0f, 0f, 3f, 0f)
                                )
                            .SetHeight(heightSignature)
                            .SetVerticalAlignment(VerticalAlignment.BOTTOM));
                }
                _tableSignature
                  .AddCell(PDFTableBusiness.SetCell(5, 1, col, this.pdfFontText, false)
                       .Add(new Paragraph("C.C:")
                           .SetPaddings(0f, 0f, 3f, 0f)
                           )
                       .SetHeight(heightSignature)
                       .SetVerticalAlignment(VerticalAlignment.BOTTOM));
                #endregion
            }
            else
            {
                #region ROW
                _tableSignature
                   .AddCell(PDFTableBusiness.SetCell(5, 1, 48, this.pdfFontText)
                        .Add(content.SetMultipliedLeading(0.75f).SetPaddings(3f, 3f, 3f, 3f)));
                #endregion
            }
        }

        private async Task<Dictionary<string, IEnumerable<CatalogItemInfo>>> GetCatalogsAsync(string[] catalogsKeys)
        {
            var catalogDictionary = new Dictionary<string, IEnumerable<CatalogItemInfo>>();

            foreach (var key in catalogsKeys)
            {
                var catalogItem = await _catalogBusiness.ListByCatalogAsync(key);
                catalogDictionary[key] = catalogItem;
            }

            return catalogDictionary;
        }

        private string ValidityEmpty(string value)
        {
            return value.IsNullOrEmpty() ? " " : value;
        }

        private CatalogItemInfo ValidityDescriptionCatalog(IEnumerable<CatalogItemInfo> catalog, Guid id)
        {
            var itemById = catalog.SingleOrDefault(x => x.Id == id);

            return itemById != null ? itemById : new CatalogItemInfo();
        }
        #endregion
    }
}
