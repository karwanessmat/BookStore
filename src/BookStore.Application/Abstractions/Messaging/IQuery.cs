using BookStore.SharedKernel.Abstractions;
using MediatR;

namespace BookStore.Application.Abstractions.Messaging;


public interface IQuery : IRequest<Result>, IBaseCommand
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery
{
}

public interface IBaseQuery
{
}

