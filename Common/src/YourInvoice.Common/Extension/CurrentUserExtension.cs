///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using yourInvoice.Common.Constant;
using System.Security.Claims;

namespace yourInvoice.Common.Extension
{
    public class CurrentUserExtension
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string typeEmailClaim = "emails";

        public CurrentUserExtension(IHttpContextAccessor httpContextAccessor)
         => _httpContextAccessor = httpContextAccessor;

        public bool IsAuthenticated
         => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated
        ?? false;

        public string UserId
         => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ConstantCode_Claims.OID)
         ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ConstantCode_Claims.Sub)
        ?? string.Empty;

        public string UserName
         => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ConstantCode_Claims.Name)
         ?? string.Empty;

        public string UserEmail
         => ((ClaimsIdentity)_httpContextAccessor.HttpContext?.User?.Identity)?.Claims?.FirstOrDefault(c => c?.Type?.ToLowerInvariant() == typeEmailClaim)?.Value ?? string.Empty;
    }
}