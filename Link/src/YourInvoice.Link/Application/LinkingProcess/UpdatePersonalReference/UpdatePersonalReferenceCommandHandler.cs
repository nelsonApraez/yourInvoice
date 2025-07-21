///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;

namespace yourInvoice.Link.Application.LinkingProcess.UpdatePersonalReference
{
    public class UpdatePersonalReferenceCommandHandler : IRequestHandler<UpdatePersonalReferenceCommand, ErrorOr<bool>>
    {
        private readonly IPersonalReferenceRepository personalReferenceRepository;

        public UpdatePersonalReferenceCommandHandler(IPersonalReferenceRepository personalReferenceRepository)
        {
            this.personalReferenceRepository = personalReferenceRepository;
        }

        public async Task<ErrorOr<bool>> Handle(UpdatePersonalReferenceCommand request, CancellationToken cancellationToken)
        {
            var data = UtilityBusinessLink.PassDataOriginDestiny(request.PersonalReferences, new PersonalReferences());
            await personalReferenceRepository.UpdatePersonalReferencesAsync(data);

            return true;
        }
    }
}
