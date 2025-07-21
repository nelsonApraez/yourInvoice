using ErrorOr;
using Moq;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using FluentAssertions;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateLegalCommercialAndBankReferenceCommandHandlerTest
    {
        private readonly Mock<ILegalCommercialAndBankReferenceRepository> _mockRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly CreateLegalCommercialAndBankReferenceCommandHandler _handler;

        public CreateLegalCommercialAndBankReferenceCommandHandlerTest()
        {
            _mockRepository = new Mock<ILegalCommercialAndBankReferenceRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _handler = new CreateLegalCommercialAndBankReferenceCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task HandleCreateLegalCommercialAndBankReference_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateLegalCommercialAndBankReferenceCommand command = new(new References() { Id_LegalGeneralInformation = Guid.NewGuid() });
            _mockRepository.Setup(x => x.CreateLegalCommercialAndBankReferenceAsync(It.IsAny<LegalCommercialAndBankReference>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleCreateLegalCommercialAndBankReference_WhenExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateLegalCommercialAndBankReferenceCommand command = new(new References() { Id_LegalGeneralInformation = Guid.NewGuid() });
            _mockRepository.Setup(x => x.CreateLegalCommercialAndBankReferenceAsync(It.IsAny<LegalCommercialAndBankReference>())).ReturnsAsync(true);
            _mockRepository.Setup(x => x.ExistsLegalCommercialAndBankReferenceAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.MessageExistsInformation, "referencias comerciales y bancarias"), result.FirstError.Description);
        }
    }
}
