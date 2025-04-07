using Application.Commands.User;

namespace Application.Commands.Medic
{
    public class CreateMedicCommand : UserCommand<Guid>
    {
        public required string Rank { get; set; }
        public required string Specialization { get; set; }
        public required string Hospital { get; set; }
    }
}
