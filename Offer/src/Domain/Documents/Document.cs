///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public Document()
        {
        }

        public Document(Guid id, Guid offerId, Guid? relatedId, string name, Guid typeId, bool isSigned, string url, string fileSize = null, string tokenZapsign = null)
        {
            Id = id;
            OfferId = offerId;
            RelatedId = relatedId;
            Name = name;
            TypeId = typeId;
            IsSigned = isSigned;
            Url = url;
            FileSize = fileSize;
            TokenZapsign = tokenZapsign;
        }

        public Guid OfferId { get; private set; }

        public Guid? RelatedId { get; private set; }// esta propiedad tambien recibe el UserId correspondiente al buyer

        public string Name { get; set; }

        public Guid? TypeId { get; private set; }

        public bool? IsSigned { get; set; }

        public string Url { get; set; }

        public string FileSize { get; set; }

        public string TokenZapsign { get; set; }

        public Offer Offer { get; set; }
    }
}