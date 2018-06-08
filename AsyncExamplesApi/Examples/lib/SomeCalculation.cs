namespace AsyncExamplesApi.Examples.lib
{
    public class SomeCalculation
    {
        public int Calculate(int seed)
        {
            int theAnswer = seed;
            for (int i = 0; i <= 500; i++)
            {
                for (int x = 0; x <= 500; x++)
                {
                    for (int y = 0; y <= 500; y++)
                    {
                        var something = i + x + y;
                        var somethingElse = something % 2;
                        theAnswer += somethingElse;
                    }
                }
            }

            return theAnswer;
        }
    }
}