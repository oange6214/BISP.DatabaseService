using BISP.Base;

namespace BISP.Client.WPF.Models;

public class OperatingState : ViewModelBase
{
    private bool _add;

    public bool Add
    {
        get { return _add; }
        set { _add = value; OnPropertyChanged(); }
    }

    private bool _remove;

    public bool Remove
    {
        get { return _remove; }
        set { _remove = value; OnPropertyChanged(); }
    }


    private bool _update;

    public bool Update
    {
        get { return _update; }
        set { _update = value; OnPropertyChanged(); }
    }

    private bool _cancel;

    public bool Cancel
    {
        get { return _cancel; }
        set { _cancel = value; OnPropertyChanged(); }
    }

}
