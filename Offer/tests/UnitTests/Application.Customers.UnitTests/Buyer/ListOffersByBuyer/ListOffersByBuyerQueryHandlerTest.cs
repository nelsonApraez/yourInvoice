///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Buyer.ListOffers;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain.Users.Queries;

namespace Application.Customer.UnitTest.Buyer.ListOffersByBuyer
{
    public class ListOffersByBuyerQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private ListOffersByBuyerQueryHandler _handler;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<ISystem> _mockISystem;

        public ListOffersByBuyerQueryHandlerTest()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandleListOffersByBuyer_WhenRepositoryReturnsData()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            SearchInfo searchInfo = new() { ColumnOrder = "OfferNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListOffersByBuyerQuery command = new(searchInfo, false);
            _mockRepository.Setup(x => x.ListOffersAsync(It.IsAny<Guid>(), searchInfo, It.IsAny<int>())).ReturnsAsync(new ListDataInfo<OfferListResponse>()
            { Data = new List<OfferListResponse>() { new OfferListResponse() { PurchaseValue = 10, FutureValue = 10, CreationDate = DateTime.Now.ToString() } } });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListOffersByBuyerQueryHandler(_mockRepository.Object, _mockCatalogBusiness.Object, _mockISystem.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListOffersHistoryByBuyer_WhenRepositoryReturnsData()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            SearchInfo searchInfo = new() { ColumnOrder = "OfferNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListOffersByBuyerQuery command = new(searchInfo, true);
            _mockRepository.Setup(x => x.ListOffersHistoryAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(new ListDataInfo<OfferListResponse>()
            { Data = new List<OfferListResponse>() { new OfferListResponse() { PurchaseValue = 10, FutureValue = 10, CreationDate = DateTime.Now.ToString() } } });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListOffersByBuyerQueryHandler(_mockRepository.Object, _mockCatalogBusiness.Object, _mockISystem.Object);
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