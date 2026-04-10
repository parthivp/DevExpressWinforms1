using DevExpress.Utils.MVVM;
using DevExpressWinforms1.Models;
using DevExpressWinforms1.ViewModels;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DevExpressWinforms1.Views;

public class MainForm : XtraForm
{
    private readonly MVVMContext _mvvmContext;
    private readonly GridControl _grid;
    private readonly GridView _view;
    private readonly GridColumn _nameColumn;
    private readonly GridColumn _activeQuantityColumn;
    private readonly RepositoryItemAnyControl _activeQuantityRepository;
    private readonly MainViewModel _viewModel;

    public MainForm()
    {
        Text = "DevExpress GridControl + MVVM sample";
        Width = 900;
        Height = 500;

        _viewModel = new MainViewModel();

        _mvvmContext = new MVVMContext
        {
            ContainerControl = this,
            ViewModelType = typeof(MainViewModel)
        };

        _grid = new GridControl { Dock = DockStyle.Fill };
        _view = new GridView(_grid)
        {
            OptionsBehavior = { Editable = true },
            OptionsView = { ShowGroupPanel = false }
        };

        _nameColumn = _view.Columns.AddVisible(nameof(ItemModel.Name), "Name");
        _nameColumn.VisibleIndex = 0;

        _activeQuantityColumn = _view.Columns.AddField("ActiveQuantity");
        _activeQuantityColumn.Caption = "Active / Quantity";
        _activeQuantityColumn.UnboundType = DevExpress.Data.UnboundColumnType.Object;
        _activeQuantityColumn.Visible = true;
        _activeQuantityColumn.VisibleIndex = 1;

        _activeQuantityRepository = new RepositoryItemAnyControl
        {
            Name = "repoActiveQuantity"
        };
        _activeQuantityRepository.CreateControl += (_, e) => e.Control = new ActiveQuantityEditControl();
        _activeQuantityRepository.CustomizeControl += (_, e) =>
        {
            if (e.Control is ActiveQuantityEditControl editor && e.Row is ItemModel row)
            {
                editor.Bind(row);
            }
        };

        _activeQuantityColumn.ColumnEdit = _activeQuantityRepository;
        _grid.RepositoryItems.Add(_activeQuantityRepository);

        _grid.MainView = _view;
        _grid.ViewCollection.Add(_view);

        Controls.Add(_grid);

        _view.CustomUnboundColumnData += OnCustomUnboundColumnData;
        _view.CustomDrawColumnHeader += OnCustomDrawColumnHeader;
        _view.MouseDown += OnGridMouseDown;
        _view.CellValueChanged += OnCellValueChanged;

        Load += OnLoad;
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        _mvvmContext.SetViewModel(typeof(MainViewModel), _viewModel);
        var fluent = _mvvmContext.OfType<MainViewModel>();
        fluent.SetBinding(_grid, g => g.DataSource, vm => vm.Items);

        UpdateHeaderGlyph();
    }

    private void OnCustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
    {
        if (!e.IsGetData || e.Column != _activeQuantityColumn)
        {
            return;
        }

        e.Value = _view.GetRow(e.ListSourceRowIndex);
    }

    private void OnCellValueChanged(object sender, CellValueChangedEventArgs e)
    {
        if (e.Column.FieldName is nameof(ItemModel.IsActive) or nameof(ItemModel.Quantity))
        {
            UpdateHeaderGlyph();
        }
    }

    private void OnCustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
    {
        if (e.Column != _activeQuantityColumn)
        {
            return;
        }

        e.Painter.DrawObject(e.Info);
        DrawHeaderCheck(e);
        e.Handled = true;
    }

    private void DrawHeaderCheck(ColumnHeaderCustomDrawEventArgs e)
    {
        var state = _viewModel.GetHeaderState();
        var editorInfo = new CheckEditViewInfo(new RepositoryItemCheckEdit { AllowGrayed = true })
        {
            CheckState = state switch
            {
                true => CheckState.Checked,
                false => CheckState.Unchecked,
                _ => CheckState.Indeterminate
            }
        };

        var glyphSize = 18;
        var rect = new Rectangle(
            e.Bounds.Right - glyphSize - 8,
            e.Bounds.Top + (e.Bounds.Height - glyphSize) / 2,
            glyphSize,
            glyphSize);

        editorInfo.Bounds = rect;
        editorInfo.CalcViewInfo(e.Cache.Graphics);

        var painter = new CheckEditPainter();
        using var args = new ControlGraphicsInfoArgs(editorInfo, new GraphicsCache(e.Cache.Graphics), rect);
        painter.Draw(args);
    }

    private void OnGridMouseDown(object? sender, MouseEventArgs e)
    {
        var hit = _view.CalcHitInfo(e.Location);
        if (!hit.InColumn || hit.Column != _activeQuantityColumn)
        {
            return;
        }

        var headerRect = hit.ColumnInfo?.Bounds ?? Rectangle.Empty;
        var checkRect = new Rectangle(headerRect.Right - 26, headerRect.Top + (headerRect.Height - 18) / 2, 18, 18);
        if (!checkRect.Contains(e.Location))
        {
            return;
        }

        var state = _viewModel.GetHeaderState();
        var newValue = state != true;
        _viewModel.SetAllRowsActive(newValue);
        _view.RefreshData();
        UpdateHeaderGlyph();
    }

    private void UpdateHeaderGlyph()
    {
        _view.LayoutChanged();
    }
}
