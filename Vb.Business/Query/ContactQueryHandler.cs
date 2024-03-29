﻿using AutoMapper;
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

public class ContactQueryHandler :
    IRequestHandler<GetAllContactQuery, ApiResponse<List<ContactResponse>>>,
    IRequestHandler<GetContactByIdQuery, ApiResponse<ContactResponse>>,
    IRequestHandler<GetContactByParameterQuery, ApiResponse<List<ContactResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public ContactQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ContactResponse>>> Handle(GetAllContactQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .ToListAsync(cancellationToken);

        var mappedList = mapper.Map<List<Contact>, List<ContactResponse>>(list);
        return new ApiResponse<List<ContactResponse>>(mappedList);
    }

    public async Task<ApiResponse<ContactResponse>> Handle(GetContactByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if ( entity == null )
        {
            return new ApiResponse<ContactResponse>("Record not found");
        }

        var mapped = mapper.Map<Contact, ContactResponse>(entity);
        return new ApiResponse<ContactResponse>(mapped);
    }

    public async Task<ApiResponse<List<ContactResponse>>> Handle(GetContactByParameterQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Contact>()
            .Include(x => x.Customer)
            .Where(x => x.CustomerId == request.CustomerId ||
                        x.ContactType.ToUpper().Contains(request.ContactType.ToUpper()) ||
                        x.Information.ToUpper().Contains(request.Information.ToUpper()))
            .ToListAsync(cancellationToken);

        var mappedList = mapper.Map<List<Contact>, List<ContactResponse>>(list);
        return new ApiResponse<List<ContactResponse>>(mappedList);
    }
}