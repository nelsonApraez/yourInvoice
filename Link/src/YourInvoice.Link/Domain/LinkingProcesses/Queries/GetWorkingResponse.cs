///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetWorkingResponse
    {
        public Guid Id { get; set; }
        public Guid? Id_GeneralInformation { get; set; }
        public string? BusinessName { get; set; }
        public string? Profession { get; set; }
        public string? Position { get; set; }
        public decimal? PhoneNumber { get; set; }
        public Guid? DepartmentState { get; set; }
        public string DescriptionDepartmentState { get; set; }
        public Guid? City { get; set; }
        public string DescriptionCity { get; set; }
        public string? Address { get; set; }
        public Guid? Completed { get; set; }
        public string? DescriptionCompleted { get; set; }
        public string? WhatTypeProductServiceSell { get; set; }
    }
}