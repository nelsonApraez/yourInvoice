///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.Offer.SignDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.SignDocs
{
    public class SignDocsCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly SignDocsCommandHandler _handler;

        public SignDocsCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockRepository = new Mock<IOfferRepository>();
            _mockZapsign = new Mock<IZapsign>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockStorage = new Mock<IStorage>();

            _handler = new SignDocsCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object);
        }

        [Fact]
        public async Task HandleSignDocs_Signed_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url");
            Document Endorsement = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction, CommercialOffer, Endorsement, EndorsementNotification });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { token = "token", status = "pending", signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);

            SignDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.Equal("token", result.Value.Token);
        }

        [Fact]
        public async Task HandleSignDocs_NoResponseZapsign_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url");
            Document Endorsement = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction, CommercialOffer, Endorsement, EndorsementNotification });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);

            SignDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.ZapsignNoToken), result.FirstError.Description);
        }

        [Fact]
        public async Task HandleSignDocs_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);

            SignDocsCommand command = new(offerid);

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