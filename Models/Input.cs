namespace AlgeBruh.Models
{
    public class InputHandler
    {
        private readonly List<char> validChars = ['1','2','3','4','5','6','7','8','9','0','=','+','-','/','*','√','^','.',','];

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

        //splitting nums and operations
    }
}