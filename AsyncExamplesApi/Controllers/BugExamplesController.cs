using AsyncExamplesApi.Examples;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncExamplesApi.Controllers
{
    [RoutePrefix("BugExamples")]
    public class BugExamplesController : ApiController
    {
        [Route(nameof(ReturnNull_BadExample))]
        [HttpGet]
        public async Task<string> ReturnNull_BadExample()
        {
            var nullTaskExample = new ReturnNullExample();

            var badMessage = await nullTaskExample.GetSomethingBadAsync(); // null reference exeption here, since the task is null

            return badMessage;
        }

        [Route(nameof(ReturnNull_GoodExample))]
        [HttpGet]
        public async Task<string> ReturnNull_GoodExample()
        {
            var nullTaskExample = new ReturnNullExample();

            var goodMessage = await nullTaskExample.GetSomethingGoodAsync(); // message will be null, which is expected

            return goodMessage;
        }

        [Route(nameof(DeadlockExample))]
        [HttpGet]
        public string DeadlockExample()
        {
            var deadlockExample = new DeadlockExample();

            var badMessage = deadlockExample.GetSomethingAsync_BadDotResult(); // this will never return

            return badMessage;
        }

        [Route(nameof(DeadlockExample_FixUsingConfigureAwait))]
        [HttpGet]
        public string DeadlockExample_FixUsingConfigureAwait()
        {
            var deadlockExample = new DeadlockExample();

            var goodMessage = deadlockExample.GetSomethingAsync_GoodDotResult(); // this is now synchronous and doesn't deadlock

            return goodMessage;
        }

        [Route(nameof(DeadlockExample_FixUsingTaskRun))]
        [HttpGet]
        public string DeadlockExample_FixUsingTaskRun()
        {
            var deadlockExample = new DeadlockExample();

            var goodMessage = deadlockExample.GetSomethingAsync_UsingTaskAndDotResult(); // this is now synchronous and doesn't deadlock

            return goodMessage;
        }
    }
}