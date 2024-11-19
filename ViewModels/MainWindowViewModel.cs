using AlgeBruh.Models;
using System;
using System.Reactive;
using ReactiveUI;

namespace AlgeBruh.ViewModels;

public class MainWindowViewModel : ViewModelBase  {
    
    private double _firstValue;
    private double _secondValue;
    private Operation _operation = Operation.Addition;


    private Result<double> _shownValue = Result<double>.Success(0);
    public Result<double> ShownValue {
        get => _shownValue;
        set {
            this.RaiseAndSetIfChanged(ref _shownValue, value);
            this.RaisePropertyChanged(nameof(DisplayValue));
        }
    }

    public string DisplayValue {
        get {
            if (ShownValue.showErrorMessage) {
                return ShownValue.ErrorMessage;
            }
            else {
                return ShownValue.Value.ToString();
            }
        }
    }

    public ReactiveCommand<int, Unit> AddNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberCommand { get; }
    public ReactiveCommand<Operation, Unit> ExecuteCommand { get; }
    
    public MainWindowViewModel() {
        AddNumberCommand = ReactiveCommand.Create<int>(AddNumber);
        ExecuteCommand = ReactiveCommand.Create<Operation>(Execute);
        RemoveLastNumberCommand = ReactiveCommand.Create(RemoveLastNumber);
    }

    private void AddNumber(int value) {
        if (ShownValue.showErrorMessage) {
        ShownValue = Result<double>.Failure("Unhandled error. Goddamnit.");
        }

        else {
            ShownValue = Result<double>.Success(ShownValue.Value * 10 + value);
        }
    }

    private void RemoveLastNumber() {     
        if (ShownValue.Value == 0) {
            ShownValue = Result<double>.Success(0);
            return;
        }
        string shownValueString = ShownValue.Value.ToString();
        shownValueString = shownValueString.Substring(0, shownValueString.Length - 1);

        if (string.IsNullOrEmpty(shownValueString)) {
            ShownValue = Result<double>.Success(0);
        }
        else {
            ShownValue = Result<double>.Success(double.Parse(shownValueString));
        }
    }
    
    private void Execute(Operation operation) {
        if (_inputValueString != "" && ShownValue.Value == 0) {
        _firstValue = double.Parse(_inputValueString);
        }
        switch (_operation) {
            case Operation.Addition: {
                _firstValue += _secondValue;
                break;
            }
            case Operation.Substraction: {
                _firstValue -= _secondValue;
                break;
            }
            case Operation.Multiplication: {
                _firstValue *= _secondValue;
                break;
            }
            case Operation.Division: {
                if (_secondValue == 0) {
                    ShownValue = Result<double>.Failure("No further steps can be trusted.");
                    return;
                }
                else {
                    _firstValue /= _secondValue;
                }
                break;
            }
            case Operation.Square: {
                _firstValue = _firstValue * _firstValue;
                break;
            }
            case Operation.SquareRoot: {
                if (_firstValue <= 0.0) {
                    ShownValue = Result<double>.Failure("Taking it complex huh? No.");
                }
                else {
                    _firstValue = Math.Round(Math.Sqrt(_firstValue), 10);
                }
                break;
            }

            default: {
                ShownValue = Result<double>.Failure("Unhandled error.");
                break;
            }

        }
        
        // reset
        if (operation == Operation.Result) {
        ShownValue = Result<double>.Success(_firstValue);
        }
        else {
            _operation = operation;
        }
    }

    public void ClearScreen() {
        ShownValue = Result<double>.Success(0);
        _operation = Operation.Addition;
        _firstValue = 0;
        _secondValue = 0;
        }
    
    public void Exit() {
            Environment.Exit(0);
        }
}
