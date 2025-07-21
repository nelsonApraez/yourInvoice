///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;

namespace yourInvoice.Common.Integration.ScanFiles
{
    public class ScanFile : IScanFile
    {
        public bool ValidateFile(IFormFile file)
        {
            return false;
        }
    }
}