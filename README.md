# DevExpress WinForms GridControl sample (.NET 10)

This sample creates a WinForms application that demonstrates:

- `GridControl` + `GridView` bound to `BindingList<ItemModel>`.
- A `Name` column.
- A combined `Active / Quantity` column rendered with a custom in-cell control (`CheckEdit` + `SpinEdit`).
- MVVM-style separation with a `MainViewModel` that owns data and header-check-state logic.
- A tri-state header checkbox that toggles all row checkboxes.
- `Quantity` automatically resetting to `0` when `IsActive` is set to `false`.

## Files

- `DevExpressWinforms1/Models/ItemModel.cs` - data model and row-level state behavior.
- `DevExpressWinforms1/ViewModels/MainViewModel.cs` - collection and bulk toggle logic.
- `DevExpressWinforms1/Views/ActiveQuantityEditControl.cs` - combined checkbox + numeric editor UI.
- `DevExpressWinforms1/Views/MainForm.cs` - grid setup, MVVM bindings, tri-state header checkbox drawing + interaction.

## Note

`DevExpress.Win` package access may require an authenticated NuGet feed/subscription in your environment.
