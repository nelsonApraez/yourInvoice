///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using MediatR;
using yourInvoice.Common.Business.CatalogModule;

using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Buyer.ProcessFile;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.OperationFiles;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;

namespace Application.Customer.UnitTest.Buyer.Process
{
    public class ProcessFileCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockIUnitOfWork;
        private readonly Mock<IFileOperation> _mockIFileOperation;
        private readonly Mock<IStorage> _mockIStorage;
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private readonly Mock<IOperationFileRepository> _mockIOperationFileRepository;
        private readonly Mock<IOfferRepository> _mockIOfferRepository;
        private readonly Mock<INotificationRepository> _mockINotificationRepository;
        private readonly Mock<IEventNotificationsRepository> _mockIEventNotificationsRepository;
        private readonly Mock<ICatalogBusiness> _mockICatalogBusiness;
        private readonly Mock<IHistoricalStatesRepository> _mockIHistoricalStatesRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<IUserRepository> _mockIUserRepository;
        private ProcessFileCommandHandler _handler;

        public ProcessFileCommandHandlerTest()
        {
            _mockIUnitOfWork = new Mock<IUnitOfWork>();
            _mockIFileOperation = new Mock<IFileOperation>();
            _mockIStorage = new Mock<IStorage>();
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockIOperationFileRepository = new Mock<IOperationFileRepository>();
            _mockIOfferRepository = new Mock<IOfferRepository>();
            _mockINotificationRepository = new Mock<INotificationRepository>();
            _mockIEventNotificationsRepository = new Mock<IEventNotificationsRepository>();
            _mockICatalogBusiness = new Mock<ICatalogBusiness>();
            _mockIHistoricalStatesRepository = new Mock<IHistoricalStatesRepository>();
            _mockIUserRepository = new Mock<IUserRepository>();
            _mockIMediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task HandlerProcessFile_WhenNumberOffer_GetInformations()
        {
            _mockICatalogBusiness.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ProcessFileData.GetCatalogItemInfo);
            _mockIOperationFileRepository.Setup(s => s.GetFilesWithoutProcessAsync()).ReturnsAsync(ProcessFileData.GeOperationFile);
            _mockIOperationFileRepository.Setup(s => s.UpdateStartDateAsync(It.IsAny<List<OperationFile>>())).ReturnsAsync(true);
            _mockINotificationRepository.Setup(s => s.AddAsync(It.IsAny<List<Notification>>())).ReturnsAsync(true);
            _mockIEventNotificationsRepository.Setup(s => s.AddAsync(It.IsAny<List<EventNotification>>())).ReturnsAsync(true);
            _mockIOperationFileRepository.Setup(s => s.UpdateStateToProcessAsync(It.IsAny<List<OperationFile>>())).ReturnsAsync(true);
            _mockIStorage.Setup(s => s.DownloadByteAsync(It.IsAny<string>())).ReturnsAsync(new byte[0]);
            _mockIFileOperation.Setup(s => s.ReadFileExcel<PurchaseOperation>(It.IsAny<byte[]>())).Returns(ProcessFileData.GetPurchaseOperation);
            _mockIUnitOfWork.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockIMediator.Setup(m => m.Send(It.IsAny<AddHistoricalCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            _mockIUserRepository.Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(UserData.GetUser);
            _handler = new ProcessFileCommandHandler(_mockIUnitOfWork.Object, _mockIFileOperation.Object, _mockIStorage.Object, _mockIInvoiceDispersionRepository.Object,
            _mockIOperationFileRepository.Object, _mockIOfferRepository.Object, _mockINotificationRepository.Object, _mockIEventNotificationsRepository.Object, _mockICatalogBusiness.Object,
            _mockIMediator.Object, _mockIUserRepository.Object);

            ProcessFileCommand command = new ProcessFileCommand();
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value);
        }
    }
}