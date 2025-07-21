///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateBank;
using yourInvoice.Link.Application.LinkingProcess.CreateFinancial;
using yourInvoice.Link.Application.LinkingProcess.GetBank;
using yourInvoice.Link.Application.LinkingProcess.GetFinancial;
using yourInvoice.Link.Application.LinkingProcess.UpdateBank;
using yourInvoice.Link.Application.LinkingProcess.UpdateFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;


namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class Bank_FinancialData
    {
        public static CreateBankCommand CreateBankCommandNoValid => new CreateBankCommand(
            new Bank
            {
                Id_GeneralInformation = Guid.Empty,
                BankProduct = "Ahorros",
                BankReference = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                PhoneNumber = "123456789012345",

            });

        public static CreateBankCommand CreateBankCommand => new CreateBankCommand(
             new Bank
             {

                 Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 BankProduct = "Ahorros",
                 BankReference = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                 PhoneNumber = "123456789012345",


             });

        public static UpdateBankCommand UpdateBankCommand => new UpdateBankCommand(
             new UpdateBank
             {
                 Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a9"),
                 Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 BankProduct = "Ahorros",
                 BankReference = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                 PhoneNumber = "123456789012345",
             }
            );

        public static GetBankResponse GetBankResponse => new GetBankResponse()
        {

            Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a0"),
            Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
            BankProduct = "Ahorros",
            BankReference = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
            City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
            Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
            DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
            PhoneNumber = "123456789012345",
            StatusId= Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),


        };

        public static CreateFinancialCommand CreateFinancialCommandNoValid => new CreateFinancialCommand(
    new Financial
    {
        Id_GeneralInformation = Guid.Empty,
        TotalAssets = 20000000,
        TotalLiabilities = 10000000,
        TotalWorth = 10000000,
        Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
        MonthlyIncome = 3500000,
        MonthlyExpenditures = 1500000,
        OtherIncome=465000,
        DescribeOriginIncome="Prueba",


    });

        public static CreateFinancialCommand CreateFinancialCommand => new CreateFinancialCommand(
             new Financial
             {

                 Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 TotalAssets = 20000000,
                 TotalLiabilities = 10000000,
                 TotalWorth = 10000000,
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 MonthlyIncome = 3500000,
                 MonthlyExpenditures = 1500000,
                 OtherIncome = 465000,
                 DescribeOriginIncome = "Prueba",

             });

        public static UpdateFinancialCommand UpdateFinancialCommand => new UpdateFinancialCommand(
             new UpdateFinancial
             {
                 Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a9"),
                 Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 TotalAssets = 20000000,
                 TotalLiabilities = 10000000,
                 TotalWorth = 10000000,
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 MonthlyIncome = 3500000,
                 MonthlyExpenditures = 1500000,
                 OtherIncome = 465000,
                 DescribeOriginIncome = "Prueba",
             }
            );

        public static GetFinancialResponse GetFinancialResponse => new GetFinancialResponse()
        {

            Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a9"),
            Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
            TotalAssets = "20000000",
            TotalLiabilities = "10000000",
            TotalWorth = "10000000",
            Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
            MonthlyIncome = "3500000",
            MonthlyExpenditures = "1500000",
            OtherIncome = "465000",
            DescribeOriginIncome = "Prueba",
            StatusId = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),


        };

        public static GetFinancialQuery GetFinancialQueryEmpy =>
            new GetFinancialQuery(idGeneralInformation: Guid.Empty);

        public static GetBankQuery GetBankQueryEmpy =>
            new GetBankQuery(idGeneralInformation: Guid.Empty);

        public static GetBankQuery GetBankQueryValid => new GetBankQuery(idGeneralInformation: Guid.NewGuid());
        public static GetBankQuery GetBankQuery => new GetBankQuery(idGeneralInformation: Guid.Empty);

        public static GetFinancialQuery GetFinancialQueryValid => new GetFinancialQuery(idGeneralInformation: Guid.NewGuid());
        public static GetFinancialQuery GetFinancialQuery => new GetFinancialQuery(idGeneralInformation: Guid.Empty);

    }
}