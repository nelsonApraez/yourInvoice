///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Persistence.Repositories;

namespace yourInvoice.Common.UnitTests.Business
{
    public class CatalogBusinessTest
    {
        private readonly Mock<ICatalogRepository> mockICatalogRepository = new Mock<ICatalogRepository>();

        public CatalogBusinessTest()
        {
        }

        [Fact]
        public async Task ListByCatalog_When_IsEmpty()
        {
            this.mockICatalogRepository.Setup(x => x.ListByCatalogAsync(It.IsAny<string>())).ReturnsAsync(new List<CatalogItemInfo>());

            var business = new CatalogBusiness(this.mockICatalogRepository.Object);

            var result = await business.ListByCatalogAsync(string.Empty);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ListByCatalog_When_NotIsEmpty()
        {
            this.mockICatalogRepository.Setup(x => x.ListByCatalogAsync(It.IsAny<string>())).ReturnsAsync(CatalogBusinessData.GetCatalogItemInfo);

            var business = new CatalogBusiness(this.mockICatalogRepository.Object);

            var result = await business.ListByCatalogAsync("TIPODOCUMENTO");

            Assert.True(result.Any());
        }
    }
}