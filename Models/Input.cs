namespace AlgeBruh.Models
{
    public class InputHandler   // shunting yard implementation to read input
    {
        private readonly List<char> validChars = 
        ['1','2','3','4','5','6','7','8','9','0','=','+','-','/','*','√','^','.',',','(',')','²'];
        public Queue<Token> OutputQueue { get; set; } = new Queue<Token>();
        public Stack<char> OperatorStack {get; set;} = new Stack<char>();
        private readonly Dictionary<char, Operation> operatorMap = new Dictionary<char, Operation>
        {
            { '+', Operation.Addition },
            { '-', Operation.Substraction },
            { '*', Operation.Multiplication },
            { '/', Operation.Division },
            { '²', Operation.Square },
            { '√', Operation.SquareRoot },
            { '^', Operation.Power }
        };
        private readonly Dictionary<char, int> precedence = new Dictionary<char, int>
        {  
            { '-', 1 },
            { '+', 2 },
            { '/', 3 },
            { '*', 4 },
            { '√', 5 },
            { '^', 6 },
        };      // tracks operator precedence, 
        private readonly Dictionary<char, bool> rightAssociative = new Dictionary<char, bool>
        {
            { '^', true },
            { '√', true } 
        };


        public void Validate(string input)
        {
            int balance = 0;    // tracks number of parentheses
            foreach (char c in input)
            {
                if (c == '(') balance++;
                else if (c == ')') balance--;

                if (balance < 0)
                    throw new Exception("Mismatched parentheses.");

                if (!validChars.Contains(c))
                    throw new Exception($"Invalid character: {c}");
            }

            if (balance != 0)
                throw new Exception("Mismatched parentheses.");
        }

        public void Postfix(string input)
        {
            char[] inputArray = input.Trim().ToCharArray();
            string numberBuffer = String.Empty;
            bool isDecimal = false;

            foreach (char c in inputArray)
            {
                if (char.IsDigit(c) || c == ',' || c == '.')
                {
                    if (c == ',' || c == '.')
                    {
                        if (isDecimal)
                            throw new Exception("Multiple decimal points...");
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
                    if (!string.IsNullOrEmpty(numberBuffer))
                    {
                        EnqueueNum(numberBuffer);
                        numberBuffer = string.Empty;
                        isDecimal = false;
                    }

                    while (OperatorStack.Count > 0 && OperatorStack.Peek() != '(')
                    {
                        EnqueueOp(OperatorStack.Pop());
                    }

                    if (OperatorStack.Count == 0 || OperatorStack.Pop() != '(')
                        throw new Exception("Mismatched parentheses.");
                }
                else if (operatorMap.ContainsKey(c))
                {
                    if (!string.IsNullOrEmpty(numberBuffer))
                    {
                        EnqueueNum(numberBuffer);
                        numberBuffer = string.Empty;
                        isDecimal = false;
                    }

                    while (OperatorStack.Count > 0 && OperatorStack.Peek() != '(' &&
                           (precedence[OperatorStack.Peek()] > precedence[c] ||
                           (precedence[OperatorStack.Peek()] == precedence[c] && !rightAssociative.ContainsKey(OperatorStack.Peek()))))
                    {
                        EnqueueOp(OperatorStack.Pop());
                    }
                    OperatorStack.Push(c);
                }
                else
                {
                    throw new Exception($"Invalid input {c}.");
                }
            }

            if (!string.IsNullOrEmpty(numberBuffer))
            {
                EnqueueNum(numberBuffer);
            }

            while (OperatorStack.Count > 0)
            {
                char op = OperatorStack.Pop();
                if (op == '(' || op == ')')
                    throw new Exception("Mismatched parentheses.");
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
                throw new Exception($"Not a valid number.");
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