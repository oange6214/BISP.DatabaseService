using BISP.Base;
using BISP.Client.WPF.Store;

namespace BISP.Client.WPF.ViewModel;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private readonly NavigationStore _navigationStore;

    #endregion

    #region Properties

    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

    #endregion

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

}

