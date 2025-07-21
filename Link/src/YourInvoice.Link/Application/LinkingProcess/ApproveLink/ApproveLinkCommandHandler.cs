///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Application.LinkingProcess.ApproveLink.EmailApprove;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.ApproveLink
{
    public sealed class ApproveLinkCommandHandler : IRequestHandler<ApproveLinkCommand, ErrorOr<bool>>
    {
        private readonly IMediator mediator;

        public ApproveLinkCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ErrorOr<bool>> Handle(ApproveLinkCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.IdUserLink;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = idCurrentUser, StatusLinkId = CatalogCodeLink_LinkStatus.Linked });
            await this.mediator.Publish(new EmailApproveNotificationCommand { Id = idCurrentUser });

            return true;
        }
    }
}