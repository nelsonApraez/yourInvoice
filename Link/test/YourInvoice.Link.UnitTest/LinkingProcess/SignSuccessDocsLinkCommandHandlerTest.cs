using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Link.Application.LinkingProcess.SignSuccessDocs;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class SignSuccessDocsLinkCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockIAccountRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<ILinkStatusRepository> _mockLinkStatusRepository;
        private readonly Mock<IMediator> _mediator;
        private SignSuccessDocsLinkCommandHandler _handler;

        public SignSuccessDocsLinkCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockIAccountRepository = new Mock<IAccountRepository>();
            _mockLinkStatusRepository = new Mock<ILinkStatusRepository>();
            _mockStorage = new Mock<IStorage>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _mockZapsign = new Mock<IZapsign>();
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task HandleSignSuccessDocs_Sucess_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid relatedId = Guid.NewGuid();

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            Document LinkingFormat = new(Guid.NewGuid(), relatedId, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url", "1 mb", "token");

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(relatedId)).ReturnsAsync(new List<Document>() {  LinkingFormat });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());

            _mockLinkStatusRepository.Setup(x => x.GetLinkStatusAsync(relatedId)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingSignature });
            _mockIAccountRepository.Setup(x => x.GetAccountIdAsync(relatedId)).ReturnsAsync(new Domain.Accounts.Queries.AccountResponse { Name = "nn", SecondName = "nn", LastName = "nn", SecondLastName = "nn", Email = "123@hh.com" });

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() { new ZapsignFileDetailExtraDocResponse() { name = "Instruccion de Giro Inversionista.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                            new ZapsignFileDetailExtraDocResponse() { name = "Certificacion de Compra.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "Formato de vinculación.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsLinkCommand command = new(relatedId);


            _handler = new SignSuccessDocsLinkCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockLinkStatusRepository.Object,
                _mockZapsign.Object,  _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mediator.Object, _mockIAccountRepository.Object);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleSignSuccessDocs_NoToken_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid relatedId = Guid.NewGuid();

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            Document LinkingFormat = new(Guid.NewGuid(), relatedId, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url");


            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(relatedId)).ReturnsAsync(new List<Document>() { LinkingFormat });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());

            _mockLinkStatusRepository.Setup(x => x.GetLinkStatusAsync(relatedId)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingSignature });
            _mockIAccountRepository.Setup(x => x.GetAccountIdAsync(relatedId)).ReturnsAsync(new Domain.Accounts.Queries.AccountResponse { Name = "nn", SecondName = "nn", LastName = "nn", SecondLastName = "nn", Email = "123@hh.com" });

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() { new ZapsignFileDetailExtraDocResponse() { name = "Instruccion de Giro Inversionista.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                            new ZapsignFileDetailExtraDocResponse() { name = "Certificacion de Compra.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "Aceptacion Oferta Mercantil e Instruccion de giro.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsLinkCommand command = new(relatedId);
            _handler = new SignSuccessDocsLinkCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockLinkStatusRepository.Object,
                _mockZapsign.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mediator.Object, _mockIAccountRepository.Object);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.ZapsignNoToken), result.FirstError.Description);
        }
    }
}
