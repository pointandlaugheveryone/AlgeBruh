namespace AlgeBruh.Models
{
    class Calculator
    {
        private readonly InputHandler _input;
        public double CurrentResult {get; private set;} = 0;


        public Calculator(InputHandler inputHandler)
        {
            _input = inputHandler;
        }



        private double Calculate(Operation operation, double value1, double value2)
        {
            switch (operation)
            {
                case Operation.Addition:
                    return value1 + value2;
                
                case Operation.Substraction:
                    return value1 - value2;
                
                case Operation.Multiplication:
                    return value1 * value2;

                case Operation.Division:
                    if (value1 == 0)
                        throw new DivideByZeroException("No further steps can be trusted.");
                    return value1 / value2;

                case Operation.Power:
                    return Math.Pow(value1, value2);

                default:
                   throw new InvalidOperationException("Unhandled error.");
            }
        }

        private double CalculateUnary(Operation operation, double value)
        {
            switch (operation)
            {
                case Operation.Square:
                    return value * value;

                case Operation.SquareRoot:
                    if (value < 0)
                    {
                        throw new Exception("Going complex? Too bad");
                    }
                    else 
                    {
                        return Math.Sqrt(value);
                    }

                default:
                    throw new InvalidOperationException("Unhandled error.");
            }
        }


        private double Process(Queue<Token> input)     // reads processed input, calls operation methods
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
                            double result = CalculateUnary(op, operand);
                            nums.Push(result);
                        }

                        else
                        {
                            if (nums.Count < 2)
                            {
                                throw new InvalidOperationException("not enough numbers.");
                            }
                            
                            double num2 = nums.Pop();
                            double num1 = nums.Pop();
                            double result = Calculate(op, num1, num2);
                            nums.Push(result);
                        }
                    }
                }
            }
            if (nums.Count != 1)
            {
                throw new InvalidOperationException("Expression could not be deciphered.");
            }

            CurrentResult = nums.Pop();
            return CurrentResult;
        }

        public string GetResult(string rawInput)
        {
            Queue<Token> tokens = _input.ParseInput(rawInput);
            double result = Process(tokens);
            return result.ToString();
        }
    }
}