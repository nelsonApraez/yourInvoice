///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalGeneralInformation;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetLegalGeneralInformationQueryHandlerTest
    {
        private readonly Mock<ILegalGeneralInformationRepository> _mockLegalGeneralInformationRepository;
        private GetLegalGeneralInformationQueryHandler _handler;

        public GetLegalGeneralInformationQueryHandlerTest()
        {
            _mockLegalGeneralInformationRepository = new Mock<ILegalGeneralInformationRepository>();
        }

        [Fact]
        public async Task Handler_Get_LegalGeneralInformationQuery_Sucess()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.GetLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(LegalGeneralInformationData.GetLegalGeneralInformationResponse);

            _handler = new GetLegalGeneralInformationQueryHandler(_mockLegalGeneralInformationRepository.Object);

            var query = LegalGeneralInformationData.GetLegalGeneralInformationQuery;
            var result = await _handler.Handle(query, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handler_Get_LegalGeneralInformationQuery_Not_Current_User_validation()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.GetLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(LegalGeneralInformationData.GetLegalGeneralInformationResponse);

            _handler = new GetLegalGeneralInformationQueryHandler(_mockLegalGeneralInformationRepository.Object);

            var query = LegalGeneralInformationData.GetLegalGeneralInformationQueryEmpy;
            var result = await _handler.Handle(query, default);

            result.IsError.Should().BeTrue();
            Assert.Null(result.Value);
        }
    }
}