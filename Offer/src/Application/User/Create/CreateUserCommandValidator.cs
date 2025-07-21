///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Offer.Application.User.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(r => r.name)
                .NotEmpty()
                .MaximumLength(50)
                .WithName("Name");

            RuleFor(r => r.job)
                 .NotEmpty()
                 .MaximumLength(50)
                 .WithName("Job");

            RuleFor(r => r.address)
                 .NotEmpty()
                 .MaximumLength(100)
                  .WithName("Address");

            RuleFor(r => r.phone)
                 .NotEmpty()
                 .MaximumLength(10)
                 .WithName("Phone Number");

            RuleFor(r => r.email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50)
                .WithName("Email");

            RuleFor(r => r.city)
                .NotEmpty()
                .MaximumLength(50)
                .WithName("City");

            RuleFor(r => r.documentType)
                .InclusiveBetween(1, 5)
                .WithName("Document type");

            RuleFor(r => r.documentNumber)
                .NotEmpty()
                .MaximumLength(20)
                .WithName("Document number");

            RuleFor(r => r.documentExpedition)
                .NotEmpty()
                .MaximumLength(50)
                .WithName("Document expedition");

            RuleFor(r => r.userType)
                .InclusiveBetween(1, 2)
                .WithName("User type");

            RuleFor(r => r.role)
               .InclusiveBetween(1, 3)
               .WithName("Role");

            RuleFor(r => r.commercialRegistrationNumber)
               .NotEmpty()
               .MaximumLength(50)
               .WithName("Commercial Registration Number");

            RuleFor(r => r.commercialRegistrationCity)
               .NotEmpty()
               .MaximumLength(50)
               .WithName("Commercial Registration City");

            RuleFor(r => r.chamberOfCommerceCity)
               .NotEmpty()
               .MaximumLength(50)
               .WithName("Chamber Of Commerce City");
        }
    }
}