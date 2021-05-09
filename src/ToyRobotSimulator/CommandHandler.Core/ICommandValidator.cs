namespace CommandHandler.Core
{
    public interface ICommandValidator<in TCommand>
    {
        void Validate(TCommand command);
    }
}