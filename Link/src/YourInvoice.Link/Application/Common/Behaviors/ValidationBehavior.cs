///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validat = null)
        {
            _validator = validat;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }

            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (result.IsValid)
            {
                return await next();
            }

            var errors = result.Errors
                        .ConvertAll(Failure => Error.Validation(
                            Failure.PropertyName,
                            Failure.ErrorMessage
                        ));

            return (dynamic)errors;
        }
    }
}