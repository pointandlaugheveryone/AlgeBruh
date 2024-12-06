namespace AlgeBruh.Models
{
    public class InputHandler   // shunting yard implementation to read input
    {
        private readonly List<char> validChars = 
        ['1','2','3','4','5','6','7','8','9','0','=','+','-','/','*','√','^','.',',']; // TODO: implement parentheses validation
        public Queue<Token> OutputQueue { get; set; } = new Queue<Token>();
        private readonly Dictionary<char, Operation> operatorMap = new Dictionary<char, Operation>
        {
            { '+', Operation.Addition },
            { '-', Operation.Substraction },
            { '*', Operation.Multiplication },
            { '/', Operation.Division },
            { '√', Operation.SquareRoot },
            { '^', Operation.Power }
        };


        public void Validate(string input)
        {
            foreach (char ch in input)
            {
                if(!validChars.Contains(ch))
                {
                    throw new Exception($"character {ch} not recognised.");
                }
            }
        }

        public void ParseInput(string input)
        {
            char[] inputArray = input.Trim().ToCharArray();
            string numberBuffer = String.Empty;
            bool isDecimal = false;

            foreach (char c in inputArray)
            {
                if ((char.IsDigit(c)) || c == ',' || c == '.')  // read decimals, check for invalid decimal point
                {
                    if (c == ',' | c == '.')
                    {
                        if (isDecimal)
                        {
                            throw new Exception("There are two decimal points, you have fat fingers.");
                        }
                    isDecimal = true;
                    }
                    numberBuffer += c;
                }

                else
                {
                    if (!String.IsNullOrEmpty(numberBuffer))
                    {
                        if (double.TryParse(numberBuffer, out double number))
                        {
                            OutputQueue.Enqueue(new NumberToken(number));
                        }
                        else
                        {
                            throw new Exception($"{numberBuffer} is not a valid number... cringe");
                        }
                        numberBuffer = string.Empty;
                        isDecimal = false;
                    }
                    if (operatorMap.TryGetValue(c, out Operation operation))
                    {
                        OutputQueue.Enqueue(new OperatorToken(operation));
                    }
                    else
                    {
                        throw new Exception("Unhandled input, Incident will be reported.");
                    }
                }
            }

            if (!string.IsNullOrEmpty(numberBuffer))
            {
                if (double.TryParse(numberBuffer, out double number))
                {
                    OutputQueue.Enqueue(new NumberToken(number));
                }
                else
                {
                    throw new Exception($"Unhandled input, Incident will be reported.");
                };
            }
        }
    }
}