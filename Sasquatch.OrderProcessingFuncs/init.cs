using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Sasquatch.OrderProcessingFuncs.Repositories;

namespace Sasquatch.OrderProcessingFuncs
{
    public static class init
    {
        /// <summary>
        /// This function creates several backend repository components. This
        /// includes the database, collection and some sample documents to use.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("init")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Init function triggered");

            // Instatiate an instance of the repository and
            // initialize the components.
            var productRepository = new ProductRepository();
            await productRepository.Initialize();

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
