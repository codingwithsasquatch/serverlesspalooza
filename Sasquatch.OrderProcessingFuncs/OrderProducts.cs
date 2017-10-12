using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Sasquatch.Entities;
using Sasquatch.OrderProcessingFuncs.Repositories;

namespace Sasquatch.OrderProcessingFuncs
{
    public static class OrderProducts
    {
        [FunctionName("OrderProducts")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Order Products function triggered.");

            // Retrieve the order request details
            var order = await req.Content.ReadAsAsync<OrderRequest>();
            if (order == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Missing order details.");
            }

            // Retrieve the product
            var productRepository = new ProductRepository();
            var product = productRepository.GetProductById(order.Item.ProductId);

            // If the product does not exist return a meaningful response
            if (product == null)
            {
                var response = new OrderResponse {OrderStatus = "Unsuccessful", Message = "Product not found"};
                return req.CreateResponse(HttpStatusCode.OK, response);
            }

            //  Check to see if there is enough inventory
            var orderResponse = new OrderResponse() {OrderStatus = "Success"};
            if (product.Count >= order.Item.Quantity)
            {
                orderResponse.Message = "Your product will be shipped within 5 business days.";
                // TODO: Send an email 
            }
            else
            {
                orderResponse.Message = "Your product will be shipped within 10 business days.";
                // TODO: Send an email
                // TODO: Place an item into a queue to manufacture more products.
            }

            // Decrement the inventory count
            productRepository.UpdateProductCount(product.Id, order.Item.Quantity).Wait();

            return req.CreateResponse(HttpStatusCode.OK, orderResponse);
        }


    }
}
