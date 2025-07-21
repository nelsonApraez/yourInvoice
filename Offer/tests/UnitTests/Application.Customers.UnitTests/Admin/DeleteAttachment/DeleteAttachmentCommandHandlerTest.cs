///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Admin.DeleteAttachment;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Admin.DeleteAttachment
{
    public class DeleteAttachmentCommandHandlerTest
    {
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IStorage> _mockIStorage;
        private readonly Mock<IUnitOfWork> _mockIUnitOfWork;

        private DeleteAttachmentCommandHandler _handler;

        public DeleteAttachmentCommandHandlerTest()
        {
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockIStorage = new Mock<IStorage>();
            _mockIUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task HandlerDeleteAttachmentCommand_WhenNotIs_True()
        {
            _mockIDocumentRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DeleteAttachmentData.GetDocuments);
            _mockIStorage.Setup(s => s.DeleteBlobByUrlAsync(It.IsAny<string>())).ReturnsAsync(DeleteAttachmentData.ResultDeleteStorage);
            _mockIUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(DeleteAttachmentData.SaveChange);
            _handler = new DeleteAttachmentCommandHandler(_mockIDocumentRepository.Object, _mockIStorage.Object, _mockIUnitOfWork.Object);
            DeleteAttachmentCommand comman = new DeleteAttachmentCommand(Guid.NewGuid());
            var result = await _handler.Handle(comman, default);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandlerDeleteAttachmentCommand_WhenNotIs_False()
        {
            _mockIDocumentRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(DeleteAttachmentData.GetDocumentsEmpy);
            _mockIStorage.Setup(s => s.DeleteBlobByUrlAsync(It.IsAny<string>())).ReturnsAsync(DeleteAttachmentData.ResultDeleteStorage);
            _mockIUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(DeleteAttachmentData.SaveChange);
            _handler = new DeleteAttachmentCommandHandler(_mockIDocumentRepository.Object, _mockIStorage.Object, _mockIUnitOfWork.Object);
            DeleteAttachmentCommand comman = new DeleteAttachmentCommand(Guid.NewGuid());
            var result = await _handler.Handle(comman, default);
            Assert.False(result.Value);
        }
    }
}