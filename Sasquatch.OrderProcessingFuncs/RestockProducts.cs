using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Sasquatch.Entities;
using Sasquatch.OrderProcessingFuncs.Repositories;

namespace Sasquatch.OrderProcessingFuncs
{
    public static class RestockProducts
    {
        [FunctionName("RestockProducts")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")]string myQueueItem,
            TraceWriter log)
        {
            log.Info($"RestockProducts function triggered.");

            RestockRequest restockRequest = JsonConvert.DeserializeObject<RestockRequest>(myQueueItem);               
            var productRepository = new ProductRepository();
            productRepository.IncrementProductCount(restockRequest.ProductId, restockRequest.RestockQuantity).Wait();            
        }
    }
}
