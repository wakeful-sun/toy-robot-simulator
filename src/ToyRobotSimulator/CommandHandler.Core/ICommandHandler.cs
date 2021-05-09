namespace CommandHandler.Core
{
    public interface ICommandHandler<in TCommand, out TResponse>
    {
        TResponse Execute(TCommand command);
    }
}