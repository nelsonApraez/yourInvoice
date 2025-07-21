///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    using MediatR;
    using Moq;
    using yourInvoice.Link.Application.LinkingProcess.EmailToRequestDocument;
    using yourInvoice.Link.Application.LinkingProcess.SendRequestUpdateDocuments;
    using yourInvoice.Link.Domain.Accounts;
    using yourInvoice.Offer.Domain.Notifications;

    public class SendRequestUpdateDocumentCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockIAccountRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private SendRequestUpdateDocumentCommandHandler _handler;

        public SendRequestUpdateDocumentCommandHandlerTest()
        {
            _mockIAccountRepository = new Mock<IAccountRepository>();
            _mockIMediator = new Mock<IMediator>();

            _handler = new SendRequestUpdateDocumentCommandHandler(_mockIAccountRepository.Object, _mockIMediator.Object);
        }

        [Fact]
        public async Task HandleSendRequestDocument_Sucess()
        {
            //Arrange
            SendRequestUpdateDocumentCommand command = new(
                accountId: Guid.NewGuid(), 
                request: new RequestUpdateDocuments
                {
                    DisplayName = "JOHN DOE SMITH",
                    Email= "email@pruebas.com",
                    Message = "Con el fin de continuar el estudio de su solicitud de vinculación, se le solicita enviar los documentos que se listan a continuación:"
                }
            );
            _mockIAccountRepository.Setup(s => s.GetAccountIdAsync(It.IsAny<Guid>())).ReturnsAsync(SendRequestDocumentData.GetRequestDocumentResponse);
            _mockIMediator.Setup(m => m.Send(It.IsAny<EmailToRequestDocumentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleSendRequestDocument_NoSuccess_AccountEmpty()
        {
            //Arrange
            SendRequestUpdateDocumentCommand command = new(
                accountId: Guid.NewGuid(),
                request: new RequestUpdateDocuments
                {
                    DisplayName = "JOHN DOE SMITH",
                    Email = "email@pruebas.com",
                    Message = "Con el fin de continuar el estudio de su solicitud de vinculación, se le solicita enviar los documentos que se listan a continuación:"
                }
            );
            _mockIAccountRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(SendRequestDocumentData.GetRequestDocumentResponseEmpty);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.False(result.Value);
        }
    }
}
