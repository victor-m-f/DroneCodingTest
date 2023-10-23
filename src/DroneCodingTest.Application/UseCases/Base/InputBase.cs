using DroneCodingTest.Application.Validation;
using MediatR;

namespace DroneCodingTest.Application.UseCases.Base;

public abstract class InputBase<TOutput> : IRequest<TOutput>
    where TOutput : OutputBase
{
    public virtual ValidationResult Validate() => new();
}
