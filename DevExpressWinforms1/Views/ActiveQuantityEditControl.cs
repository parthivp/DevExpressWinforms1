using DevExpressWinforms1.Models;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.CustomEditor;

namespace DevExpressWinforms1.Views;

public class ActiveQuantityEditControl : XtraUserControl, IAnyControlEdit
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

    // IAnyControlEdit implementation
    object IAnyControlEdit.EditValue
    {
        get => (object?)_item ?? DBNull.Value;
        set
        {
            if (value is ItemModel model)
                Bind(model);
        }
    }

    bool IAnyControlEdit.SupportsDraw => false;

    bool IAnyControlEdit.AllowBorder => false;

    bool IAnyControlEdit.AllowBitmapCache => false;

    event EventHandler IAnyControlEdit.EditValueChanged
    {
        add { }
        remove { }
    }

    Size IAnyControlEdit.CalcSize(Graphics g) => Size;

    void IAnyControlEdit.Draw(GraphicsCache cache, AnyControlEditViewInfo viewInfo) { }

    void IAnyControlEdit.SetupAsDrawControl() { }

    void IAnyControlEdit.SetupAsEditControl() { }

    string IAnyControlEdit.GetDisplayText(object editValue) => string.Empty;

    bool IAnyControlEdit.IsNeededKey(KeyEventArgs e) => false;

    bool IAnyControlEdit.AllowClick(Point point) => true;

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
