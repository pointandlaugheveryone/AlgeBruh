namespace AlgeBruh.Models
{
    class Calculator
    {
        public double CurrentResult {get; private set;} = 0;

        private void Calculate(Operation operation, double value1, double value2)
        {
            switch (operation)
            {
                case Operation.Addition:
                    CurrentResult = value1 + value2;
                    break;
                
                case Operation.Substraction:
                    CurrentResult = value1 - value2;
                    break;
                
                case Operation.Multiplication:
                    CurrentResult = value1 * value2;
                    break;

                case Operation.Division:
                    if (value1 == 0)
                        throw new DivideByZeroException("No further steps can be trusted.");
                    CurrentResult = value1 / value2;
                    break;

                case Operation.Power:
                    CurrentResult = Math.Pow(value1, value2);
                    break;

                default:
                   throw new InvalidOperationException("Unhandled error.");
            }
        }

        private void CalculateUnary(Operation operation, double inputValue)
        {
            switch (operation)
            {
                case Operation.Square:
                    CurrentResult = inputValue * inputValue;
                    break;

                case Operation.SquareRoot:
                    if (CurrentResult < 0)
                    {
                        throw new Exception("Going complex? Too bad");
                    }
                    else 
                    {
                        CurrentResult = Math.Sqrt(inputValue);
                    }
                    break;

            }
        }


        public string GetResult(Queue<Token> input)     // reads processed input, calls operation methods
        {
            Stack<double> nums = new Stack<double>();     // numbers from RPN

            while (input.Count() > 0)
            {
                Token currentToken = input.Dequeue();
                if (currentToken is NumberToken numberToken)    // could be done with as but I hate nulls
                {
                    double value = numberToken.Value;
                    nums.Push(value);
                }
                if (currentToken is OperatorToken operatorToken)
                {
                    Operation op = operatorToken.Operation;
                    if (nums.Count() >  0)
                    {
                        if (op == Operation.Square || op == Operation.SquareRoot)
                        {
                            if (nums.Count < 1)
                            {
                                throw new InvalidOperationException("You forgor about the number.");
                            }
                            double operand = nums.Pop();
                            CalculateUnary(op, operand);
                        }

                        else
                        {
                            double num1 = nums.Pop();
                            double num2 = nums.Pop();
                            Calculate(op, num1, num2);
                        }
                        
                        nums.Push(CurrentResult);
                    }
                }
            }
            if (nums.Count != 1)
            {
                throw new InvalidOperationException("Im confused");
            }

            CurrentResult = nums.Pop();
            return CurrentResult.ToString();
        }
    }
}