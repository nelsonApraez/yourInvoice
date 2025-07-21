///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetLegalSignatureDeclarationQueryHandlerTest
    {
        private readonly Mock<ILegalSignatureDeclarationRepository> _mockLegalSignatureDeclarationRepository;
        private GetLegalSignatureDeclarationQueryHandler _handler;

        public GetLegalSignatureDeclarationQueryHandlerTest()
        {
            _mockLegalSignatureDeclarationRepository = new Mock<ILegalSignatureDeclarationRepository>();
        }

        [Fact]
        public async Task Handler_Get_LegalSignatureDeclaration_Sucess()
        {
            //_mockLegalSignatureDeclarationRepository.Setup(s => s.GetAccounLegalGeneralAsync(It.IsAny<Guid>())).ReturnsAsync(LegalSignatureDeclarationData.GetAccounLegalGeneralResponse);
            //_mockLegalSignatureDeclarationRepository.Setup(s => s.GeParagraphAsync(It.IsAny<string>())).ReturnsAsync(LegalSignatureDeclarationData.GetParagraph);
            //_mockLegalSignatureDeclarationRepository.Setup(s => s.GetLegalSignatureDeclarationAsync(It.IsAny<Guid>())).ReturnsAsync(LegalSignatureDeclarationData.GetLegalSignatureDeclarationResponse);

            //_handler = new GetLegalSignatureDeclarationQueryHandler(_mockLegalSignatureDeclarationRepository.Object);

            //var query = LegalSignatureDeclarationData.GetLegalSignatureDeclarationQuery;
            //var result = await _handler.Handle(query, default);

            //Assert.NotNull(result.Value);
        }
    }
}