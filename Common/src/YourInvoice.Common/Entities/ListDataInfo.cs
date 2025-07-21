///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Runtime.Serialization;

namespace yourInvoice.Common.Entities
{
    public class ListDataInfo<T>
    {
        public bool ExistsInvoiceAproved { get; set; }

        public bool ExistsInvoiceRejected { get; set; }

        /// <summary>
        /// Cantidad Total de elementos que existen
        /// </summary>
        [DataMember(Name = "totalItems")]
        public int Count { get; set; }

        /// <summary>
        /// Los datos que viajan serializados
        /// </summary>
        [DataMember(Name = "content")]
        public List<T> Data { get; set; }
    }
}