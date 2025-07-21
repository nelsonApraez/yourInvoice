///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Beneficiary.List;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.MoneyTransfers.Queries;

namespace Application.Customer.UnitTest.Beneficiary
{
    public class ListBeneficiariesQueryHandlerTest
    {
        private readonly Mock<IMoneyTransferRepository> _mockRepository;
        private readonly ListBeneficiariesQueryHandler _handler;

        public ListBeneficiariesQueryHandlerTest()
        {
            _mockRepository = new Mock<IMoneyTransferRepository>();
            _handler = new ListBeneficiariesQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleListBeneficiaries_WhenRepositoryReturnsData()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            SearchInfo searchInfo = new() { ColumnOrder = "Name", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListBeneficiariesQuery command = new(new Guid(), searchInfo);
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(new ListDataInfo<BeneficiariesListResponse>());

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