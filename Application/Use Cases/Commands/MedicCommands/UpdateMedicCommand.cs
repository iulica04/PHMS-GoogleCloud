using Application.Commands.User;
using MediatR;

namespace Application.Commands.Medic
{
    public class UpdateMedicCommand : UserCommand<Unit>
    {
        public Guid Id { get; set; }
        public required string Rank { get; set; }
        public required string Specialization { get; set; }
        public required string Hospital { get; set; }
    }
}
