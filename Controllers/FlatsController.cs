using Microsoft.AspNetCore.Mvc;
using Server.Moodle;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        // GET: api/<FlatsController>
        [HttpGet]
        [Route("Get")]
        public List<Flat> Get(int id)
        {
            return Flat.Read(id);
        }
        
        // GET api/<FlatsController>/5
        [HttpGet("{id}")]
        public Flat? GetByFlatId(int id)
        {
            return Flat.ReadByFlatId(id);
        }
        
        [HttpGet]
        [Route("GetByPrice")]
        public IActionResult GetByPrice(double price,int id)
        {
            var flats = Flat.GetByPrice(price,id);

            if (flats.Count == 0)
            {
                return NotFound("No flats found for the given price");
            }

            return Ok(flats);
        }

        [HttpGet]
        [Route("GetByCityRating/rating/{rating}/id/{id}")]
        public ActionResult<List<Flat>> GetByCityRating(float rating,int id)
        {
            var flats = Flat.GetByCityRating(rating,id);

            if (flats.Count == 0)
            {
                return NotFound("No flats found for the given rating");
            }

            return flats;
        }

        // POST api/<FlatsController>
        [HttpPost ]
        public bool Post([FromBody] Flat flat)
        {
            return flat.Insert();
        }

        // PUT api/<FlatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


        // need to check we send email to benny
        // DELETE api/<FlatsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var f = Flat.DeleteById(id);
                return Ok(f);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
     
    }
}
