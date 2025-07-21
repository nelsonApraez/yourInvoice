///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Application.LinkingProcess.GetStatusForm;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Offer.Domain.Notifications;
using System.Linq.Expressions;
using G = yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetStatusFormQueryHandlerTest
    {
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<ILinkStatusRepository> _mockILinkStatusRepository;

        private GetStatusFormQueryHandler _handler;

        public GetStatusFormQueryHandlerTest()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockIMediator = new Mock<IMediator>();
            _mockILinkStatusRepository = new Mock<ILinkStatusRepository>();
        }

        [Fact]
        public async Task Handler_Get_StatusForm_Sucess()
        {
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<BankInformation, bool>>>())).ReturnsAsync(StatusFormData.GetBankInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<FinancialInformation, bool>>>())).ReturnsAsync(StatusFormData.GetFinancialInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<ExposureInformation, bool>>>())).ReturnsAsync(StatusFormData.GetExposureInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<G.GeneralInformation, bool>>>())).ReturnsAsync(StatusFormData.GetGeneralInformation);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<PersonalReferences, bool>>>())).ReturnsAsync(StatusFormData.GetPersonalReferences);
            _mockPersonRepository.Setup(c => c.GetAsync(It.IsAny<Expression<Func<WorkingInformation, bool>>>())).ReturnsAsync(StatusFormData.GetWorkingInformation);
            _mockILinkStatusRepository.Setup(c => c.GetLinkStatusAsync(It.IsAny<Guid>())).ReturnsAsync(new LinkStatus());
            _mockIMediator.Setup(m => m.Send(It.IsAny<ChangeLinkStatusCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            _handler = new GetStatusFormQueryHandler(_mockPersonRepository.Object, _mockIMediator.Object, _mockILinkStatusRepository.Object);

            var command = new GetStatusFormQuery(Guid.NewGuid());
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }
    }
}