namespace AlgeBruh.Models
{
    public class InputHandler   // shunting yard implementation to read input
    {
        private readonly List<char> validChars = 
        ['1','2','3','4','5','6','7','8','9','0','=','+','-','/','*','√','^','.',',','(',')'];
        public Queue<Token> OutputQueue { get; set; } = new Queue<Token>();
        public Stack<char> OperatorStack {get; set;} = new Stack<char>();
        private readonly Dictionary<char, Operation> operatorMap = new Dictionary<char, Operation>
        {
            { '+', Operation.Addition },
            { '-', Operation.Substraction },
            { '*', Operation.Multiplication },
            { '/', Operation.Division },
            { '√', Operation.SquareRoot },
            { '^', Operation.Power }
        };
        private readonly Dictionary<char, int> precedence = new Dictionary<char, int>
        {
            { '+', 2 },
            { '-', 2 },
            { '*', 3 },
            { '/', 3 },
            { '^', 4 },
            { '√', 4 } 
        };
        private readonly Dictionary<char, bool> rightAssociative = new Dictionary<char, bool>
        {
            { '^', true },
            { '√', true } 
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

        public void Postfix(string input)
        {
            char[] inputArray = input.Trim().ToCharArray();
            string numberBuffer = String.Empty;
            bool isDecimal = false;

            foreach (char c in inputArray)
            {
                // operator handling
                if ((char.IsDigit(c)) || c == ',' || c == '.')  // read decimals, check for invalid decimal point
                {
                    if (c == ',' || c == '.')
                    {
                        if (isDecimal)
                        {
                            throw new Exception("There are two decimal points, you have fat fingers.");
                        }
                    isDecimal = true;
                    }
                    numberBuffer += c;
                }

                else if (c == '(')
                {
                    OperatorStack.Push(c);
                }

                else if (c == ')')
                {
                    while (OperatorStack.Count > 0 && OperatorStack.Peek() != '(')
                    {
                        EnqueueOp(OperatorStack.Pop());
                    }

                    if ((OperatorStack.Count == 0) || (OperatorStack.Pop() != '('))
                    {
                        throw new Exception("Invalid parentheses.");
                    }
                }

                else if (operatorMap.ContainsKey(c))
                {
                    while (
                        (OperatorStack.Count > 0) && 
                        (OperatorStack.Peek() != '(') && 
                        (precedence[OperatorStack.Peek()] > precedence[c]) &&
                        (precedence[OperatorStack.Peek()] == precedence[c]) &&
                        (!rightAssociative.ContainsKey(c)) &&
                        (!rightAssociative.ContainsKey(c))
                        )
                    {
                        EnqueueOp(OperatorStack.Pop());
                    }
                    OperatorStack.Push(c);
                }

                else
                {
                    throw new Exception($"Invalid input {c}. Incident will be reported.");
                }

                // if operator or parantheses encountered, enqueue current number
                if (!char.IsDigit(c) && c != ',' && c != '.')
                {
                    EnqueueNum(numberBuffer);
                    numberBuffer = string.Empty;    // reset variables
                    isDecimal = false;
                }
            }

            // handle remaining characters
            if (!string.IsNullOrEmpty(numberBuffer))    
            {
                EnqueueNum(numberBuffer);
            }
        
            while (OperatorStack.Count > 0)
            {
                char op = OperatorStack.Pop();
                if (op == '(' || op == ')')
                {
                    throw new Exception("Mismatched parentheses, dumbass.");
                }
                EnqueueOp(op);
            }
        }

        public void EnqueueOp(char c)
        {
            if (operatorMap.TryGetValue(c, out Operation operation))
            {
                OutputQueue.Enqueue(new OperatorToken(operation));
            }
        }

        public void EnqueueNum(string buffer)
        {
            if (double.TryParse(buffer, out double number))
            {
                OutputQueue.Enqueue(new NumberToken(number));
            }
            else 
            {
                throw new Exception($"{buffer} does NOT counts as a number.");
            }
        }

        public Queue<Token> ParseInput(string input) 
        {
            Validate(input);
            Postfix(input);
            return OutputQueue;
        }
    }
}