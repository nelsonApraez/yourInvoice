///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Business.CatalogModule
{
    public static class CatalogCodeLink_ExposureQuestion
    {
        public static readonly Guid PublicRecognition = Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465");
        public static readonly Guid ManagesPublicResources = Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1");
        public static readonly Guid TaxObligations = Guid.Parse("A582D141-4AC3-4729-93D9-70F69E2A0257");
        public static readonly Guid PubliclyExposedPersonLink = Guid.Parse("8693AC3D-DC03-4963-9CAF-4EC30854B27D");
    }

    public static class CatalogCodeLink_SAGRILAFTQuestion
    {
        public static readonly Guid Q1 = Guid.Parse("C2FD3F9B-9090-412A-8BD7-764CDCECA370");
        public static readonly Guid Q2 = Guid.Parse("45C7A971-2176-4A7E-99D6-4B97768402AF");
        public static readonly Guid Q3 = Guid.Parse("75722C29-7560-4A15-96AA-C72E589EFA3B");
        public static readonly Guid Q4 = Guid.Parse("11CEEE51-02F9-4F03-A963-7DC877DD8F33");
        public static readonly Guid Q5 = Guid.Parse("75CA1F89-B4DC-4B2F-8124-4DF345C6E7A2");
        public static readonly Guid Q6 = Guid.Parse("2B6DB088-F4D6-44ED-AFDD-8405BDDAA51D");
        public static readonly Guid Q7 = Guid.Parse("9F72A1B9-A178-482A-878F-7A39EB116291");
    }

    public static class CatalogCodeLink_StatusForm
    {
        public static readonly Guid InProgress = Guid.Parse("3449291E-30B1-47A1-86F6-0F15529D3030");
        public static readonly Guid WithoutStarting = Guid.Parse("5BE7FD13-BA2C-4734-8C87-1C20DE3B312C");
        public static readonly Guid Complete = Guid.Parse("9A2A47F3-27D8-4A54-9B7D-75B63EA3E4A8");
    }

    public static class CatalogCodeLink_LinkStatus
    {
        public static readonly Guid InProcess = Guid.Parse("441A6265-534A-4A6E-A31E-2728632252AB");
        public static readonly Guid PendingSignature = Guid.Parse("32AF6D04-545D-440E-8F1C-0BA13427D84C");
        public static readonly Guid ValidationRejected = Guid.Parse("645711D2-3C70-4651-B1AF-58F48C557506");
        public static readonly Guid SignatureUnsuccessful = Guid.Parse("D4D29178-7D2F-4A01-9EA6-CE2BD454AF36");
        public static readonly Guid PendingApproval = Guid.Parse("932164E7-7856-4EF2-A6B0-D14AF4AF894D");
        public static readonly Guid Linked = Guid.Parse("EC056820-6285-444D-B27C-F41CAAE43F6F");
        public static readonly Guid Rejected = Guid.Parse("810D6ACD-FA92-4593-B092-9E344F6AEDFE");
    }
}