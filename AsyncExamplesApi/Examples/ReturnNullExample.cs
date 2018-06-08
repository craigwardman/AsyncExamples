using System.Threading.Tasks;
using AsyncExamplesApi.Examples.lib;

namespace AsyncExamplesApi.Examples
{
    /// <summary>
    /// Note that only applies when you are not using async/await - these keywords will convert "return null" for you
    /// but you shouldn't always use them, as in the example case, it would be wasted usage/overhead when the method doesn't care about the result
    /// </summary>
    public class ReturnNullExample
    {
        private bool shouldMakeTheCall = false;

        /// <summary>
        /// This usually appears after re-factoring something that didn't used to be async, when "returning null" was the correct syntax to return a null result.
        /// </summary>
        /// <returns></returns>
        public Task<string> GetSomethingBadAsync()
        {
            if (shouldMakeTheCall) // e.g. some logical decision whether to call the external resource
            {
                return new SomeAsyncResource().GetStringContentAsync();
            }
            else
            {
                return null; // verbatim "return null" returns a null Task, not a Task with a null value
            }
        }

        /// <summary>
        /// In this version you return a completed task with a value of null.
        /// </summary>
        /// <returns></returns>
        public Task<string> GetSomethingGoodAsync()
        {
            if (shouldMakeTheCall) // e.g. some logical decision whether to call the external resource
            {
                return new SomeAsyncResource().GetStringContentAsync();
            }
            else
            {
                return Task.FromResult<string>(null); // this is now a Task with a null string
            }
        }
    }
}