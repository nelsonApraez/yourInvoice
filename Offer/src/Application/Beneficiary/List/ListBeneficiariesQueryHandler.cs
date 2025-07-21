///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.MoneyTransfers.Queries;

namespace yourInvoice.Offer.Application.Beneficiary.List
{
    public class ListBeneficiariesQueryHandler : IRequestHandler<ListBeneficiariesQuery, ErrorOr<ListDataInfo<BeneficiariesListResponse>>>
    {
        private readonly IMoneyTransferRepository repository;

        public ListBeneficiariesQueryHandler(IMoneyTransferRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<ListDataInfo<BeneficiariesListResponse>>> Handle(ListBeneficiariesQuery command, CancellationToken cancellationToken)
        {
            var result = await this.repository.ListAsync(command.offerId, command.pagination);

            return result;
        }
    }
}