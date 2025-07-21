using ErrorOr;
using FluentAssertions;
using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Truora;
using yourInvoice.Link.Application.LinkingProcess.GetUrlTruora;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetUrlTruoraCommandHandlerTest
    {
       
        private readonly Mock<ITruora> _mockITruora;        
        private readonly Mock<ILinkStatusRepository> _mockILinkStatusRepository;
        private GetUrlTruoraCommandHandler _handler;

        public GetUrlTruoraCommandHandlerTest()
        {
            _mockILinkStatusRepository = new Mock<ILinkStatusRepository>();
            _mockITruora = new Mock<ITruora>();
        }

        [Fact]
        public async Task GetUrlTruora_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid Id = Guid.NewGuid();

            _mockITruora.Setup(x => x.CreateApiKeyAsync(Id)).ReturnsAsync( new Dictionary<string, string>());
            _mockILinkStatusRepository.Setup(x => x.GetLinkStatusAsync(Id)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingSignature } );

            GetUrlTruoraCommand command = new(Id);

            _handler = new GetUrlTruoraCommandHandler(_mockITruora.Object,_mockILinkStatusRepository.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUrlTruora_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid Id = Guid.NewGuid();

            _mockITruora.Setup(x => x.CreateApiKeyAsync(Id)).ReturnsAsync(new Dictionary<string, string>());
            _mockILinkStatusRepository.Setup(x => x.GetLinkStatusAsync(Id)).ReturnsAsync(new LinkStatus() { StatusLinkId = CatalogCodeLink_LinkStatus.PendingApproval });

            GetUrlTruoraCommand command = new(Id);

            _handler = new GetUrlTruoraCommandHandler(_mockITruora.Object, _mockILinkStatusRepository.Object);
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
