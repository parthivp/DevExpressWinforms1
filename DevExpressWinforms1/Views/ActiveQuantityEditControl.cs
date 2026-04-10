using DevExpressWinforms1.Models;
using DevExpress.XtraEditors;

namespace DevExpressWinforms1.Views;

public class ActiveQuantityEditControl : XtraUserControl
{
    private readonly CheckEdit _checkEdit;
    private readonly SpinEdit _spinEdit;
    private ItemModel? _item;

    public ActiveQuantityEditControl()
    {
        _checkEdit = new CheckEdit
        {
            Dock = DockStyle.Left,
            Width = 28,
            Properties =
            {
                Caption = string.Empty,
                AutoWidth = false
            }
        };

        _spinEdit = new SpinEdit
        {
            Dock = DockStyle.Fill,
            Properties =
            {
                IsFloatValue = true
            }
        };

        Controls.Add(_spinEdit);
        Controls.Add(_checkEdit);

        _checkEdit.CheckedChanged += OnCheckedChanged;
        _spinEdit.EditValueChanged += OnQuantityChanged;
    }

    public void Bind(ItemModel item)
    {
        _item = item;
        _checkEdit.Checked = item.IsActive;
        _spinEdit.Value = (decimal)item.Quantity;
        _spinEdit.Enabled = item.IsActive;
    }

    private void OnCheckedChanged(object? sender, EventArgs e)
    {
        if (_item is null)
        {
            return;
        }

        _item.IsActive = _checkEdit.Checked;
        _spinEdit.Enabled = _item.IsActive;
        if (!_item.IsActive)
        {
            _spinEdit.Value = 0;
        }
    }

    private void OnQuantityChanged(object? sender, EventArgs e)
    {
        if (_item is null || !_item.IsActive)
        {
            return;
        }

        _item.Quantity = (double)_spinEdit.Value;
    }
}
