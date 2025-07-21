using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.Document
{
    public class Document : AggregateRoot
    {
        public Document()
        {
        }

        public Document(Guid id, Guid? relatedId, string name, Guid typeId, bool isSigned, string url, string fileSize = null, string tokenZapsign = null, string processIdTruora = null)
        {
            Id = id;
            ProcessIdTruora = processIdTruora;
            RelatedId = relatedId;
            Name = name;
            TypeId = typeId;
            IsSigned = isSigned;
            Url = url;
            FileSize = fileSize;
            TokenZapsign = tokenZapsign;
        }

        /// <summary>
        /// Esta propiedad recibe los Id que sea necesarios para relacionar el documento.
        /// </summary>
        public Guid? RelatedId { get; private set; }

        public string Name { get; set; }

        public Guid? TypeId { get; private set; }

        public bool? IsSigned { get; set; }

        public string Url { get; set; }

        public string FileSize { get; set; }

        public string TokenZapsign { get; set; }

        public string ProcessIdTruora { get; set; }
    }
}