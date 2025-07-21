///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.ScanFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Offer.Invoice.UploadFiles;
using yourInvoice.Offer.Domain.Cufes;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class UploadFilesCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IPayerRepository> _mockPayerRepository;
        private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IScanFile> _mockScanFile;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICufeRepository> _cufeRepository;
        private readonly UploadFilesCommandHandler _handler;
        private IMemoryCache memoryCache;

        public UploadFilesCommandHandlerTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            memoryCache = serviceProvider.GetService<IMemoryCache>();

            _mockRepository = new Mock<IOfferRepository>();
            _mockPayerRepository = new Mock<IPayerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockScanFile = new Mock<IScanFile>();
            _mockStorage = new Mock<IStorage>();
            _cufeRepository = new Mock<ICufeRepository>();

            _handler = new UploadFilesCommandHandler(_mockInvoiceRepository.Object, _mockUnitOfWork.Object, memoryCache, _mockScanFile.Object,
                _mockStorage.Object, _mockRepository.Object, _mockPayerRepository.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _cufeRepository.Object);
        }

        private List<IFormFile> CreateMockFormFiles()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Resource\6445.zip");

            string sFilePath = Path.GetFullPath(sFile);

            // Crear una lista de IFormFile de ejemplo con archivos
            var formFiles = new List<IFormFile>
        {
            CreateMockFormFile("file1.txt", "text/plain", new byte[0]),
            CreateMockFormFile("6445.zip", ConstantCode_MimeType.ZIP, File.ReadAllBytes(sFilePath)),
            // Agregar más archivos según sea necesario
        };

            return formFiles;
        }

        private IFormFile CreateMockFormFile(string fileName, string contentType, byte[] content)
        {
            // Crear un IFormFile de ejemplo con un nombre de archivo, tipo de contenido y contenido
            var stream = new MemoryStream(content);
            var file = new FormFile(stream, 0, content.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }

        [Fact]
        public async Task HandleUploadFiles_WithFile_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            memoryCache.Set(offerid, new InvoiceProcessCache(memoryCache, offerid));
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);

            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847", "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockPayerRepository.Setup(x => x.GetByIdAsync(payerid)).ReturnsAsync(new yourInvoice.Offer.Domain.Payers.Payer(payerid, "890106527", "", "Cliente", "", "", "", true, "", false, "", "", ""));
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            //Domain.Invoices.Invoice invoice = new Domain.Invoices.Invoice(
            _mockInvoiceRepository.Setup(x => x.Add(It.IsAny<yourInvoice.Offer.Domain.Invoices.Invoice>())).Returns(new yourInvoice.Offer.Domain.Invoices.Invoice());
            UploadFilesCommand command = new(offerid, CreateMockFormFiles());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleUploadFiles_WhenListOfFilesIsEmpty_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid sellerid = Guid.NewGuid();
            memoryCache.Set(offerid, new InvoiceProcessCache(memoryCache, offerid));
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, sellerid, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);

            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);

            UploadFilesCommand command = new(offerid, new List<IFormFile>());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleUploadFiles_WhenOfferNotExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            memoryCache.Set(offerid, new InvoiceProcessCache(memoryCache, offerid));

            _mockRepository.Setup(x => x.OfferIsInProgressAsync(offerid)).ReturnsAsync(true);

            UploadFilesCommand command = new(offerid, new List<IFormFile>());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }
    }
}