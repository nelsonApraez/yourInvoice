///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Business.CatalogModule
{
    #region Offer

    public static class CatalogCode_OfferStatus
    {
        public static readonly Guid InProgress = Guid.Parse("51ED6EC6-F55E-4301-8BB8-CD425531856F");
        public static readonly Guid Enabled = Guid.Parse("BB421108-ED7F-4CA4-8AED-05E4F6139285");
        public static readonly Guid Purchased = Guid.Parse("E23D5024-78B7-465E-95E7-F0509E41AA4E");
    }

    #endregion Offer

    #region Invoice

    public static class CatalogCode_InvoiceStatus
    {
        public static readonly Guid Approved = Guid.Parse("7FDDF6C1-F14D-4254-81DE-BA0759360C82");
        public static readonly Guid InProgress = Guid.Parse("27614A95-911A-4204-B49F-E6D5B9D530B4");
        public static readonly Guid Rejected = Guid.Parse("8BC21F26-518D-4B71-BC48-D899E5886682");
        public static readonly Guid ValidationDian = Guid.Parse("BF6FB92C-D0F1-4AB7-9D60-D8B0289D8C7B");
        public static readonly Guid Loaded = Guid.Parse("6030B706-0377-45D9-9A9D-DEC4A39F1FF2");
        public static readonly Guid WaitValidationDian = Guid.Parse("D425F091-F49D-4854-9AA4-5AA4639B051D");
    }

    public static class CatalogCode_InvoiceCreditLine
    {
        public static readonly Guid Progress = Guid.Parse("");
    }

    public static class CatalogCode_InvoiceMoneyType
    {
        public static readonly Guid COP = Guid.Parse("09B40602-1636-44C5-B8D4-58C38F5D736B");
        public static readonly Guid USD = Guid.Parse("DA951D43-BF14-43E0-B957-E514FE2A0C97");
    }

    public static class CatalogCode_DocumentType
    {
        public static readonly Guid Invoice = Guid.Parse("54D87901-44FD-45BA-B9C8-B0FD20D44F13");
        public static readonly Guid Endorsement = Guid.Parse("5115F1FE-6203-48B0-8961-08A624AA6F09");
        public static readonly Guid MoneyTransferInstruction = Guid.Parse("9DF63A71-1F6D-4590-8A9C-5CEC623E9AD9");
        public static readonly Guid BeneficiaryDocumentOrRut = Guid.Parse("C88DA711-8A23-496B-967E-95A78B521698");
        public static readonly Guid BeneficiaryBankCertificate = Guid.Parse("3F8DB0E8-9248-4232-9E27-86352407558D");

        //public static readonly Guid Appendix = Guid.Parse("314E43F0-11AC-4C0A-8D78-9347CD570A55");
        public static readonly Guid CommercialOffer = Guid.Parse("51D0B63E-334D-413D-8FDD-B07C273A8629");

        public static readonly Guid FactoringIn = Guid.Parse("CB71DE13-92C5-4BB4-8EB9-3FCD5914DA79");
        public static readonly Guid FactoringOut = Guid.Parse("2EF3B5AF-6EC3-4A59-8C3A-BEF1D16E7BAC");
        public static readonly Guid TransferSupportBuyer = Guid.Parse("25A9F6C9-5843-4200-8F9D-8F34DEF72162");
        public static readonly Guid CommercialOfferBuyer = Guid.Parse("74485909-D799-437B-ADA7-03BCA8C6F524");
        public static readonly Guid MoneyTransferInstructionBuyer = Guid.Parse("14478874-EDA0-4E39-AFCD-39FD0448B8DE");
        public static readonly Guid PurchaseCertificate = Guid.Parse("F20143D6-6B9D-4EE0-A755-D0304D32AF24");
        public static readonly Guid EndorsementNotification = Guid.Parse("8EBA4B27-FF46-4C0B-8563-B780259F1A7B");
        public static readonly Guid MoneyTransferInstructionExcel = Guid.Parse("78F20110-FAEF-4D4D-A04B-C137584258F1");
        public static readonly Guid DocumentsUploadByUserOnResume = Guid.Parse("3EC27F4B-22D2-42AD-B2CE-A0B17E4DB94C");
        public static readonly Guid LinkingFormat = Guid.Parse("5F8F71C4-F2E4-414B-9060-BCAF2A25AF63");
        public static readonly Guid BrokerContract = Guid.Parse("46F4DDE4-83C1-4E2E-BD7C-9FE81AB25FEF");
        public static readonly Guid DianRegistrationAuthorization = Guid.Parse("A4DA6417-FA58-447D-B16F-CA8D3766D436");
    }

    #endregion Invoice

    #region ServiceBus

    public static class CatalogCode_ServiceBus
    {
        public static readonly Guid ConexionString = Guid.Parse("F419F398-1425-46AB-8976-E7D2216E2190");
        public static readonly Guid QueueCUFES = Guid.Parse("D023D490-12BC-46EE-9E1E-D100461B3DB8");
    }

    #endregion ServiceBus

    #region Storage

    public static class CatalogCode_Storage
    {
        public static readonly Guid ConexionString = Guid.Parse("35FDB1D2-D9FA-4B3D-82C3-1D49D0B4FF41");
        public static readonly Guid ContainerName = Guid.Parse("0B830D8A-6BA0-4EA1-9EED-D594C1372B90");
    }

    #endregion Storage

    #region Beneficiary

    public static class CatalogCode_IdType
    {
        public static readonly Guid Cedula = Guid.Parse("EC42B2F5-3ECC-4B2B-A0C0-C4B90F87797C");
        public static readonly Guid Pasaporte = Guid.Parse("DD633EBD-AF13-472E-BC8C-381E21C2A1F9");
        public static readonly Guid NIT = Guid.Parse("23A7CE80-A963-47DD-8737-EB3E497F3D5D");
        public static readonly Guid CedulaExtrangeria = Guid.Parse("FBB3B4EA-3D57-417A-83C9-22243E775FE5");
    }

    public static class CatalogCode_AccountType
    {
        public static readonly Guid Ahorro = Guid.Parse("86484AE3-CCE9-4328-A7F6-CC0086AEA9F2");
        public static readonly Guid Corriente = Guid.Parse("0EA92A60-6059-4849-9A22-25AEA977151B");
    }

    public static class CatalogCode_PersonType
    {
        public static readonly Guid Natural = Guid.Parse("5C2F2B7E-EDDA-4E5F-B875-08072E206A83");
        public static readonly Guid Juridica = Guid.Parse("C77E3528-E582-435D-A527-700C5B57F9CA");
    }

    #endregion Beneficiary

    public static class CatalogCode_DatayourInvoice
    {
        public static readonly Guid RepresentativeLegalName = Guid.Parse("0C37EF70-32A4-4C24-BB7C-7D0B39EC39C4");
        public static readonly Guid RepresentativeLegalDocument = Guid.Parse("E6EA5DD0-172E-4F0C-8B2A-FB20FF31F9D9");
        public static readonly Guid RepresentativeLegalCity = Guid.Parse("DFBC9739-4494-4473-BBA5-C309B1C01565");
        public static readonly Guid MaxBeneficiaries = Guid.Parse("AAE62066-CBE3-4622-A81B-026936F40939");
        public static readonly Guid EmailAdmin = Guid.Parse("DEF6CC01-44B9-4CD8-8BD7-F6FAA6AA32B9");
        public static readonly Guid TimeEnableLink = Guid.Parse("5311C517-5337-4B76-8B3C-E64597E257C7");
        public static readonly Guid UrlyourInvoice = Guid.Parse("BA680E05-F7E0-474D-BC1D-C6F0C12AFC16");
        public static readonly Guid DocumentType = Guid.Parse("E1C6F7D2-EAFD-490F-A71D-B2944385310E");
        public static readonly Guid TimeEnableOffers = Guid.Parse("EFA1EA9B-92BD-47DC-95C9-C17A881D770A");
        public static readonly Guid RetriesZapzing = Guid.Parse("F84291B5-7897-4516-8CB1-47E8EFA6FF11");
        public static readonly Guid Bank = Guid.Parse("19C7CD9D-246B-4141-9C63-AF5F86C8E507");
        public static readonly Guid AccountType = Guid.Parse("D6365932-56CD-4A8F-A448-A7C1F9558896");
        public static readonly Guid AccountNumber = Guid.Parse("70E94B90-59E3-492D-BCA9-58BB1D317861");
        public static readonly Guid Nit = Guid.Parse("8341B87B-21BE-42D6-A995-DCB19B60D173");
        public static readonly Guid urlVinculacionCompletedEmailToAdmin = Guid.Parse("844D934B-B67E-4206-97CA-0D1B9641A26B");

    }

    public static class CatalogCode_Templates
    {
        public static Guid PfdCommercialOfferPn = Guid.Parse("A6038636-9B6C-47A2-91AC-A659FA1A8DC3");
        public static Guid PfdCommercialOfferPj = Guid.Parse("4E77B1EB-F535-49DF-8CAB-66C8B3348A42");
        public static Guid PdfMoneyTransferInstructionPn = Guid.Parse("369FFCA6-3B2F-44B1-88F6-296C163FF8A8");
        public static Guid PdfMoneyTransferInstructionPj = Guid.Parse("37E91CBD-3912-4125-9D49-16B2376BCB66");
        public static Guid PdfAppendix1 = Guid.Parse("72FC5C76-AD35-41A0-B19B-043F2B91E082");
        public static Guid EmailToAdmin = Guid.Parse("264DF4BE-717F-4E52-9257-97A0435ED0E2");
        public static Guid EmailToSeller = Guid.Parse("F0DF3B87-DF5D-4827-B91B-EE63F4D63176");
        public static Guid EmailToAdminRejectedByTime = Guid.Parse("3996D848-C16A-4665-9137-3EBC734A09C4");
        public static Guid PdfEndorsementNotificationPn = Guid.Parse("EFCEC995-008F-466C-A331-FDF64CF151B7");
        public static Guid PdfEndorsementNotificationPj = Guid.Parse("5615F36A-9CC3-478B-BB44-93B3C2782AA7");
        public static Guid PdfEndorsementPj = Guid.Parse("610D3AF4-F9F2-4F1D-A00D-BBA3E7BC3702");
        public static Guid PdfEndorsementPn = Guid.Parse("F67A2E9D-391B-4FF6-B4F0-22098B791151");
        public static Guid EmailBuyerRejectToAdmin = Guid.Parse("F8EC76A0-6E86-41A4-A2A2-F793BB836577");
        public static Guid EmailBuyerApproveToAdmin = Guid.Parse("CDC5AFD3-74C9-4BE6-AF1E-CFA2881C2796");
        public static Guid EmailBuyerNewOffer = Guid.Parse("6649FBD5-14F5-40E6-9559-B41EB1C9C81F");
        public static Guid PfdCommercialOfferAcceptedPj = Guid.Parse("89B80A4C-2156-4041-B5C0-98371C4E87D3");
        public static Guid PfdCommercialOfferAcceptedPn = Guid.Parse("B635EB38-1F3E-4691-AAF3-06ECBCEF6F56");
        public static Guid PdfPurchaseCertificate = Guid.Parse("814854D4-6915-4BFA-9572-4DB6143DA79B");
        public static Guid PdfMoneyTransferInstructionBuyerPj = Guid.Parse("08D3F626-0251-404F-B4DE-3AF0EEFB0604");
        public static Guid PdfMoneyTransferInstructionBuyerPn = Guid.Parse("D2A9BCB9-7E1E-4BC6-9CDE-A9335366B60E");
        public static Guid EmailToSellerAdminPurchased = Guid.Parse("9EB597DD-44C8-4C96-9776-0BAFFE827B90");
        public static Guid EmailToAdminProcessFile = Guid.Parse("1979AB4C-783A-411C-A1FB-24896510605F");
        public static Guid EmailToAdminPreRegister = Guid.Parse("17CCF32F-3B0F-4ADD-818F-385D837974F2");
        public static Guid EmailToUserPreRegister = Guid.Parse("5E9F2BB4-6284-4F92-B96E-01A4A67B9818");
        public static Guid EmailToRejectPreRegister = Guid.Parse("F02CC4CC-6B05-4EFE-A0A1-5C9922080934");
        public static Guid EmailApproveAccount = Guid.Parse("2BA614E4-D03B-432B-93A5-F2AF2D58A1AA");
        public static Guid EmailToUserPostProcessDian = Guid.Parse("955F89BF-C79B-4C3C-BBF1-6A54C1340290");
        public static Guid EmailToAdminLinkingCompletedPN = Guid.Parse("4563F4AC-BAD0-4D21-974B-3003AC76507E");
        public static Guid EmailToAdminLinkingCompletedPJ = Guid.Parse("2BC227B3-7630-482C-8C05-5B806B937DCB");
        public static Guid DianRegistrationAuthorizationDocument = Guid.Parse("4F5D775C-DD63-4508-90A6-83632640C0E6");
        public static Guid BrokerageContractDocumentPN = Guid.Parse("A63AD82C-5ED5-4963-BCD8-CA508C7D60E8");
        public static Guid BrokerageContractDocumentPJ = Guid.Parse("62A47A0D-EE4B-46E1-8887-7462969F99DF");
        public static Guid MailRequestAdicionalDocument = Guid.Parse("47E061A1-7381-40D8-BE2A-C57244D2C51B");
        public static Guid MailNotificationLinkApproval = Guid.Parse("41F3D66D-D624-4A72-8E29-1ECF6A2C404D");
    }

    public static class CatalogCode_FTP_Factoring
    {
        public static readonly Guid FtpFactoringHost = Guid.Parse("42946BC4-51F8-4225-A2D1-4C6771B3169B");
        public static readonly Guid FtpFactoringPath = Guid.Parse("F4260EEB-6080-4FFB-B06C-6F2CCE24E0DF");
        public static readonly Guid FtpFactoringUser = Guid.Parse("31FD3327-F7E1-47D9-B35E-DD057C8CAFFD");
        public static readonly Guid FtpFactoringPasProjrd = Guid.Parse("A300F043-4EBE-4E13-9F8E-5189CAC2FE49");
        public static readonly Guid FtpFactoringPathOut = Guid.Parse("B0930987-400E-4CB9-B934-7E747136C3E4");
        public static readonly Guid FtpFactoringPort = Guid.Parse("EA7CDD38-FC16-4933-94C7-2DDE409BBCAE");
    }

    public static class CatalogCode_InvoiceDispersionStatus
    {
        public static readonly Guid Rejected = Guid.Parse("639C38AC-7CC4-4850-98AF-293620E44CFC");
        public static readonly Guid PendingPurchase = Guid.Parse("5233F4AC-5F5E-4F28-B0B7-1C3C83A7193D");
        public static readonly Guid Purchased = Guid.Parse("EC587836-2949-426F-962D-BCEE74A51EE5");
        public static readonly Guid CanceledModified = Guid.Parse("3432B4C6-AB47-44FD-9E0F-E405E3D93F7D");
        public static readonly Guid Canceled = Guid.Parse("1988D651-22CF-48AB-936A-E156055509A2");
        public static readonly Guid PendingTransfer = Guid.Parse("E8592054-3A1D-4024-A3E5-F56222964973");
        public static readonly Guid Defeated = Guid.Parse("8E0168F1-525C-46C6-A907-EE29DF74D88C");
    }

    public static class CatalogCode_UserRole
    {
        public static readonly Guid Administrator = Guid.Parse("0326A0F0-9DDC-412A-B11A-B9D862B51E40");
        public static readonly Guid Buyer = Guid.Parse("72CA7D80-DE02-49A6-AB26-588CFE2A8F5B");
        public static readonly Guid Seller = Guid.Parse("BFFBA02F-B413-4F18-9DF6-79715B541E84");
    }

    public static class CatalogCode_UrlGeneral
    {
        public static readonly Guid UrlLogueoPlanformyourInvoice = Guid.Parse("63729A16-319C-45D5-A81E-09248E00287B");
    }

    public static class CatalogCode_TypeNotification
    {
        public static readonly Guid EmailSummaryOffer = Guid.Parse("11797D47-8131-475B-A27A-B3873CF6355D");
    }

    public static class CatalogCode_StatusPreRegister
    {
        public static readonly Guid Rejected = Guid.Parse("9A0E187D-F3DE-4948-8630-2CC6577E6C3F");
        public static readonly Guid Approved = Guid.Parse("0306CE79-BF38-43F1-A923-30815EADFE79");
        public static readonly Guid Pending = Guid.Parse("77B5B2BD-5041-4313-95A3-C33A175757D9");
    }

    public static class CatalogCode_FormStatus
    {
        public static readonly Guid InProgress = Guid.Parse("3449291E-30B1-47A1-86F6-0F15529D3030");
        public static readonly Guid WithoutStarting = Guid.Parse("5BE7FD13-BA2C-4734-8C87-1C20DE3B312C");
        public static readonly Guid Completed = Guid.Parse("9A2A47F3-27D8-4A54-9B7D-75B63EA3E4A8");
    }

    public static class CatalogCode_StatusVinculation
    {
        public static readonly Guid Rejected = Guid.Parse("9A0E187D-F3DE-4948-8630-2CC6577E6C3F");
        public static readonly Guid Approved = Guid.Parse("0306CE79-BF38-43F1-A923-30815EADFE79");
        public static readonly Guid Pending = Guid.Parse("77B5B2BD-5041-4313-95A3-C33A175757D9");
        public static readonly Guid PendingApproval = Guid.Parse("9C3CBDAF-C081-4CB8-BEE9-0F348C16221D");
    }

    public static class CatalogCode_LegalFinancial
    {
        public static readonly Guid OperationsForeignCurrencyQuestionId = Guid.Parse("0AA32301-9885-4503-AB2E-638765352DE2");
        public static readonly Guid AccountsForeignCurrencyQuestionId = Guid.Parse("D0FBF907-F63F-4A78-952C-E9D5180A6028");
    }
}