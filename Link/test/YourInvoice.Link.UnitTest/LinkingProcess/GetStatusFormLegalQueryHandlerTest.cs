///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Application.LinkingProcess.GetStatusFormLegal;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Offer.Domain.Notifications;
using System.Linq.Expressions;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetStatusFormLegalQueryHandlerTest
    {
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<ILinkStatusRepository> _mockILinkStatusRepository;

        private GetStatusFormLegalQueryHandler _handler;

        public GetStatusFormLegalQueryHandlerTest()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockIMediator = new Mock<IMediator>();
            _mockILinkStatusRepository = new Mock<ILinkStatusRepository>();
        }

        [Fact]
        public async Task Handler_Get_StatusForm_Sucess()
        {
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalGeneralInformation, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalGeneralInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalBoardDirector, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalBoardDirector);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalFinancialInformation, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalFinancialInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalRepresentativeTaxAuditor, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalRepresentativeTaxAuditor);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalShareholder, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalShareholder);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalCommercialAndBankReference, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalCommercialAndBankReference);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<LegalSAGRILAFT, bool>>>())).ReturnsAsync(StatusFormLegalData.LegalSAGRILAFT);
            _mockILinkStatusRepository.Setup(c => c.GetLinkStatusAsync(It.IsAny<Guid>())).ReturnsAsync(new LinkStatus());
            _mockIMediator.Setup(m => m.Send(It.IsAny<ChangeLinkStatusCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");

            _handler = new GetStatusFormLegalQueryHandler(_mockPersonRepository.Object, _mockIMediator.Object, _mockILinkStatusRepository.Object);

            var command = new GetStatusFormLegalQuery(Guid.NewGuid());
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }
    }
}