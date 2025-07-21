using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Application.LinkingProcess.SignDocs;
using yourInvoice.Common.Entities;
using FluentAssertions;
using ErrorOr;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Domain.Accounts;
using MediatR;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class SignDocsLinkCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockIAccountRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<ILinkStatusRepository> _mockLinkStatusRepository;
        private readonly Mock<IMediator> _mockIMediator; 
        private SignDocsLinkCommandHandler _handler;

        public SignDocsLinkCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockIAccountRepository = new Mock<IAccountRepository>();
            _mockLinkStatusRepository = new Mock<ILinkStatusRepository>();
            _mockStorage = new Mock<IStorage>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _mockZapsign = new Mock<IZapsign>();
            _mockIMediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task SignDocsLink_Signed_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid relatedId = Guid.NewGuid();
            
            Document LinkingFormat = new(Guid.NewGuid(), relatedId, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url");

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(relatedId)).ReturnsAsync(new List<Document>() { LinkingFormat });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { token = "token", status = "pending", signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url", status = "pending" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _mockLinkStatusRepository.Setup(x => x.GetLinkStatusAsync(relatedId)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingSignature});
            _mockIAccountRepository.Setup(x => x.GetAccountIdAsync(relatedId)).ReturnsAsync(new  Domain.Accounts.Queries.AccountResponse { Name= "nn", SecondName = "nn", LastName = "nn", SecondLastName = "nn", Email = "123@hh.com"});
            SignDocsLinkCommand command = new(relatedId);

            _handler = new SignDocsLinkCommandHandler(_mockUnitOfWork.Object,_mockStorage.Object, 
                _mockZapsign.Object,_mockDocumentRepository.Object,_mockCatalogBusiness.Object,_mockLinkStatusRepository.Object, _mockIAccountRepository.Object, _mockIMediator.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.Equal("token", result.Value.Token);
        }

        [Fact]
        public async Task SignDocsLink_NoResponseZapsign_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid relatedId = Guid.NewGuid();

            Document LinkingFormat = new(Guid.NewGuid(), relatedId, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url");

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(relatedId)).ReturnsAsync(new List<Document>() { LinkingFormat });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _mockLinkStatusRepository.Setup(x => x.GetLinkStatusAsync(relatedId)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingSignature });
            _mockIAccountRepository.Setup(x => x.GetAccountIdAsync(relatedId)).ReturnsAsync(new Domain.Accounts.Queries.AccountResponse { Name = "nn", SecondName = "nn", LastName = "nn", SecondLastName = "nn", Email = "123@hh.com" });
            SignDocsLinkCommand command = new(relatedId);

            _handler = new SignDocsLinkCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object,
                _mockZapsign.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mockLinkStatusRepository.Object, _mockIAccountRepository.Object, _mockIMediator.Object);
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
        public async Task SignDocsLink_PendingApproval_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid relatedId = Guid.NewGuid();

            Document LinkingFormat = new(Guid.NewGuid(), relatedId, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url");


            _mockLinkStatusRepository.Setup(x => x.GetLinkStatusAsync(relatedId)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingApproval });

            SignDocsLinkCommand command = new(relatedId);

            _handler = new SignDocsLinkCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object,
                _mockZapsign.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mockLinkStatusRepository.Object, _mockIAccountRepository.Object, _mockIMediator.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.PendigApproval), result.FirstError.Description);
        }
    }
}
