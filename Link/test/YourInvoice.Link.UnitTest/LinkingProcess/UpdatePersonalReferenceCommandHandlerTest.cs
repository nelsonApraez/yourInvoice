using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdatePersonalReference;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdatePersonalReferenceCommandHandlerTest
    {
        private readonly Mock<IPersonalReferenceRepository> _mockPersonalReferenceRepository;
        private UpdatePersonalReferenceCommandHandler _handler;

        public UpdatePersonalReferenceCommandHandlerTest()
        {
            this._mockPersonalReferenceRepository = new Mock<IPersonalReferenceRepository>();

            this._handler = new UpdatePersonalReferenceCommandHandler(this._mockPersonalReferenceRepository.Object);
        }

        [Fact]
        public async Task Handler_UpdatePersonalReferences_Success()
        {
            //Arrange
            var command = new UpdatePersonalReferenceCommand(PersonalReferenceData.PersonalReferencesCreated);

            _mockPersonalReferenceRepository.Setup(x => x.UpdatePersonalReferencesAsync(PersonalReferenceData.PersonalReferencesCreated)).ReturnsAsync(true);
            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.Value);
        }
    }
}
