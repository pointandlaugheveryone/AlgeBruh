using AlgeBruh.Models;
using System;
using System.Reactive;
using ReactiveUI;
using System.Windows.Input;

namespace AlgeBruh.ViewModels;

public class MainWindowViewModel : ViewModelBase  {
    private readonly Calculator _calculator = new Calculator();
    private readonly InputHandler _input = new();

    private string _displayValue = String.Empty;
    public string DisplayValue
    {
        get => _displayValue;
        set => this.RaiseAndSetIfChanged(ref _displayValue, value);
    }

    public ICommand AddCharCommand {get; }
    public ICommand ClearCommand {get; }
    public ICommand DeleteLastCommand {get; }
    public ICommand CalcCommand {get; }

    public MainWindowViewModel()
    {
        _calculator = new Calculator();
        _input = new InputHandler();

        // init commands
        AddCharCommand = ReactiveCommand.Create<string>(AddChar);
        ClearCommand = ReactiveCommand.Create(Clear);
        DeleteLastCommand = ReactiveCommand.Create(Delete);
        CalcCommand = ReactiveCommand.Create(Calculate);
    }

    private void AddChar(string c)
    {
        DisplayValue += c;
    }
    private void Clear()
    {
        DisplayValue = String.Empty;
    }
    private void Delete()
    {
        if (!String.IsNullOrEmpty(DisplayValue))
        {
            DisplayValue = DisplayValue[..^1];
        }
    }
    private void Calculate()
    {
        try
        {
            _input.ParseInput(DisplayValue);
            // <a _calculator method call to process queue from line above
            DisplayValue = _calculator.CurrentResult.ToString();
        }
        catch (Exception ex)
        {
            DisplayValue = ex.Message;
        }
    }
}
