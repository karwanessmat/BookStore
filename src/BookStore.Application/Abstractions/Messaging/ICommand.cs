using BookStore.SharedKernel.Abstractions;
using MediatR;

namespace BookStore.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : 
    IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}



