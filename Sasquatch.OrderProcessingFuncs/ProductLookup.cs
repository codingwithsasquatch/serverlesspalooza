using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Sasquatch.Entities;
using Sasquatch.OrderProcessingFuncs.Models;
using Sasquatch.OrderProcessingFuncs.Repositories;

namespace Sasquatch.OrderProcessingFuncs
{
    public static class ProductLookup
    {
        /// <summary>
        /// This function performs a simple lookup of the inventory
        /// count of a product.
        /// </summary>
        /// <param name="req">Incoming request</param>
        /// <param name="log">Log diagnostics</param>
        /// <returns>The inventory count of the requested product.</returns>
        [FunctionName("ProductLookup")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Product Lookup function triggered.");

            // Retrieve the product ID from the query string, if it's not
            // passed in then return a bad request response.
            var productId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "productid", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            if (string.IsNullOrEmpty(productId))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass the product ID in the query string.");

            // Retrieve the product
            var repository = new ProductRepository();
            var product = repository.GetProductById(productId);

            // Return Not Found if we can't find the product, otherwise
            // return an OK and the product information.
            return product == null ? req.CreateResponse(HttpStatusCode.NotFound) : req.CreateResponse(HttpStatusCode.OK, product);
        }
    }
}
