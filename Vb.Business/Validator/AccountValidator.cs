using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vb.Schema;

namespace Vb.Business.Validator;

public class AccountValidator : AbstractValidator<AccountRequest>
{
    public AccountValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty();
        RuleFor(x => x.Balance).NotEmpty().ScalePrecision(18, 4);
        RuleFor(x => x.CurrencyType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.IBAN).NotEmpty().MaximumLength(34);
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(100);
    }
}