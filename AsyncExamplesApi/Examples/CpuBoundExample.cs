using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncExamplesApi.Examples.lib;

namespace AsyncExamplesApi.Examples
{
    /// <summary>
    /// CPU bound code is code that makes use of the CPU and so can benefit from multiple threads for fastest execution.
    /// From a performance point of view, there is no benefit to yielding execution if you are aiming for maximum CPU usage
    /// However, if you don't plan on hitting max CPU the entire time then you could wrap it in an async/await to allow other code some thread time
    /// (such as some other async code)
    /// </summary>
    public class CpuBoundExample
    {
        private int iterations = 5;

        /// <summary>
        /// Standard C#, loop over a collection and do some work on them.
        /// This will all run on the request thread, so limited CPU cores and hogs ASP.NET request
        /// For a single iteration, you could consider at least moving it onto another thread (Task.Run) and awaiting it - to free up the main ASP.NET thread
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalculateMany_NoParallel_Bad()
        {
            var calculator = new SomeCalculation();

            return TimedExecution(() =>
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        calculator.Calculate(i);
                    }
                }
            );
        }

        /// <summary>
        /// You could use a single task to free up the main thread - but no benefit of parallelisation
        /// or multiple tasks using Task.Run() - however this suffers even more from resource starvation than IO bound tasks.
        /// The best option is to use the task parallel library of functions which uses optimal partitioning of threads/workload size
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalculateMany_UsingTPL_Good()
        {
            var calculator = new SomeCalculation();

            return TimedExecution(() => 
            {
                Parallel.For(0, iterations, i => // Parallel library is better than Task.Run() for this as it will use intelligent partitioning
                {
                    calculator.Calculate(i);
                });
            }
            );
        }

        private TimeSpan TimedExecution(Action a)
        {
            var sw = new Stopwatch();
            sw.Start();
            a();
            sw.Stop();

            return sw.Elapsed;
        }
    }
}