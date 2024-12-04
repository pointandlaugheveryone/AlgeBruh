using System;

namespace AlgeBruh.Models
{
    class Calculation
    {
        public double CurrentResult {get; private set;} = 0;

        public void Arithmetic(Operation operation, double inputValue)
        {
            switch (operation)
            {
                case Operation.Addition:
                    CurrentResult += inputValue;
                    break;
                
                case Operation.Substraction:
                    CurrentResult -= inputValue;
                    break;
                
                case Operation.Multiplication:
                    CurrentResult *= inputValue;
                    break;

                case Operation.Division:
                    if (inputValue == 0)
                        throw new DivideByZeroException("No further steps can be trusted.");
                    CurrentResult /= inputValue;
                    break;

                default:
                   throw new InvalidOperationException("Unhandled error.");
            }

            
        }
    }
}