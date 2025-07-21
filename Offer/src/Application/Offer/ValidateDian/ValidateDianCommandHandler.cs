///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Bus;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.ValidateDian
{
    public sealed class ValidateDianCommandHandler : IRequestHandler<ValidateDianCommand, ErrorOr<bool>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOfferRepository _OfferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceBus _serviceBus;
        private readonly ICatalogBusiness _catalogBusiness;

        public ValidateDianCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IServiceBus serviceBus, ICatalogBusiness catalogBusiness,
            IOfferRepository offerRepository)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _OfferRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
        }

        public async Task<ErrorOr<bool>> Handle(ValidateDianCommand request, CancellationToken cancellationToken)
        {
            //si el estado de la oferta no es en progreso saca error
            if (!await _OfferRepository.OfferIsInProgressAsync(request.Id))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            //obtener parametros desde los catalogos y validamos que la descripción tenga un valor
            var conexionStringBus = await _catalogBusiness.GetByIdAsync(CatalogCode_ServiceBus.ConexionString);
            if (string.IsNullOrEmpty(conexionStringBus?.Descripton))
                return Error.Validation(MessageCodes.ServiceBusConexionNotExist, GetErrorDescription(MessageCodes.ServiceBusConexionNotExist));

            var queueCUFES = await _catalogBusiness.GetByIdAsync(CatalogCode_ServiceBus.QueueCUFES);
            if (string.IsNullOrEmpty(queueCUFES?.Descripton))
                return Error.Validation(MessageCodes.ServiceBusConexionQueueCUFESNotExist, GetErrorDescription(MessageCodes.ServiceBusConexionQueueCUFESNotExist));

            //buscar las facturas con estado [Cargado]
            var invoices = await _invoiceRepository.FindByStatus(request.Id, CatalogCode_InvoiceStatus.Loaded);

            //Obtengo informacion de la oferta.
            var offer = await _OfferRepository.GetByIdWithNamesAsync(request.Id);

            //actualizar los estados de las facturas [En validación DIAN]
            invoices.ForEach(invoice =>
            {
                invoice.StatusId = CatalogCode_InvoiceStatus.ValidationDian;
                _invoiceRepository.Update(invoice);
            });
            await _unitOfWork.SaveChangesAsync();

            List<Envoice> cufes = invoices.Select(p => new Envoice { EnvoiceId = p.Id, CUFE = p.Cufe }).Distinct().ToList();

            ValidateDianSend validateDianSend = new()
            {
                OfferId = request.Id,
                Consecutive = offer.consecutive,
                Nit = offer.payerNit,
                Envoices = cufes
            };

            //enviar los cufes al bus
            ServiceBusParameters serviceBusParameters = new()
            {
                ConnectionString = conexionStringBus.Descripton,
                AuthenticationAD = false,
                ClientId = string.Empty,
                ClientSecret = string.Empty,
                FullyQualifiedNamespace = string.Empty,
                TenantId = string.Empty
            };
            _serviceBus.Start(serviceBusParameters);
            await _serviceBus.SendMessageAsync(validateDianSend, queueCUFES.Descripton);

            return true;
        }
    }
}