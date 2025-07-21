///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.Accounts.Validity;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.Roles;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Link.UnitTest.Account
{
    public class ValidityQueryHandlerTest
    {
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ISystem> _mockSystem;
        private readonly ValidityQueryHandler _handler;

        public ValidityQueryHandlerTest()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockSystem = new Mock<ISystem>();

            _handler = new ValidityQueryHandler(_mockRoleRepository.Object, _mockSystem.Object, _mockAccountRepository.Object);
        }

        [Fact]
        public async Task HandleValidity_WhenRoleExist_And_VinculationApproved()
        {
            //Arrange
            ValidityQuery command = new();
            _mockSystem.Setup(x => x.User).Returns(UserData.GetUser);
            _mockRoleRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>())).ReturnsAsync(new List<GetRoleResponse>() { ValidityData.GetRoleRes });
            _mockAccountRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(ValidityData.GetAccountNaturalResponse);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            Assert.Equal(3, result.Value.Process);
        }

        [Fact]
        public async Task HandleValidity_WhenVinculationPending()
        {
            //Arrange
            ValidityQuery command = new();
            _mockSystem.Setup(x => x.User).Returns(UserData.GetUser);
            _mockRoleRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>())).ReturnsAsync(new List<GetRoleResponse>() { ValidityData.GetRoleResNull });
            _mockRoleRepository.Setup(x => x.GetRoleNewUserAsync(It.IsAny<string>())).ReturnsAsync(ValidityData.GetRoleResponse);
            _mockAccountRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(ValidityData.GetAccountApprovedResponse);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            Assert.Equal(2, result.Value.Process);
        }

        [Fact]
        public async Task HandleValidity_WhenPreRegisterPending()
        {
            //Arrange
            ValidityQuery command = new();
            _mockSystem.Setup(x => x.User).Returns(UserData.GetUser);
            _mockRoleRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>())).ReturnsAsync(new List<GetRoleResponse>() { ValidityData.GetRoleResNull });
            _mockAccountRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(ValidityData.GetAccountResponse);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            Assert.Equal(1, result.Value.Process);
        }
    }
}