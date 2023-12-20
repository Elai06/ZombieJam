namespace Infrastructure.Windows
{
    public interface IWindowFactory
    {
        Window Create(WindowType windowType);
    }
}