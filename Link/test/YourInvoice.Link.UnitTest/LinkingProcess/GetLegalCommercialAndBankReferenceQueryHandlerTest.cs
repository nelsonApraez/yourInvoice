using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetLegalCommercialAndBankReferenceQueryHandlerTest
    {
        private readonly Mock<ILegalCommercialAndBankReferenceRepository> _mockRepository;
        private readonly GetLegalCommercialAndBankReferenceQueryHandler _handler;

        public GetLegalCommercialAndBankReferenceQueryHandlerTest()
        {
            _mockRepository = new Mock<ILegalCommercialAndBankReferenceRepository>();
            _handler = new GetLegalCommercialAndBankReferenceQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandlerGetLegalCommercialAndBankReference_Sucess()
        {
            Guid id = Guid.NewGuid();

            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetLegalCommercialAndBankReferenceQuery command = new(id);
            _mockRepository.Setup(x => x.GetLegalCommercialAndBankReferenceAsync(id)).ReturnsAsync(new LegalCommercialAndBankReferenceResponse());

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
