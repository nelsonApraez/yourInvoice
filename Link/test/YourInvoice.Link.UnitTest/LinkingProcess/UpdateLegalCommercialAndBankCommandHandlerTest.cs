using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateLegalCommercialAndBankCommandHandlerTest
    {
        private readonly Mock<ILegalCommercialAndBankReferenceRepository> _mockRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly UpdateLegalCommercialAndBankCommandHandler _handler;

        public UpdateLegalCommercialAndBankCommandHandlerTest()
        {
            _mockRepository = new Mock<ILegalCommercialAndBankReferenceRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _handler = new UpdateLegalCommercialAndBankCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleUpdateLegalCommercialAndBankReference_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            UpdateLegalCommercialAndBankCommand command = new(new UpdateCommercialAndBank() { Id_LegalGeneralInformation = Guid.NewGuid() });
            _mockRepository.Setup(x => x.UpdateLegalCommercialAndBankReferenceAsync(It.IsAny<LegalCommercialAndBankReference>())).ReturnsAsync(true);

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
