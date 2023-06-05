using BISP.Base;
using System;

namespace BISP.Client.WPF.Store;

public class NavigationStore
{
	private ViewModelBase _currentViewModel;

	public ViewModelBase CurrentViewModel
	{
		get { return _currentViewModel; }
		set 
		{
			_currentViewModel?.Dispose();
			_currentViewModel = value;
			OnCurrentViewModelChange();
        }
	}

	public event Action CurrentViewModelChanged;

	private void OnCurrentViewModelChange()
	{
		CurrentViewModelChanged?.Invoke();
	}
}
