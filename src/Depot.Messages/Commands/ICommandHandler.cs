using System.Threading.Tasks;

namespace Depot.Messages.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}