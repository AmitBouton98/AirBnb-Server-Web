using Microsoft.AspNetCore.Mvc;
using Server.Moodle;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET: api/<OrderController>
        [HttpGet]
        public List<Order> Get()
        {
            return Order.Read();
        }
        // GET: api/<OrderController>
        [HttpGet]
        [Route("GetByUserId")]
        public List<Order> GetByUserId(int userId)
        {
            return Order.readByUser(userId);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public Order? Get(int id)
        {
            return Order.Read(id);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            try
            {
                bool orderStatus = order.Insert();
                if (orderStatus)
                {
                    // return a success 
                    //return Ok("Order successful");
                    return StatusCode(StatusCodes.Status201Created, new { message = "Order successful" });
                }
                else
                {
                    // return a failure
                    return BadRequest( "Order failed");
                }
            }
            catch (Exception ex)
            {
                // return an error 
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("Put")]
        public IActionResult Put([FromBody] Order order)
        {
            try
            {
                var user = Order.Update(order);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = Order.Delete(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
