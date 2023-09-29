using Microsoft.VisualStudio.TestTools.UnitTesting;
using components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using csca5028.lib;
using csca5028.web;
using System.Text.Json;

namespace csca5028.Tests
{
    [TestClass()]
    public class StoreTests
    {
        [TestMethod()]
        public void SendHealthCheckMessageTest()
        {
            //create a store with the name "test", storelocation "test", id 1, and healthcheckurl "test"
            //'New York Store', 'new-york-pos', 5, 1),
            //'123 Main St', 'New York', 'NY', '10001', 'USA', 40.7128, -74.0060
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            store.SendHealthCheckMessage("localhost");

            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                //create a channel to communicate with the rabbitmq server
                using (var channel = connection.CreateModel())
                {
                    var queueName = "new-york-pos";
                    channel.ExchangeDeclare(exchange: "healthcheck", type: ExchangeType.Direct);
                    channel.QueueBind(queue: queueName, exchange: "healthcheck", routingKey: "healthcheck");
                    //declare a queue to send messages to. queue name should be based on the store ID

                    var message = channel.BasicGet(queueName, true);
                    if (message == null)
                    {
                        Assert.Fail();
                    }
                    else
                    {
                        var body = message.Body.ToArray();
                        var messageString = Encoding.UTF8.GetString(body);
                        Assert.AreEqual("PONG", messageString);
                    }


                }
            }
        }

        [TestMethod()]
        public void GenerateSaleTest()
        {
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            //assert that the sale has more than 0 items
            Assert.IsTrue(sale.TotalItems > 0);
            //assert that the sale has a total price greater than 0
            Assert.IsTrue(sale.TotalPrice > 0);
        }

        [TestMethod]
        public void ProcessCardTransactionIntegrationTest()
        {
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                               new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            if (sale.paymentType == Sale.PaymentType.CreditCard)
            {
                csca5028.web.CreditCardProcessor processor = new csca5028.web.CreditCardProcessor("http://localhost:9001", new System.Net.Http.HttpClient());
                var response = store.ProcessCardTransaction(sale, processor);
                Assert.IsTrue((response.ResponseType == CreditCardResponseTypes._0) || (response.ResponseType == CreditCardResponseTypes._1));
                Assert.IsTrue(response.AuthCode.Length == 10);
                Console.WriteLine("Response Received: {0} with code {1}", response.ResponseType.ToString(), response.AuthCode);
            }
            else
            {
                Console.WriteLine("Payment type is not credit card. It was {0}", sale.paymentType.ToString());
            }
        }

        [TestMethod()]
        public void QueueSaleForProcessingTest()
        {
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            var saleJson = Newtonsoft.Json.JsonConvert.SerializeObject(sale);

            store.QueueSaleForProcessing("localhost","test-sale-exchange","test-queue",sale);
            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                //create a channel to communicate with the rabbitmq server
                using (var channel = connection.CreateModel())
                {
                    var queueName = "test-queue";
                    channel.ExchangeDeclare(exchange: "test-sale-exchange", type: "x-consistent-hash", true, false);
                    //bind weight at 10% to the queue
                    channel.QueueBind(queue: queueName, exchange: "test-sale-exchange", "42", new Dictionary<string, object>() { { "x-weight",10} });
                    //declare a queue to send messages to. queue name should be based on the store ID

                    var message = channel.BasicGet(queueName, true);
                    if (message == null)
                    {
                        Assert.Fail();
                    }
                    else
                    {
                        var body = message.Body.ToArray();
                        var messageString = Encoding.UTF8.GetString(body);
                        Assert.AreEqual(saleJson, messageString);
                    }


                }
            }
        }


        [TestMethod]
        public void SaleSerialisationTest()
        {
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            var saleJson = Newtonsoft.Json.JsonConvert.SerializeObject(sale);
            


            var saleDeserialised = Newtonsoft.Json.JsonConvert.DeserializeObject<Sale>(saleJson);
            Assert.AreEqual(sale, saleDeserialised);
        }

        /*[TestMethod()]
        public void ProcessCardTransactionMockTest()
        {
            var mockPaymentService = new Mock<csca5028.TransactionProcessor.CreditCardProcessor>("http://localhost:9001", new System.Net.Http.HttpClient());
            CreditCard card = new CreditCard();
            card.CardExpiry = "12/25";
            card.CardNumber = "4111111111111111";
            card.CardCVV = "123";
            card.Amount = "100";

            CreditCardResponse response = new CreditCardResponse();
            response.ResponseType = CreditCardResponseTypes.AUTH;
            response.AuthCode = "1234567890";
            
            CreditCardProcessor processor = Mock.Of<csca5028.TransactionProcessor.CreditCardProcessor>(p => p.ProcesstransactionAsync(card) == Task.FromResult(response));
            //mockPaymentService.Setup<Task<CreditCardResponse>>(p => p.ProcesstransactionAsync(card)).Returns(Task.FromResult(response));

            //var res = mockPaymentService.Object.ProcesstransactionAsync(card);
            var res = processor.ProcesstransactionAsync(card);
            Assert.IsNotNull(res);
            Assert.AreEqual(CreditCardResponseTypes.AUTH, res.Result.ResponseType);
            Assert.AreEqual("1234567890", res.Result.AuthCode);

            
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                                              new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            var mockResponse = store.ProcessCardTransaction(sale, processor);
            
            
            
        }*/
    }
}