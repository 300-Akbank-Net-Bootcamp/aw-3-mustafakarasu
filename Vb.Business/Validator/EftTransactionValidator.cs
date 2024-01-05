﻿using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator;

public class EftTransactionValidator : AbstractValidator<EftTransactionRequest>
{
    public EftTransactionValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty().ScalePrecision(18,4);
        RuleFor(x => x.Description).MaximumLength(300);
        RuleFor(x => x.ReferenceNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SenderAccount).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SenderIban).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SenderName).NotEmpty().MaximumLength(50);

    }
}