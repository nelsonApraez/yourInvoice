///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences
{
    public interface IPersonalReferenceRepository
    {
        Task<GetReferenceResponse> GetPersonalReferenceAsync(Guid idGeneralInformation);

        Task<bool> ExistsPersonalReferencesByIdAsync(Guid idGeneralIfnormation);

        PersonalReferences Add(PersonalReferences personalReferences);

        Task<bool> UpdatePersonalReferencesAsync(PersonalReferences personalReferences);
    }
}
