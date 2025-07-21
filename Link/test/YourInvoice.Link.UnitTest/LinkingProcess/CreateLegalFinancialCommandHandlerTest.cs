using ErrorOr;
using Moq;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using FluentAssertions;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateLegalFinancialCommandHandlerTest
    {
        private readonly Mock<ILegalFinancialInformationRepository> _mockRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly CreateLegalFinancialCommandHandler _handler;

        public CreateLegalFinancialCommandHandlerTest()
        {
            _mockRepository = new Mock<ILegalFinancialInformationRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _handler = new CreateLegalFinancialCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task HandleCreateLegalFinancial_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateLegalFinancialCommand command = new(new LegalFinancial() { Id_LegalGeneralInformation = Guid.NewGuid(), OperationsTypes = new List<Guid?>() });
            _mockRepository.Setup(x => x.CreateLegalFinancialAsync(It.IsAny<LegalFinancialInformation>())).ReturnsAsync(true);            

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleCreateLegalFinancial_WhenExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateLegalFinancialCommand command = new(new LegalFinancial() { Id_LegalGeneralInformation = Guid.NewGuid() });
            _mockRepository.Setup(x => x.CreateLegalFinancialAsync(It.IsAny<LegalFinancialInformation>())).ReturnsAsync(true);
            _mockRepository.Setup(x => x.ExistsLegalFinancialAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.MessageExistsInformation, "información Financiera"), result.FirstError.Description);
        }
    }
}
