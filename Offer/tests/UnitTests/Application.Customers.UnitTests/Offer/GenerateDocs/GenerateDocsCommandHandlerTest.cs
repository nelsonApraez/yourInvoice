///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Offer.Generate;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.GenerateDocs
{
    public class GenerateDocsCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IMoneyTransferRepository> _mockMoneyTransferRepository;
        private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<IPayerRepository> _mockPayerRepository;
        private readonly GenerateDocsCommandHandler _handler;

        public GenerateDocsCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockRepository = new Mock<IOfferRepository>();
            _mockMoneyTransferRepository = new Mock<IMoneyTransferRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockPayerRepository = new Mock<IPayerRepository>();
            _mockStorage = new Mock<IStorage>();

            _handler = new GenerateDocsCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockMoneyTransferRepository.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockInvoiceRepository.Object, _mockCatalogBusiness.Object, _mockPayerRepository.Object);
        }

        [Fact]
        public async Task HandleGenerateDocs_Generated_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);

            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847", "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockPayerRepository.Setup(x => x.GetByIdAsync(payerid)).ReturnsAsync(new yourInvoice.Offer.Domain.Payers.Payer() { Name = "", Nit = "" });
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>());
            _mockInvoiceRepository.Setup(x => x.Add(It.IsAny<yourInvoice.Offer.Domain.Invoices.Invoice>())).Returns(new yourInvoice.Offer.Domain.Invoices.Invoice());
            _mockInvoiceRepository.Setup(x => x.ListToAppendix1Document(offerid)).Returns(new List<yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListAppendix1DocumentResponse>());
            _mockMoneyTransferRepository.Setup(x => x.ListToMoneyTransferDocumentAsync(offerid)).ReturnsAsync(new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentResponse()
            {
                TableContent = new List<yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse>()
            { new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse() { AccountNumber="123", Bank="banco", AccountType="tipo", DocumentNumber="1", DocumentType="tipo", Name="nombre", Total="123" },
            new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse() { AccountNumber="123", Bank="banco", AccountType="tipo", DocumentNumber="1", DocumentType="tipo", Name="nombre", Total="123" },
            new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse() { AccountNumber="123", Bank="banco", AccountType="tipo", DocumentNumber="1", DocumentType="tipo", Name="nombre", Total="123" },
            new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse() { AccountNumber="123", Bank="banco", AccountType="tipo", DocumentNumber="1", DocumentType="tipo", Name="nombre", Total="123" },
            new yourInvoice.Offer.Domain.MoneyTransfers.Queries.MoneyTransferDocumentTableContentResponse() { AccountNumber="123", Bank="banco", AccountType="tipo", DocumentNumber="1", DocumentType="tipo", Name="nombre", Total="123" }}
            });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            GenerateDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleGenerateDocs_IsSigned_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid sellerid = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, sellerid, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { new Document(It.IsAny<Guid>(),
                offerid, null,"", CatalogCode_DocumentType.CommercialOffer,true,"") });

            GenerateDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.DocumentIsSigned), result.FirstError.Description);
        }

        [Fact]
        public async Task HandleGenerateDocs_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();

            GenerateDocsCommand command = new(offerid);
            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }
    }
}