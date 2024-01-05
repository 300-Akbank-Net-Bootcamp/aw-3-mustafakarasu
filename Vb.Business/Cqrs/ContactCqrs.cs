using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vb.Base.Response;
using Vb.Schema;

namespace Vb.Business.Cqrs;

public record CreateContactCommand(ContactRequest Model) : IRequest<ApiResponse<ContactResponse>>;
public record UpdateContactCommand(int Id, ContactRequest Model) : IRequest<ApiResponse>;
public record DeleteContactCommand(int Id) : IRequest<ApiResponse>;

public record GetAllContactQuery() : IRequest<ApiResponse<List<ContactResponse>>>;
public record GetContactByIdQuery(int Id) : IRequest<ApiResponse<ContactResponse>>;
public record GetContactByParameterQuery(int CustomerId, string ContactType, string Information) : IRequest<ApiResponse<List<ContactResponse>>>;