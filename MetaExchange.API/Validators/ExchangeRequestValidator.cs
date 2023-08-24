using FluentValidation;
using MetaExchange.API.Controllers.Requests;

namespace MetaExchange.API.Validators
{
    public class ExchangeRequestValidator : AbstractValidator<ExchangeRequest>
    {
        public ExchangeRequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount should not be 0")
                .GreaterThan(0).WithMessage("Amount should be greater 0");

            RuleFor(x => x.RequestType)
                .IsInEnum().WithMessage("Invalid reques type");
        }
    }
}
