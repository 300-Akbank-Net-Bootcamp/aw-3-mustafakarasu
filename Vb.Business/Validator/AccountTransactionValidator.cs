using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator;

public class AccountTransactionValidator : AbstractValidator<AccountTransactionRequest>
{
    public AccountTransactionValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty().ScalePrecision(18, 4);
        RuleFor(x => x.Description).MaximumLength(300);
        RuleFor(x => x.TransferType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ReferenceNumber).NotEmpty().MaximumLength(50);
    }
}