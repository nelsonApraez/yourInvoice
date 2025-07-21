///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations
{
    public class WorkingInformation : AggregateRoot
    {
        public WorkingInformation()
        { }

        public WorkingInformation(Guid id, Guid id_GeneralInformation, string? businessName,
            string? profession, string? position, decimal? phoneNumber, Guid? departmentState,
            Guid? city, string? address, Guid? completed, Guid? statusId, string? whatTypeProductServiceSell, DateTime? statusDate)
        {
            Id = id;
            Id_GeneralInformation = id_GeneralInformation;
            BusinessName = businessName;
            Profession = profession;
            Position = position;
            PhoneNumber = phoneNumber;
            DepartmentState = departmentState;
            City = city;
            Address = address;
            Completed = completed;
            StatusId = statusId;
            WhatTypeProductServiceSell = whatTypeProductServiceSell;
            StatusDate = statusDate;
        }

        public Guid Id_GeneralInformation { get; set; }
        public string? BusinessName { get; set; }
        public string? Profession { get; set; }
        public string? Position { get; set; }
        public decimal? PhoneNumber { get; set; }
        public Guid? DepartmentState { get; set; }
        public Guid? City { get; set; }
        public string? Address { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public string? WhatTypeProductServiceSell { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}