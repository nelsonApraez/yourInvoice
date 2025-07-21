///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Xml;

namespace yourInvoice.Offer.Application.Offer.Invoice.UploadFiles
{
    public class InvoiceDataExtracted
    {
        public string Cufe { get; set; }
        public string FechaEmision { get; set; }
        public string Factura { get; set; }
        public string TipoPago { get; set; }
        public string NitEmisor { get; set; }
        public string NitReceptor { get; set; }
        public string FechaVencimiento { get; set; }
        public string CurrencyCode { get; set; }
        public string Trm { get; set; }
        public string Total { get; set; }
        public string TaxAmount { get; set; }

        public InvoiceDataExtracted GetData(XmlDocument xmlDoc, byte[] bytes)
        {
            Cufe = GetElementValue(bytes, "cbc:UUID");
            FechaEmision = GetElementValue(bytes, "cbc:IssueDate");
            Factura = GetElementValue(bytes, "cbc:ID");

            XmlNodeList parentNitEmisor = xmlDoc.GetElementsByTagName("cac:AccountingSupplierParty");
            NitEmisor = GetElementValueByParent(parentNitEmisor, "cbc:CompanyID");

            XmlNodeList parentNitReceptor = xmlDoc.GetElementsByTagName("cac:AccountingCustomerParty");
            NitReceptor = GetElementValueByParent(parentNitReceptor, "cbc:CompanyID");

            XmlNodeList parentPaymentMeans = xmlDoc.GetElementsByTagName("cac:PaymentMeans");
            TipoPago = GetElementValueByParent(parentPaymentMeans, "cbc:ID");

            XmlNodeList parentTaxAmount = xmlDoc.GetElementsByTagName("cac:TaxTotal");
            TaxAmount = GetElementValueByParent(parentTaxAmount, "cbc:TaxAmount");

            string fechaVencimiento2 = GetElementValue(bytes, "cbc:PaymentDueDate");
            FechaVencimiento = GetElementValue(bytes, "cbc:DueDate");

            if (string.IsNullOrEmpty(FechaVencimiento))
            {
                FechaVencimiento = fechaVencimiento2;
            }

            CurrencyCode = GetElementValue(bytes, "cbc:DocumentCurrencyCode");
            Trm = GetElementValue(bytes, "cbc:CalculationRate");
            Total = GetElementValue(bytes, "cbc:PayableAmount");

            return this;
        }

        public bool AnyNullOrEmpty()
        {
            return (string.IsNullOrEmpty(Factura) || string.IsNullOrEmpty(Cufe) || string.IsNullOrEmpty(CurrencyCode) ||
                 string.IsNullOrEmpty(FechaEmision) || string.IsNullOrEmpty(FechaVencimiento) || string.IsNullOrEmpty(NitEmisor) ||
                 string.IsNullOrEmpty(NitReceptor) || string.IsNullOrEmpty(TipoPago) || string.IsNullOrEmpty(Total));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nodos"></param>
        /// <param name="nombreHijo"></param>
        /// <returns></returns>
        private static string GetElementValueByParent(XmlNodeList nodos, string nombreHijo)
        {
            foreach (XmlNode nodo in nodos)
            {
                if (nodo.Name == nombreHijo)
                {
                    return nodo.InnerText;
                }

                if (nodo.HasChildNodes)
                {
                    string valor = GetElementValueByParent(nodo.ChildNodes, nombreHijo);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        return valor;
                    }
                }
            }

            return null; // El hijo no se encontró
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="elemento"></param>
        /// <returns></returns>
        private static string GetElementValue(byte[] xmlData, string elemento)
        {
            try
            {
                using (var xmlStream = new MemoryStream(xmlData))
                using (var xmlReader = XmlReader.Create(xmlStream))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == elemento)
                        {
                            return ReadValue(xmlReader);
                        }
                    }
                }

                // Si no se encuentra el elemento, regresar null
                return null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al obtener el value del elemento: {ex.Message}");
#endif
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        private static string ReadValue(XmlReader xmlReader)
        {
            string val = null;

            if (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Text || xmlReader.NodeType == XmlNodeType.CDATA)
                {
                    val = xmlReader.Value;
                }
                else if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Text || xmlReader.NodeType == XmlNodeType.CDATA)
                        {
                            val = xmlReader.Value;
                            break;
                        }
                    }
                }
            }

            return val;
        }
    }
}