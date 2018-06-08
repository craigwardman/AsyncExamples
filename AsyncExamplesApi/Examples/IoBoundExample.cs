using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncExamplesApi.Examples.lib;

namespace AsyncExamplesApi.Examples
{
    /// <summary>
    /// IO bound async calls free up the ASP.NET thread to process other calls while waiting IO completion on slow resources such as network, disk, etc.
    /// </summary>
    public class IoBoundExample
    {
        private int iterations = 5;

        /// <summary>
        /// Awaiting each IO bound operation in sequence is wasting time when the resource is under-utilised
        /// </summary>
        /// <returns></returns>
        public Task<TimeSpan> GetMany_AwaitingEach_Bad()
        {
            var resourceGetter = new SomeAsyncResource();

            return TimedExecution(async () =>
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        await resourceGetter.GetStringContentAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        /// <summary>
        /// Fire off multiple requests and await IO completion in parallel.
        /// However be careful not to overload the remote resource or the local client machine (each tasks uses memory, disk/network etc.)
        /// Considering batching.
        /// </summary>
        /// <returns></returns>
        public Task<TimeSpan> GetMany_AwaitingAll_IOBound_Good()
        {
            var resourceGetter = new SomeAsyncResource();

            return TimedExecution(async () =>
                {
                    var tasks = new List<Task<string>>();

                    for (int i = 0; i < iterations; i++)
                    {
                        tasks.Add(resourceGetter.GetStringContentAsync());
                    }

                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
            );
        }

        private async Task<TimeSpan> TimedExecution(Func<Task> a)
        {
            var sw = new Stopwatch();
            sw.Start();
            await a().ConfigureAwait(false);
            sw.Stop();

            return sw.Elapsed;
        }
    }
}