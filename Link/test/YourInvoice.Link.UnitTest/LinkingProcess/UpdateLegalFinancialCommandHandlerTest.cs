using Moq;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalFinancial;
using FluentAssertions;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateLegalFinancialCommandHandlerTest
    {
        private readonly Mock<ILegalFinancialInformationRepository> _mockRepository;
        private readonly UpdateLegalFinancialCommandHandler _handler;

        public UpdateLegalFinancialCommandHandlerTest()
        {
            _mockRepository = new Mock<ILegalFinancialInformationRepository>();
            _handler = new UpdateLegalFinancialCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleUpdateLegalFinancial_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            UpdateLegalFinancialCommand command = new(new UpdateLegalFinancial() { Id_LegalGeneralInformation = Guid.NewGuid(), Id = Guid.NewGuid(), OperationsTypes = new List<Guid?>() });
            _mockRepository.Setup(x => x.UpdateLegalFinancialAsync(It.IsAny<LegalFinancialInformation>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

    }
}
