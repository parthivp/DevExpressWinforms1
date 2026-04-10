using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DevExpressWinforms1.Models;

public class ItemModel : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private bool _isActive;
    private double _quantity;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (!SetProperty(ref _isActive, value))
            {
                return;
            }

            if (!_isActive)
            {
                Quantity = 0;
            }
        }
    }

    public double Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, value))
        {
            return false;
        }

        backingField = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
