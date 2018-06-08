using System.Threading.Tasks;
using AsyncExamplesApi.Examples.lib;

namespace AsyncExamplesApi.Examples
{
    /// <summary>
    /// Deadlocks are usually caused by the usage of blocking calls, such as .Result in conjunction with synchronisation context affinity (ie. not using ConfigureAwait(false))
    /// </summary>
    public class DeadlockExample
    {
        /// <summary>
        /// This is what happens when you combine the use .Result with a missing ConfigureAwait(false)
        /// </summary>
        /// <returns></returns>
        public string GetSomethingAsync_BadDotResult()
        {
            // we can't use .Result here since the at least 1 async call in the call stack doesn't use "ConfigureAwait(false)"
            return SomeExternalCall_WithoutConfigureAwaitFalse().Result;
        }

        /// <summary>
        /// Here .Result does not deadlock as the ConfigureAwait(false) means it does not need to return to the main thread (which is blocked waiting for this task)
        /// You should be very confident that the entire call stack below this point uses ConfigureAwait(false) - it only takes a single missing one to assume context affinity
        /// </summary>
        /// <returns></returns>
        public string GetSomethingAsync_GoodDotResult()
        {
            // we can use .Result here since the entire async call stack uses "ConfigureAwait(false)"
            return SomeExternalCallWithConfigureAwaitFalse().Result;
        }

        /// <summary>
        /// Use this when you don't have confidence in, or know for a fact, the ConfureAwait(false) rule hasn't been followed
        /// By placing the call stack into a Task.Run it will be off the main thread with no context to synchronise back to
        /// </summary>
        /// <returns></returns>
        public string GetSomethingAsync_UsingTaskAndDotResult()
        {
            // only use this if you don't have control over applying "ConfigureAwait(false)" down the call stack
            return Task.Run(async () => await SomeExternalCall_WithoutConfigureAwaitFalse()).Result;
        }

        private async Task<string> SomeExternalCall_WithoutConfigureAwaitFalse()
        {
            return await new SomeAsyncResource().GetStringContentAsync();
        }

        private async Task<string> SomeExternalCallWithConfigureAwaitFalse()
        {
            return await new SomeAsyncResource().GetStringContentAsync().ConfigureAwait(false);
        }
    }
}