using System.Threading.Tasks;

namespace Infrastructure.Windows.MVVM
{
    public interface IViewModel
    {
        void Initialize();
        void Subscribe();
        void Unsubscribe();
        void Cleanup();
        void Show();
    }
}