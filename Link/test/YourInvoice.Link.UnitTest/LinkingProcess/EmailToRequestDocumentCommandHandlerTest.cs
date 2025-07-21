///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    using Moq;
    using yourInvoice.Common.Business.CatalogModule;
    using yourInvoice.Link.Application.LinkingProcess.EmailToRequestDocument;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EmailToRequestDocumentCommandHandlerTest
    {
        private readonly Mock<ICatalogBusiness> _mockICatalogBusiness;

        private EmailToRequestDocumentCommandHandler _handler;

        public EmailToRequestDocumentCommandHandlerTest()
        {
            _mockICatalogBusiness = new Mock<ICatalogBusiness>();
        }

        [Fact]
        public async Task HandEmailToSellerAdmin_WhenNotIs_empty()
        {
            var result = true;
            var command = new EmailToRequestDocumentCommand()
            {
                AttachData = new Dictionary<string, string>(),
                Label = "Señor(a)",
                Name = "JOHN DOE SMITH",
                Message = "Con el fin de continuar el estudio de su solicitud de vinculación, se le solicita enviar los documentos que se listan a continuación:",
                Email = "deivisajulio@gmail.com",
            };

            _mockICatalogBusiness.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(EmailToRequestDocumentData.GetCatalogItemInfo);
            _mockICatalogBusiness.Setup(s => s.ListByCatalogAsync(It.IsAny<string>())).ReturnsAsync(EmailToRequestDocumentData.GetCatalogItemInfoList);

            _handler = new EmailToRequestDocumentCommandHandler(_mockICatalogBusiness.Object);

            await _handler.Handle(command, default);

            Assert.True(result);
        }
    }
}
