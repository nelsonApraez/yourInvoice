///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Payers.GetByNit;
using yourInvoice.Offer.Domain.Payers;

namespace Application.Customer.UnitTest.Offer.Payer
{
    public class GetPayerByNitQueryHandlerTest
    {
        private readonly Mock<IPayerRepository> _mockIPayerRepository;
        private GetPayerByNitQueryHandler _handler;

        public GetPayerByNitQueryHandlerTest()
        {
            _mockIPayerRepository = new Mock<IPayerRepository>();
            _handler = new GetPayerByNitQueryHandler(_mockIPayerRepository.Object);
        }

        [Fact]
        public async Task HandleAllPayerByNit_WhenNitParameters_IsEmpty()
        {
            GetPayerByNitQuery command = new GetPayerByNitQuery(string.Empty);
            var result = await _handler.Handle(command, default);
            Assert.True(result.FirstError.Type == ErrorType.Validation);
        }

        [Fact]
        public async Task HandleAllPayerByNit_WhenNit_GetInformations()
        {
            _mockIPayerRepository.Setup(s => s.GetAllPayerByNitAsync(It.IsAny<string>())).ReturnsAsync(PayerData.GetPayers);
            _handler = new GetPayerByNitQueryHandler(_mockIPayerRepository.Object);
            GetPayerByNitQuery command = new GetPayerByNitQuery(PayerData.GetNit);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.Any());
        }

        [Fact]
        public async Task HandleAllPayerByNit_WhenNit_NotGetInformations()
        {
            _mockIPayerRepository.Setup(s => s.GetAllPayerByNitAsync(It.IsAny<string>())).ReturnsAsync(PayerData.GetPayersEmpty);
            _handler = new GetPayerByNitQueryHandler(_mockIPayerRepository.Object);
            GetPayerByNitQuery command = new GetPayerByNitQuery(PayerData.GetNit);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.Count == 0);
        }
    }
}