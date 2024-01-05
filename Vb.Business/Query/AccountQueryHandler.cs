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

namespace Vb.Business.Query;

public class AccountQueryHandler :
    IRequestHandler<GetAllAccountQuery, ApiResponse<List<AccountResponse>>>,
    IRequestHandler<GetAccountByCustomerIdQuery, ApiResponse<AccountResponse>>,
    IRequestHandler<GetAccountByParameterQuery, ApiResponse<List<AccountResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAllAccountQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Account>()
            .Include(x => x.AccountTransactions)
            .Include(x => x.EftTransactions)
            .Include(x => x.Customer)
            .ToListAsync(cancellationToken);

        var mappedList = mapper.Map<List<Account>, List<AccountResponse>>(list);
        return new ApiResponse<List<AccountResponse>>(mappedList);
    }

    public async Task<ApiResponse<AccountResponse>> Handle(GetAccountByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Account>()
            .Include(x => x.AccountTransactions)
            .Include(x => x.EftTransactions)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.CustomerId == request.Id, cancellationToken);

        if ( entity == null )
        {
            return new ApiResponse<AccountResponse>("Record not found");
        }

        var mapped = mapper.Map<Account, AccountResponse>(entity);
        return new ApiResponse<AccountResponse>(mapped);
    }

    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAccountByParameterQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Account>()
            .Include(x => x.AccountTransactions)
            .Include(x => x.EftTransactions)
            .Include(x => x.Customer)
            .Where(x => x.AccountNumber == request.AccountNumber ||
                        x.CurrencyType.ToUpper().Contains(request.CurrencyType.ToUpper()) ||
                        x.OpenDate <= request.OpenDate)
            .ToListAsync(cancellationToken);

        var mappedList = mapper.Map<List<Account>, List<AccountResponse>>(list);
        return new ApiResponse<List<AccountResponse>>(mappedList);
    }
}