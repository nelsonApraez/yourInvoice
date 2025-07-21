///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Common.Entities;
using yourInvoice.Link.Application.Accounts.List;
using yourInvoice.Link.Domain.Accounts;

namespace yourInvoice.Link.UnitTest.Account
{
    public class ListQueryHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private ListQueryHandler _handler;

        public ListQueryHandlerTest()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
        }

        [Fact]
        public async Task HandleListAll_WhenGetInformationExist_Sucess()
        {
            ListQuery query = new(ListData.GetSearchInfo);
            _mockAccountRepository.Setup(x => x.GetListAsync(It.IsAny<SearchInfo>())).ReturnsAsync(ListData.GetListResponse);

            _handler = new ListQueryHandler(_mockAccountRepository.Object);
            var result = await _handler.Handle(query, default);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListAll_WhenGetInformationOrderByDefault_Sucess()
        {
            ListQuery query = new(ListData.GetSearchInfoOrder);
            _mockAccountRepository.Setup(x => x.GetListAsync(It.IsAny<SearchInfo>())).ReturnsAsync(ListData.GetListResponse);
            _handler = new ListQueryHandler(_mockAccountRepository.Object);
            var result = await _handler.Handle(query, default);
            Assert.NotNull(result.Value);
        }
    }
}