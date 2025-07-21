///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Caching.Memory;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Web.API.Middlewares
{
    public class SessionMiddleware
    {
        private IMemoryCache _cache;
        private ISystem _system;
        private IUserRepository _userRepository;
        private IAccountRepository _accountRepository;

        private readonly RequestDelegate _next;
        private readonly ILogger<SessionMiddleware> _logger;

        private readonly string[] serviceProccessFactoring = { "/api/buyer/getfileftp", "/api/buyer/process/file", "/api/buyer/reminder", "/api/buyer/review/expired", "/api/dian/getfileftpdian", "/api/dian/process/filedian", "/api/linkingprocess/truora/validateProcess" };
        private readonly string nameCache = "emailUserB2C";
        private const string emailUserTempModeDEBUG = "prueba.juridica@yopmail.com";
        private const string emailUserSystem = "builtin@yourInvoice.co";

        public SessionMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<SessionMiddleware> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, CurrentUserExtension currentUserExtension, ISystem system, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            try
            {
                _system = system ?? throw new ArgumentNullException(nameof(system));
                _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
                _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

#if DEBUG

                await GetLoginUserAsync(context, currentUserExtension, IsModeDebug: false);
#else

                await GetLoginUserAsync(context, currentUserExtension);
#endif
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        private async Task UnAuthorizedAsync(HttpContext context, int code)
        {
            string status = code == 403 ? "UnAuthenticated" : "Unauthorized";
            context.Response.StatusCode = code;
            var problemDetails = new ProblemDetails
            {
                Status = code,
                Type = "Server Error",
                Title = status,
                Detail = $"An internal server has ocurred {status}.",
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        private async Task GetLoginUserAsync(HttpContext context, CurrentUserExtension currentUserExtension, bool IsModeDebug = false)
        {
            try
            {
                var pathProcess = context.Request.Path.Value ?? string.Empty;
                var isProcessSystem = serviceProccessFactoring.Any(s => s.Contains(pathProcess));
                if (!isProcessSystem && !currentUserExtension.IsAuthenticated && !IsModeDebug)
                {
                    await UnAuthorizedAsync(context, StatusCodes.Status403Forbidden); 
                    return;
                }

                var emailB2C = isProcessSystem ? emailUserSystem : (IsModeDebug ? emailUserTempModeDEBUG : currentUserExtension.UserEmail);
                if (String.IsNullOrEmpty(emailB2C))
                {
                    await UnAuthorizedAsync(context, StatusCodes.Status401Unauthorized);
                    return;
                }

                List<User> cacheEmails;
                _cache.TryGetValue(nameCache, out cacheEmails);
                User tempEmail = cacheEmails?.FirstOrDefault(c => c.Email == emailB2C);

                if (tempEmail != null && !String.IsNullOrEmpty(tempEmail.Email))
                {
                    _system.Set(tempEmail);
                    await _next.Invoke(context);
                    return;
                }

                var user = await _userRepository.GetByEmailAsync(emailB2C);
                cacheEmails = cacheEmails is null ? new List<User>() : cacheEmails;

                user = user == null ? new() { Email = emailB2C } : user;

                cacheEmails.Add(user);
                _cache.Set(nameCache, cacheEmails);
                _system.Set(user);
                await _next.Invoke(context);
                return;
            }
            catch (Exception error)
            {
                _logger.LogError("---- ERROR EN EL MIDDLEWARE CACHE ------" + error.Message);
            }
        }
    }

    public static class GlobalSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpManage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddleware>();
        }
    }
}