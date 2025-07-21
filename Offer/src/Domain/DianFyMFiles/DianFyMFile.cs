///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.DianFyMFiles
{
    public sealed class DianFyMFile : AggregateRoot
    {
        public DianFyMFile(Guid id, int offer, string name, string description, bool status, string pathStorage, int countRegisterFile, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            Offer = offer;
            Name = name;
            Description = description;
            PathStorage = pathStorage;
            CountRegisterFile = countRegisterFile;
            Status = status;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
        }

        public DianFyMFile()
        {
        }

        public Guid Id { get; private set; }
        public int Offer { get; set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string PathStorage { get; set; }
        public int CountRegisterFile { get; set; }
    }
}