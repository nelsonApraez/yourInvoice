///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Buyer.GenerateDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain.Users.Queries;

using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Buyer.GenerateDocs
{
    public class GenerateDocsBuyerCommandHandlerTest
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
        private readonly Mock<IInvoiceDispersionRepository> _invoiceDispersionRepository;
        private readonly Mock<ISystem> _system;
        private GenerateDocsBuyerCommandHandler _handler;

        public GenerateDocsBuyerCommandHandlerTest()
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
            _invoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _system = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandleGenerateDocsBuyer_Generated_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            int offerNumber = 0;
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid buyerId = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            offer.CreatedOn = new DateTime();
            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(new InvoiceDispersion(
            Guid.NewGuid(), offerNumber, buyerId, It.IsAny<Guid>(), payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
            , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847", "Cali",
                "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockPayerRepository.Setup(x => x.GetByIdAsync(payerid)).ReturnsAsync(new Payer() { Name = "", Nit = "" });
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>());
            _mockInvoiceRepository.Setup(x => x.Add(It.IsAny<Invoice>())).Returns(new Invoice());
            _mockInvoiceRepository.Setup(x => x.ListToAppendix1Document(offerid)).Returns(new List<yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListAppendix1DocumentResponse>());
            _invoiceDispersionRepository.Setup(x => x.ListToPurchaseCertificateDocument(It.IsAny<Guid>(), offerNumber)).ReturnsAsync(new List<OfferListResponse>());
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _system.Setup(x => x.User).Returns(UserData.GetUser);
            GenerateDocsBuyerCommand command = new(offerNumber);
            _handler = new GenerateDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
             _mockMoneyTransferRepository.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockInvoiceRepository.Object, _mockCatalogBusiness.Object, _mockPayerRepository.Object,
             _invoiceDispersionRepository.Object, _system.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleGenerateDocsBuyer_IsSigned_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            int offerNumber = 1;
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid buyerId = UserData.GetUser.Id;
            Guid sellerid = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, sellerid, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(new InvoiceDispersion(
                Guid.NewGuid(), offerNumber, buyerId, sellerid, payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
                , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { new Document(It.IsAny<Guid>(),
                offerid, buyerId,"", CatalogCode_DocumentType.CommercialOfferBuyer,true,"") });
            _system.Setup(x => x.User).Returns(UserData.GetUser);

            GenerateDocsBuyerCommand command = new(offerNumber);

            _handler = new GenerateDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
         _mockMoneyTransferRepository.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockInvoiceRepository.Object, _mockCatalogBusiness.Object, _mockPayerRepository.Object,
         _invoiceDispersionRepository.Object, _system.Object);

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
        public async Task HandleGenerateDocsBuyer_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            int offerid = 1;
            Guid buyerId = Guid.NewGuid();

            GenerateDocsBuyerCommand command = new(offerid);

            _system.Setup(x => x.User).Returns(UserData.GetUser);

            _handler = new GenerateDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                        _mockMoneyTransferRepository.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockInvoiceRepository.Object, _mockCatalogBusiness.Object, _mockPayerRepository.Object,
                        _invoiceDispersionRepository.Object, _system.Object);

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