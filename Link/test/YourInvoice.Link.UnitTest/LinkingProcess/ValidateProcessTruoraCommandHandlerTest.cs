using ErrorOr;
using FluentAssertions;
using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Truora;
using yourInvoice.Link.Application.LinkingProcess.ValidateProcessTruora;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class ValidateProcessTruoraCommandHandlerTest
    {
        private readonly Mock<ITruora> _mockITruora;
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IUnitOfWorkLink> _mockIUnitOfWorkLink;
        private ValidateProcessTruoraCommandHandler _handler;

        public ValidateProcessTruoraCommandHandlerTest()
        {
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockITruora = new Mock<ITruora>();
            _mockIUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
        }

        [Fact]
        public async Task ValidateProcessTruora_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid Id = Guid.NewGuid();

            Document LinkingFormat = new(Guid.NewGuid(), Id, "nombre", CatalogCode_DocumentType.LinkingFormat, false, "url", "1 mb", "token");

            _mockITruora.Setup(x => x.GetProcessAsync(It.IsAny<string>())).ReturnsAsync(new ProcessInfoResponse() { Status = "success" });
            _mockIDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(Id)).ReturnsAsync(new List<Document>() { LinkingFormat });

            ValidateProcessTruoraCommand command = new(It.IsAny<string>(),Id);

            _handler = new ValidateProcessTruoraCommandHandler(_mockITruora.Object, _mockIDocumentRepository.Object, _mockIUnitOfWorkLink.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task ValidateProcessTruora_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid Id = Guid.NewGuid();

            Document LinkingFormat = new(Guid.NewGuid(), Id, "nombre", CatalogCode_DocumentType.LinkingFormat, true, "url", "1 mb", "token");

            _mockITruora.Setup(x => x.GetProcessAsync(It.IsAny<string>())).ReturnsAsync(new ProcessInfoResponse() { Status = "success" });
            _mockIDocumentRepository.Setup(x => x.GetAllDocumentsByRelatedIdAsync(Id)).ReturnsAsync(new List<Document>() { LinkingFormat });

            ValidateProcessTruoraCommand command = new(It.IsAny<string>(), Id);

            _handler = new ValidateProcessTruoraCommandHandler(_mockITruora.Object, _mockIDocumentRepository.Object, _mockIUnitOfWorkLink.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.DocumentsAreSigned), result.FirstError.Description);
        }
    }
}
