///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetLegalRepresentativeTaxAuditorQueryHandlerTest
    {
        private readonly Mock<ILegalRepresentativeTaxAuditorRepository> _mockLegalRepresentativeTaxAuditorRepository;
        private GetLegalRepresentativeTaxAuditorQueryHandler _handler;

        public GetLegalRepresentativeTaxAuditorQueryHandlerTest()
        {
            _mockLegalRepresentativeTaxAuditorRepository = new Mock<ILegalRepresentativeTaxAuditorRepository>();
        }

        [Fact]
        public async Task Handler_Get_LegalRepresentativeTaxAuditor_Sucess()
        {
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.GetLegalRepresentativeTaxAuditorAsync(It.IsAny<Guid>())).ReturnsAsync(LegalRepresentativeTaxAuditorData.LegalRepresentativeTaxAuditor);

            _handler = new GetLegalRepresentativeTaxAuditorQueryHandler(_mockLegalRepresentativeTaxAuditorRepository.Object);

            var query = LegalRepresentativeTaxAuditorData.GetLegalRepresentativeTaxAuditorQuery;
            var result = await _handler.Handle(query, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handler_Get_LegalRepresentativeTaxAuditor_Not_Current_User_validation()
        {
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.GetLegalRepresentativeTaxAuditorAsync(It.IsAny<Guid>())).ReturnsAsync(LegalRepresentativeTaxAuditorData.LegalRepresentativeTaxAuditor);

            _handler = new GetLegalRepresentativeTaxAuditorQueryHandler(_mockLegalRepresentativeTaxAuditorRepository.Object);

            var query = LegalRepresentativeTaxAuditorData.GetLegalRepresentativeTaxAuditorQueryEmpty;
            var result = await _handler.Handle(query, default);

            result.IsError.Should().BeTrue();
            Assert.Null(result.Value);
        }
    }
}