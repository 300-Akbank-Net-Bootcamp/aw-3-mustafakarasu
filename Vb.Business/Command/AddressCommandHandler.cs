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

namespace Vb.Business.Command;

public class AddressCommandHandler :
    IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
    IRequestHandler<UpdateAddressCommand, ApiResponse>,
    IRequestHandler<DeleteAddressCommand, ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AddressCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AddressRequest, Address>(request.Model);

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Address, AddressResponse>(entityResult.Entity);
        return new ApiResponse<AddressResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Address>().Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if ( fromdb == null )
        {
            return new ApiResponse("Record not found");
        }

        var updatedEntity = mapper.Map<Address>(request.Model);

        dbContext.Addresses.Update(updatedEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Address>().Where(x => x.Id == request.Id)
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