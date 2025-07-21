using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetLegalFinancialQueryHandlerTest
    {
        private readonly Mock<ILegalFinancialInformationRepository> _mockRepository;
        private readonly GetLegalFinancialQueryHandler _handler;

        public GetLegalFinancialQueryHandlerTest()
        {
            _mockRepository = new Mock<ILegalFinancialInformationRepository>();
            _handler = new GetLegalFinancialQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandlerGetLegalFinancial_Sucess()
        {
            Guid id = Guid.NewGuid();

            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetLegalFinancialQuery command = new(id);
            _mockRepository.Setup(x => x.GetLegalFinancialInformationAsync(id)).ReturnsAsync(new GetLegalFinancialResponse());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }
    }
}
