///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Application.HistoricalStates.Add
{
    public class AddHistoricalCommandHanler : INotificationHandler<AddHistoricalCommand>
    {
        private readonly IHistoricalStatesRepository historicalStatesRepository;
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly IOfferRepository offerRepository;
        private readonly IUnitOfWork unitOfWork;

        public AddHistoricalCommandHanler(IHistoricalStatesRepository historicalStatesRepository, IInvoiceDispersionRepository invoiceDispersionRepository, IOfferRepository offerRepository, IUnitOfWork unitOfWork)
        {
            this.historicalStatesRepository = historicalStatesRepository;
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.offerRepository = offerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(AddHistoricalCommand command, CancellationToken cancellationToken)
        {
            if (command.NumberOffer != 0)
            {
                var historical = new List<HistoricalState>();
                command.OfferId = await this.offerRepository.GetIdOfferAsync(command.NumberOffer);
                if (command.InvoiceDispersionId is null || command.InvoiceDispersionId.Count <= 0)
                {
                    command.InvoiceDispersionId = await this.invoiceDispersionRepository.GetIdsAsync(command.NumberOffer, command.PayerId ?? Guid.NewGuid());
                }
                foreach (var invoiceDispersionId in command.InvoiceDispersionId)
                {
                    historical.Add(new HistoricalState(Guid.NewGuid(), command.StatusId, command.OfferId, invoiceDispersionId, true, ExtensionFormat.DateTimeCO(), command.UserId, ExtensionFormat.DateTimeCO(), command.UserId));
                }
                await this.historicalStatesRepository.AddListAsync(historical);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }
            var historicalState = new HistoricalState(Guid.NewGuid(), command.StatusId, command.OfferId, null, true, ExtensionFormat.DateTimeCO(), command.UserId, ExtensionFormat.DateTimeCO(), command.UserId);
            await this.historicalStatesRepository.AddAsync(historicalState);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}