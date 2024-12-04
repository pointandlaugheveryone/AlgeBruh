using AlgeBruh.Models;
using System;
using System.Reactive;
using ReactiveUI;

namespace AlgeBruh.ViewModels;

public class MainWindowViewModel : ViewModelBase  {
    private readonly Calculator _calculator = new Calculator();
    private string _displayValue = String.Empty;
    public string DisplayValue
    {
        get => _displayValue;
        set => this.RaiseAndSetIfChanged(ref _displayValue, value);
    }
}
