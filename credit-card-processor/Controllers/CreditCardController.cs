using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if (creditCard == null)
            {
                return BadRequest();
            }

            if (creditCard.CardNumber == null)
            {
                return BadRequest();
            }

            if (creditCard.CardNumber.Length < 16)
            {
                return BadRequest();
            }

            if (creditCard.CardNumber.Length > 16)
            {
                return BadRequest();
            }

            if (creditCard.CardNumber.Substring(0, 1) != "4")
            {
                return BadRequest();
            }

            //return a credit card response with a random 10 character auth code and a random response type with a 0.2 probability of DECLINE
            Random random = new Random();
            CreditCardResponse creditCardResponse = new CreditCardResponse();
            creditCardResponse.AuthCode = Guid.NewGuid().ToString().Substring(0, 10);
            creditCardResponse.ResponseType = random.NextDouble() < 0.2 ? CreditCardResponseTypes.DECLINE : CreditCardResponseTypes.AUTH;

            return Ok(creditCardResponse);
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
