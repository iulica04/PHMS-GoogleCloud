using Application.Commands.User;
using MediatR;

namespace Application.Commands.Patient
{
    public class UpdatePatientCommand: UserCommand<Unit>
    {
        public Guid Id { get; set; }
    }
}
