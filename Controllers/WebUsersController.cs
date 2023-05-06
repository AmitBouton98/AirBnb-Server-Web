using Microsoft.AspNetCore.Mvc;
using Server.Moodle;
using Server.Moodle.DAL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebUsersController : ControllerBase
    {
        // GET: api/<WebUsersController>
        [HttpGet]
        public List<WebUser> Get()
        {
            return WebUser.Read();
        }
        // GET: api/<WebUsersController>
        [HttpGet]
        [Route("GetById")]
        public WebUser? GetById(string id)
        {
            return WebUser.GetById(id);
        }
        [HttpGet]
        [Route("CheckIfKeyCorrect")]
        public IActionResult CheckIfKeyCorrect(string key,string id)
        {
            try
            {
                return Ok(WebUser.checkIfKeyCorrect(key, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        [Route("GetByemail/email/{email}")]
        public async Task<IActionResult> GetByemail(string email)
        {
            try 
            {
                var usr = await WebUser.GetByemail(email);
                return Ok(usr);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // POST api/<WebUsersController>
        [HttpPost]
        public IActionResult Registration([FromBody]WebUser user)
        {
            try
            {
                bool registrationStatus = user.Registration();
                if (registrationStatus)
                {
                    // return a success 
                    DBservices dBservices = new DBservices();
                    WebUser usr = dBservices.GetByemail(user.Email);
                    return Ok(usr);
                }
                else
                {
                    // return a failure
                    return BadRequest( "User registration failed" );
                }
            }
            catch (Exception ex)
            {
                // return an error 
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        /*
        // POST api/<WebUsersController>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string id, string key, string newPassword)
        {
            var user = WebUser.GetById(id);
            if (user == null)
            {
                return BadRequest("There is no such account" );
            }
            bool resetpassCheck = await user.resetPassword(key, newPassword);
            if (resetpassCheck)
            {
                return Ok(resetpassCheck);
            }
            else
            {
                return BadRequest( "key is wrong" );
            }
        }
        */
        // POST api/<WebUsersController>
        [HttpPost]
        //[Route("LogInPost/{email}/{password}")]
        [Route("LogInPost")]
        public IActionResult LogInPost(string email, string password)
        {
            try
            {
                var user = WebUser.LogInPost(email,password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message );
            }
        }
        // PUT api/<WebUsersController>/5
        [HttpPut("Put")]
        public IActionResult Put(WebUser user)
        {
            try
            {
                var u = WebUser.Update(user);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message );
            }
        }
        // DELETE api/<WebUsersController>/5
        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            try
            {
                var u = WebUser.Delete(email);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

