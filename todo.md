# TODO

## Refactor MainWindowViewModel
- [ ] Create `InputHandler` class for managing user input.
- [ ] Initialize `Calculator` and `InputHandler` in `MainWindowViewModel`.
- [ ] Define properties:
  - `DisplayValue` (string)
- [ ] Define commands:
  - `AddNumberCommand`
  - `ClearScreenCommand`
  - `RemoveLastNumberCommand`
  - `ExecuteCommand`
  - `ExitCommand`
- [ ] Implement command methods:
  - `AddNumber(int number)`
  - `ClearScreen()`
  - `RemoveLastNumber()`
  - `ExecuteOperation(Operation operation)`
  - `ExitApplication()`
- [ ] Add error handling in `ExecuteOperation`.
- [ ] Bind commands to UI buttons in `MainWindow.axaml`.

## Enhance Calculator Class
- [ ] Implement `PowerOfX` method.
- [ ] Implement `Factor` method.
- [ ] Implement `Factorial` method.
- [ ] Add method to reset calculator state.

## Improve Input Handling
- [ ] Add validation logic in `InputHandler`.
- [ ] Handle edge cases (e.g., multiple decimal points).

## Update Global Usings
- [ ] Ensure all necessary namespaces are included in `GlobalUsings.cs`.
- [ ] Remove redundant using directives from individual files.

## UI Enhancements
- [ ] Bind `DisplayValue` to the display TextBlock.
- [ ] Ensure all buttons are correctly bound to their respective commands.

## Testing
- [ ] Create unit tests for `Calculator` methods.
- [ ] Create unit tests for `InputHandler`.
- [ ] Create unit tests for `MainWindowViewModel` commands.

## Documentation
- [ ] Update `README.md` with refactored structure.
- [ ] Add comments to complex methods and classes.