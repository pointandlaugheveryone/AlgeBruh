namespace AlgeBruh.Models
{
    public abstract class Token {}

    public class NumberToken : Token
    {
        public double Value {get;}
        public NumberToken(double value)
        {
            Value = value;
        }
    }

    public class OperatorToken : Token
    {
        public Operation Operation {get;}
        public OperatorToken(Operation operation)
        {
            Operation = operation;
        }
    }
}