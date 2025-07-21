///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Application.LinkingProcess.Common;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateFinancial
{
    public class UpdateFinancialCommandHandler : IRequestHandler<UpdateFinancialCommand, ErrorOr<bool>>
    {
        private readonly IFinancialInformationRepository repository;

        public UpdateFinancialCommandHandler(IFinancialInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ErrorOr<bool>> Handle(UpdateFinancialCommand command, CancellationToken cancellationToken)
        {
            var financials = UtilityBusinessLink.PassDataOriginDestiny(command.UpdateFinancials, new FinancialInformation());
            await this.repository.UpdateFinancialAsync(financials);
            return true;
        }
    }
}