///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.Invoice.SaveChangeConfirm;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class SaveChangeConfirmCommandHandlerTest
    {
        private readonly Mock<IInvoiceRepository> _mockIInvoiceRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private SaveChangeConfirmCommandHandler _handler;

        public SaveChangeConfirmCommandHandlerTest()
        {
            _mockIInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new SaveChangeConfirmCommandHandler(_mockIInvoiceRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task HandleSaveChangeConfirm_WhenIdParameters_ShouldFalse()
        {
            SaveChangeConfirmCommand command = new SaveChangeConfirmCommand(new InvoiceExcelValidatedRequest(Guid.NewGuid(), InvoiceData.GetInvoiceExcelValidatedNull));
            var result = await _handler.Handle(command, default);
            Assert.True(result.IsError);
        }

        [Fact]
        public async Task HandleSaveChangeConfirm_WhenSaveInformation()
        {
            _mockIInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockIInvoiceRepository.Setup(s => s.GetAllByOffer(It.IsAny<Guid>())).ReturnsAsync(InvoiceData.GetInvoices);
            _mockUnitOfWork.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _handler = new SaveChangeConfirmCommandHandler(_mockIInvoiceRepository.Object, _mockUnitOfWork.Object);
            SaveChangeConfirmCommand command = new SaveChangeConfirmCommand(new InvoiceExcelValidatedRequest(Guid.NewGuid(), InvoiceData.GetInvoiceExcelValidated));
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value);
        }
    }
}