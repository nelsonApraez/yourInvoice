///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystem _system;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, ISystem system)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _system = system;
        }

        public async Task<ErrorOr<bool>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var algo = _system.User;

            //realizar validaciones del digito de verificación
            if (command.documentType == 2 && command.documentDigitVerify == null)
                return Error.Validation(MessageCodes.NitIsRequired, GetErrorDescription(MessageCodes.NitIsRequired));

            //organizar objeto a almacenar

            //guardar usuario

            return true;
        }
    }
}