///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Application.LinkingProcess.Common;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateBank
{
    public class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, ErrorOr<bool>>
    {
        private readonly IBankInformationRepository repository; 

       public UpdateBankCommandHandler(IBankInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateBankCommand command, CancellationToken cancellationToken)
        {
            var bank = UtilityBusinessLink.PassDataOriginDestiny(command.UpdateBanks, new BankInformation());
            await this.repository.UpdateBankAsync(bank);
            return true;
        }


    }
}