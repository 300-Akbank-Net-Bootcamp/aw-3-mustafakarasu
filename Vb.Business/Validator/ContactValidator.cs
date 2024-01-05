using System.Data;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator;

public class ContactValidator : AbstractValidator<ContactRequest>
{
    public ContactValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.ContactType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Information).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IsDefault).NotEmpty();
    }
}