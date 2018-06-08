using System.Threading.Tasks;

namespace AsyncExamplesApi.Examples.lib
{
    public class SomeAsyncResource
    {
        public async Task<string> GetStringContentAsync()
        {
            await Task.Delay(500).ConfigureAwait(false); // pretend resource latency

            return "Here is a string";
        }
    }
}