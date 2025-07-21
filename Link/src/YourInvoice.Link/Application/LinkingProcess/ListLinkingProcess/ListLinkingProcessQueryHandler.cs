
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;


namespace yourInvoice.Link.Application.LinkingProcess.ListLinkingProcess
{
    public sealed class ListLinkingProcessQueryHandler : IRequestHandler<ListLinkingProcessQuery, ErrorOr<ListDataInfo<ListLinkingProccessResponse>>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly string orderDefault = "statusid";
        private readonly string fieldOrder = "name";
        private readonly string fieldOrderChange = "NameOrder";

        public ListLinkingProcessQueryHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ErrorOr<ListDataInfo<ListLinkingProccessResponse>>> Handle(ListLinkingProcessQuery query, CancellationToken cancellationToken)
        {
            //query.pagination.ColumnOrder = query.pagination.ColumnOrder.ToLowerInvariant().Equals(fieldOrder) ? fieldOrderChange : query.pagination.ColumnOrder;
            int pageSize = query.pagination.PageSize;
            var account = await this.accountRepository.GetListLinkingProcessesAsync(query.pagination);

            if (account is not null)
            {
                var pending = account.Data.Select(s => new ListLinkingProccessResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserTypeId = s.UserTypeId,
                    UserType = s.UserType,
                    PersonTypeId = s.PersonTypeId,
                    PersonType = s.PersonType,
                    DocumentTypeId = s.DocumentTypeId,
                    DocumentType = s.DocumentType,
                    DocumentNumber = s.DocumentNumber,
                    CompletionPercentage =  s.CompletionPercentage > 95  ? 100  : s.CompletionPercentage,
                    StatusId = s.StatusId,
                    Status = s.Status,
                    StatusDate = s.StatusDate,
                    CreatedOn = s.CreatedOn,

                }).ToList();


                if (query.pagination.ColumnOrder == "default") {
                    var estadoPrioridades = new List<string>
                        {
                            "Pendiente Aprobación",
                            "Pendiente Firma",
                            "Validación Rechazada",
                            "Firma no Exitosa",
                            "En Proceso",
                            "Vinculado",
                            "Rechazado"
                        };
                    // Ordenar la lista según la prioridad de estados
                    var itemsOrdenadoEspecial = pending.OrderBy(i => estadoPrioridades.IndexOf(i.Status)).ToList();
                    return new ListDataInfo<ListLinkingProccessResponse>
                    {
                        Count = itemsOrdenadoEspecial.Count,
                        
                        Data = itemsOrdenadoEspecial.Skip(query.pagination.StartIndex).Take(pageSize).ToList()
                    };
                }

                var filter = query.pagination.filter;
                var nameColumn = query.pagination.ColumnOrder.UpperFirtsLetter();
                var IsOrderAsc = query.pagination.OrderType.ToLowerInvariant().Equals("asc");
                if (string.IsNullOrEmpty(filter))
                {
                    var DataTemp = IsOrderAsc ? pending.OrderBy(nameColumn).ToList() : pending.OrderByDescending(nameColumn).ToList();
                    return new ListDataInfo<ListLinkingProccessResponse>
                    {
                        Count = DataTemp.Count,
                        Data = DataTemp.Skip(query.pagination.StartIndex).Take(pageSize).ToList()
                    };
                }
                var pendingTemp = pending.Where(c => c.PersonType.ToString().Equals(filter) || c.Name.ToLower().Contains(filter.ToLower()) || c.UserType.ToLower().Contains(filter.ToLower()) || c.Status.ToLower().Contains(filter.ToLower())
                || c.DocumentType.ToLower().Contains(filter.ToLower()) || c.DocumentNumber.ToString().Equals(filter)).ToList();
                var DataTemp2 = IsOrderAsc ? pendingTemp.OrderBy(nameColumn).ToList() : pendingTemp.OrderByDescending(nameColumn).ToList();
                return new ListDataInfo<ListLinkingProccessResponse>
                {
                    Count = DataTemp2.Count,
                    Data = DataTemp2.Skip(query.pagination.StartIndex).Take(pageSize).ToList()
                };
            }
            return new ListDataInfo<ListLinkingProccessResponse>();


        }
    }
}