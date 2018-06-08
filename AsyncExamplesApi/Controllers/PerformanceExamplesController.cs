using System;
using System.Threading.Tasks;
using System.Web.Http;
using AsyncExamplesApi.Examples;

namespace AsyncExamplesApi.Controllers
{
    [RoutePrefix("PerformanceExamples")]
    public class PerformanceExamplesController : ApiController
    {
        [Route(nameof(GetMany_AwaitingEach_IOBound_Bad))]
        [HttpGet]
        public async Task<TimeSpan> GetMany_AwaitingEach_IOBound_Bad()
        {
            var ioBoundExample = new IoBoundExample();

            var timeTaken = await ioBoundExample.GetMany_AwaitingEach_Bad(); // this works but could be faster

            return timeTaken;
        }

        [Route(nameof(GetMany_AwaitingAll_IOBound_Good))]
        [HttpGet]
        public async Task<TimeSpan> GetMany_AwaitingAll_IOBound_Good()
        {
            var ioBoundExample = new IoBoundExample();

            var timeTaken = await ioBoundExample.GetMany_AwaitingAll_IOBound_Good(); // this is now allows the IO latency effect to be reduced

            return timeTaken;
        }

        [Route(nameof(CalculateMany_NoParallel_Bad))]
        [HttpGet]
        public TimeSpan CalculateMany_NoParallel_Bad()
        {
            var cpuBoundExample = new CpuBoundExample();

            var timeTaken = cpuBoundExample.CalculateMany_NoParallel_Bad(); // this hogs the request thread and doesn't make use of the full CPU

            return timeTaken;
        }

        [Route(nameof(CalculateMany_UsingTpl_Good))]
        [HttpGet]
        public TimeSpan CalculateMany_UsingTpl_Good()
        {
            var cpuBoundExample = new CpuBoundExample();

            var timeTaken = cpuBoundExample.CalculateMany_UsingTPL_Good(); // will use the CPU across multiple threads

            return timeTaken;
        }
    }
}