///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreatePersonalReference
{
    public sealed class CreatePersonalReferenceCommandHandler : IRequestHandler<CreatePersonalReferenceCommand, ErrorOr<bool>>
    {
        private readonly IPersonalReferenceRepository personalReferenceRepository;
        private readonly IUnitOfWorkLink unitOfWorkLink;

        public CreatePersonalReferenceCommandHandler(IPersonalReferenceRepository personalReferenceRepository, IUnitOfWorkLink unitOfWorkLink)
        {
            this.personalReferenceRepository = personalReferenceRepository ?? throw new ArgumentNullException(nameof(personalReferenceRepository));
            this.unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(unitOfWorkLink));
        }

        public async Task<ErrorOr<bool>> Handle(CreatePersonalReferenceCommand command, CancellationToken cancellationToken)
        {

            var idCurrentUser = command?.id_GeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }

            var exist = await this.personalReferenceRepository.ExistsPersonalReferencesByIdAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "referencias personales"));
            }

            SaveInDB(command);
            await unitOfWorkLink.SaveChangesAsync(cancellationToken);

            return true;
        }

        private void SaveInDB(CreatePersonalReferenceCommand command)
        {
            PersonalReferences personalReferences = new PersonalReferences(
                Guid.NewGuid(),
                command.id_GeneralInformation,
                command.namePersonalReference,
                command.phoneNumber,
                command.nameBussines,
                command.departmentState,
                command.city,
                command.completed,
                Guid.Empty,
                ExtensionFormat.DateTimeCO(), 
                true,
                ExtensionFormat.DateTimeCO(),
                command.id_GeneralInformation,
                null,
                Guid.Empty
                );

            personalReferenceRepository.Add(personalReferences);
        }
    }
}
