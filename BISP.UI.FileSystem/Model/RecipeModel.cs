using System;
using System.ComponentModel;

namespace BISP.UI.FileSystemByExample.Model;

public class RecipeModel : INotifyPropertyChanged
{
    public Guid Guid { get; set; }

    public string ItemName { get; set; } = null!;

    public int ItemValue { get; set; }

    public DateTime CreateAt { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
