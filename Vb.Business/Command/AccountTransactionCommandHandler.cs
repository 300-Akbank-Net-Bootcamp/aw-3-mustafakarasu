using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data.Entity;
using Vb.Data;
using Vb.Schema;

namespace Vb.Business.Command;

public class AccountTransactionCommandHandler :
    IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var checkAccountTransaction = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Model.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        if ( checkAccountTransaction != null )
        {
            return new ApiResponse<AccountTransactionResponse>($"There isn't account with {request.Model.AccountId} value.");
        }

        var entity = mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entityResult.Entity);
        return new ApiResponse<AccountTransactionResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if ( fromdb == null )
        {
            return new ApiResponse("Record not found");
        }

        var updatedEntity = mapper.Map<AccountTransaction>(request.Model);

        dbContext.AccountTransactions.Update(updatedEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.AccountId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if ( fromdb == null )
        {
            return new ApiResponse("Record not found");
        }

        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}