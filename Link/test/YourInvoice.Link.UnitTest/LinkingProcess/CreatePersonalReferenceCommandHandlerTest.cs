///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreatePersonalReference;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreatePersonalReferenceCommandHandlerTest
    {
        private readonly Mock<IPersonalReferenceRepository> _mockPersonalReferenceRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private CreatePersonalReferenceCommandHandler _handler;


        public CreatePersonalReferenceCommandHandlerTest() { 
            this._mockPersonalReferenceRepository = new Mock<IPersonalReferenceRepository>();
            this._mockUnitOfWork = new Mock<IUnitOfWorkLink>();

            this._handler = new CreatePersonalReferenceCommandHandler(this._mockPersonalReferenceRepository.Object, this._mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handler_CreateReferences_WithoutIdGeneralInformation()
        {
            //Arrange
            var command = PersonalReferenceData.PersonalReferenceCommandCreateEmpty;

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handler_CreateReferences_DataExists()
        {
            //Arrange
            var command = PersonalReferenceData.PersonalReferenceCommandCreate;

            _mockPersonalReferenceRepository.Setup(x => x.ExistsPersonalReferencesByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handler_CreateReferences_Success()
        {
            //Arrange
            var command = PersonalReferenceData.PersonalReferenceCommandCreate;

            _mockPersonalReferenceRepository.Setup(x => x.ExistsPersonalReferencesByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockPersonalReferenceRepository.Setup(x => x.Add(PersonalReferenceData.PersonalReferencesCreated)).Returns(PersonalReferenceData.PersonalReferencesCreated);
            _mockUnitOfWork.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.Value);
        }
    }
}
