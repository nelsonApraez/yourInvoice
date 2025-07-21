///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetPersonalReference;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetPersonalReferenceQueryHandlerTest
    {
        private readonly Mock<IPersonalReferenceRepository> _mockPersonalReferenceRepository;
        private GetPersonalReferenceQueryHandler _handler;

        public GetPersonalReferenceQueryHandlerTest()
        {
            this._mockPersonalReferenceRepository = new Mock<IPersonalReferenceRepository>();

            this._handler = new GetPersonalReferenceQueryHandler(this._mockPersonalReferenceRepository.Object);
        }

        [Fact]
        public async Task Handler_GetPersonalReference_Success()
        {
            //Arrange
            var command = PersonalReferenceData.GetPersonalReferenceRequest;
            _mockPersonalReferenceRepository.Setup(x => x.GetPersonalReferenceAsync(It.IsAny<Guid>())).ReturnsAsync(PersonalReferenceData.GetPersonalReferenceResponse);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.NotNull(result);
        }
    }
}
