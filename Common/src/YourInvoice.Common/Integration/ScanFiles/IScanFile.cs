///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;

namespace yourInvoice.Common.Integration.ScanFiles
{
    public interface IScanFile
    {
        bool ValidateFile(IFormFile file);
    }
}