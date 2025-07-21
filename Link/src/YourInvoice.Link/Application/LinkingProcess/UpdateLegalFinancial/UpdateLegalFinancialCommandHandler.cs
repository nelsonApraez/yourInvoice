
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalFinancial
{
    public class UpdateLegalFinancialCommandHandler : IRequestHandler<UpdateLegalFinancialCommand, ErrorOr<bool>>
    {
        private readonly ILegalFinancialInformationRepository repository;

        public UpdateLegalFinancialCommandHandler(ILegalFinancialInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ErrorOr<bool>> Handle(UpdateLegalFinancialCommand command, CancellationToken cancellationToken)
        {
            var financials = UtilityBusinessLink.PassDataOriginDestiny(command.UpdateFinancials, new LegalFinancialInformation());
            financials.OperationsType = ConvertGuidListToString(command.UpdateFinancials.OperationsTypes);
            await this.repository.UpdateLegalFinancialAsync(financials);
            return true;
        }

        public static string ConvertGuidListToString(List<Guid?> guidList)
        {
            return string.Join(",", guidList);
        }
    }
}
