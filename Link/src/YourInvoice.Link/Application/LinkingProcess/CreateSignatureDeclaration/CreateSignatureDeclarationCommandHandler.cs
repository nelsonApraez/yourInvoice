///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.CreateSignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using SignatureDeclaration = yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration.SignatureDeclaration;

public sealed class CreateSignatureDeclarationCommandHandler : IRequestHandler<CreateSignatureDeclarationCommand, ErrorOr<bool>>
{
    private readonly ISignatureDeclarationRepository signatureDeclarationRepository;
    private readonly IUnitOfWorkLink unitOfWorkLink;

    public CreateSignatureDeclarationCommandHandler(ISignatureDeclarationRepository signatureDeclarationRepository, IUnitOfWorkLink unitOfWorkLink)
    {
        this.signatureDeclarationRepository = signatureDeclarationRepository ?? throw new ArgumentNullException(nameof(signatureDeclarationRepository));
        this.unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(unitOfWorkLink));
    }



    public async Task<ErrorOr<bool>> Handle(CreateSignatureDeclarationCommand command, CancellationToken cancellationToken)
    {

        SignatureDeclaration signatureDeclaration = new SignatureDeclaration(
                    Guid.NewGuid(),
                    command.id_GeneralInformation,
                    command.generalStatement,
                    command.visitAuthorization,
                    command.sourceFundsDeclaration,
                    command.completed,
                    command.statusId,
                    ExtensionFormat.DateTimeCO(),
                    command.status,
                    ExtensionFormat.DateTimeCO(),
                    command.createBy,
                    null,
                    null
                    );
        var exist = await this.signatureDeclarationRepository.ExistsSignatureDeclarationByIdAsync(command.id_GeneralInformation);
        if (!exist)
        {
            await signatureDeclarationRepository.CreateSignatureDeclarationAsync(signatureDeclaration);
            await unitOfWorkLink.SaveChangesAsync(cancellationToken);
        }

        return true;
    }
}
