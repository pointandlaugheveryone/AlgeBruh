using AlgeBruh.Models;
using System;
using System.Reactive;
using ReactiveUI;

namespace AlgeBruh.ViewModels;

public class MainWindowViewModel : ViewModelBase  {
    
    private const double defaultValue = 0;
    private double _firstValue;
    private double _secondValue;

    private string _inputValueString = "";
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
            if (ShownValue.showMessage) {
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
        if (ShownValue.showMessage) {
        ShownValue = Result<double>.Success(value);
        _inputValueString = value.ToString();
        }

        else {
            _inputValueString += value.ToString();
            ShownValue = Result<double>.Success(double.Parse(_inputValueString));
        }
    }

    private void RemoveLastNumber() {     
        if (ShownValue.showMessage || ShownValue.Value == 0) {
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
        if (operation == Operation.SquareRoot) {        // this is extremely ugly but i just want it to work
            if (_firstValue < 0) {
                ShownValue = Result<double>.Failure("Taking it complex huh? No.");
                return;
            } 

            else {
                _firstValue = Math.Round(Math.Sqrt(_firstValue), 10);
                ShownValue = Result<double>.Success(_firstValue);
                _inputValueString = "";
                return;
            }

        }

        if (!string.IsNullOrEmpty(_inputValueString)) {
        _secondValue = double.Parse(_inputValueString);
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
        _inputValueString = "";
        }
        else {
            _operation = operation;
            _inputValueString = "";
        }
    }

    public void ClearScreen() {
        ShownValue = Result<double>.Success(0);
        _operation = Operation.Addition;
        _firstValue = 0;
        _secondValue = 0;
        _inputValueString = "";
        }
    
    public void Exit() {
            Environment.Exit(0);
        }
}
