///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Beneficiary.Delete;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Beneficiary
{
    public class DeleteBeneficiaryCommandHandlerTest
    {
        private readonly Mock<IMoneyTransferRepository> _mockIMoneyTransferRepository;
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IOfferRepository> _mockIOfferRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteBeneficiaryCommandHandler _handler;

        public DeleteBeneficiaryCommandHandlerTest()
        {
            mockStorage = new Mock<IStorage>();
            _mockIMoneyTransferRepository = new Mock<IMoneyTransferRepository>();
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockIOfferRepository = new Mock<IOfferRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteBeneficiaryCommandHandler(_mockIDocumentRepository.Object, _mockIMoneyTransferRepository.Object, _mockUnitOfWork.Object, mockStorage.Object, _mockIOfferRepository.Object);
        }

        [Fact]
        public async Task HandleDeleteBeneficiary_WhenIdParameters_ShouldTrue()
        {
            DeleteBeneficiaryCommand command = new DeleteBeneficiaryCommand(new List<Guid>() { new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840") });

            _mockIMoneyTransferRepository.Setup(x => x.ExistsByIdAsync(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(true);
            _mockIMoneyTransferRepository.Setup(x => x.GetByIdAsync(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new MoneyTransfer());
            _mockIDocumentRepository.Setup(y => y.GetDocumentsByOfferAndRelatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIOfferRepository.Setup(z => z.OfferIsInProgressByBeneficiaryIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleDeleteBeneficiary_WhenWithoutParameters_ShouldError()
        {
            DeleteBeneficiaryCommand command = new DeleteBeneficiaryCommand(new List<Guid>());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            Assert.True(result.IsError);
        }
    }
}