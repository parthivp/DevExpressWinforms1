using System.ComponentModel;
using DevExpressWinforms1.Models;

namespace DevExpressWinforms1.ViewModels;

public class MainViewModel
{
    public BindingList<ItemModel> Items { get; } =
    [
        new ItemModel { Name = "Alpha", IsActive = true, Quantity = 10 },
        new ItemModel { Name = "Beta", IsActive = false, Quantity = 0 },
        new ItemModel { Name = "Gamma", IsActive = true, Quantity = 5.5 }
    ];

    public void SetAllRowsActive(bool isActive)
    {
        foreach (var item in Items)
        {
            item.IsActive = isActive;
        }
    }

    public bool? GetHeaderState()
    {
        if (Items.Count == 0)
        {
            return false;
        }

        var activeCount = Items.Count(item => item.IsActive);
        if (activeCount == 0)
        {
            return false;
        }

        if (activeCount == Items.Count)
        {
            return true;
        }

        return null;
    }
}
