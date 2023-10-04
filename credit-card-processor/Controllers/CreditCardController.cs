using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics;

namespace credit_card_processor.Controllers
{
    [Route("api/[controller]/processtransaction")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CreditCardResponse> Post([FromBody] CreditCard creditCard)
        {
            //add prometheus metrics here to measure the number of requests to this endpoint
            //add prometheus metrics here to measure the number of requests to this endpoint that are declined
            //add prometheus metrics here to measure the number of requests to this endpoint that are authorized
            var counter = Metrics.CreateCounter("credit_card_processor_requests", "Number of requests to the credit card processor endpoint");
            counter.Inc();

            var declinedCounter = Metrics.CreateCounter("credit_card_processor_declined_requests", "Number of requests to the credit card processor endpoint that are declined");
            var authorizedCounter = Metrics.CreateCounter("credit_card_processor_authorized_requests", "Number of requests to the credit card processor endpoint that are authorized");
            var txnProcessingSummary = Metrics.CreateGauge("credit_card_processor_txn_processing_ms", "Time (in ms) it takes to process a transaction");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!IsCreditCardValid(creditCard))
            {
                return BadRequest();
            }

            //return a credit card response with a random 10 character auth code and a random response type with a 0.2 probability of DECLINE
            Random random = new Random();
            CreditCardResponse creditCardResponse = new CreditCardResponse();
            creditCardResponse.AuthCode = Guid.NewGuid().ToString().Substring(0, 10);
            creditCardResponse.ResponseType = random.NextDouble() < 0.2 ? CreditCardResponseTypes.DECLINE : CreditCardResponseTypes.AUTH;
            if (creditCardResponse.ResponseType == CreditCardResponseTypes.AUTH)
            {
                authorizedCounter.Inc();
            }
            else
            {
                declinedCounter.Inc();
            }
            stopwatch.Stop();
            txnProcessingSummary.Set(stopwatch.ElapsedMilliseconds);
            return Ok(creditCardResponse);
        }

        private bool IsCreditCardValid(CreditCard creditCard)
        {
            if (creditCard == null)
            {
                return false;
            }
            if (creditCard.CardNumber == null)
            {
                return false;
            }

            if (creditCard.CardNumber.Length < 16)
            {
                return false;
            }

            if (creditCard.CardNumber.Length > 16)
            {
                return false;
            }

            if (creditCard.CardNumber.Substring(0, 1) != "4")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public enum CreditCardResponseTypes
    {
        AUTH,
        DECLINE,
    }

    public class CreditCardResponse
    {
        public CreditCardResponseTypes ResponseType { get; set; }
        public string? AuthCode { get; set; }
    }
}
