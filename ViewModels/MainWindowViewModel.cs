using AlgeBruh.Models;
using ReactiveUI;
using System;
using System.Reactive;
using System.Windows.Input;

namespace AlgeBruh.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly InputHandler _input = new();
        private readonly Calculator _calculator;

        private string _displayValue = String.Empty;
        public string DisplayValue
        {
            get => _displayValue;
            set => this.RaiseAndSetIfChanged(ref _displayValue, value);
        }

        public ReactiveCommand<int, Unit> AddCharCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteLastCommand { get; }
        public ReactiveCommand<Operation, Unit> CalcCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public MainWindowViewModel()
        {
            _calculator = new Calculator(_input);

            // Initialize commands
            AddCharCommand = ReactiveCommand.Create<int>(AddChar);
            ClearCommand = ReactiveCommand.Create(Clear);
            DeleteLastCommand = ReactiveCommand.Create(Delete);
            CalcCommand = ReactiveCommand.Create<Operation>(Calculate);
            ExitCommand = ReactiveCommand.Create(ExitApp);
        }

        private void AddChar(int number)
        {
            DisplayValue += number.ToString();
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

        private void Calculate(Operation operation)
        {
            try
            {
                if (operation == Operation.Result)
                {
                    string result = _calculator.GetResult(DisplayValue);
                    DisplayValue = result;
                }
                else
                {
                    // Append operation symbol or handle accordingly
                    DisplayValue += GetOperationSymbol(operation);
                }
            }
            catch (Exception ex)
            {
                DisplayValue = ex.Message;
            }
        }

        private string GetOperationSymbol(Operation operation)
        {
            return operation switch
            {
                Operation.Addition => "+",
                Operation.Substraction => "-",
                Operation.Multiplication => "*",
                Operation.Division => "/",
                Operation.Square => "²",
                Operation.SquareRoot => "√",
                Operation.Power => "^",
                Operation.Result => "=",

                _ => string.Empty
            };
        }

        private void ExitApp()
        {
            Environment.Exit(0);
        }
    }
}
