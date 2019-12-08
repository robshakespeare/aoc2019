namespace Common
{
    public interface IInputLoader<out TInput>
    {
        TInput LoadInput();
    }
}
