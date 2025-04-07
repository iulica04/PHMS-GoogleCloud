using MediatR;
using Domain.Common;

namespace Application.Commands.User
{
    public abstract class UserCommand<T> : IRequest<Result<T>>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
    }
}
