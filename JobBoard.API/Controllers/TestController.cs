using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Job Board API is working!",
                Status = "Success"
            });
        }

        
    }
}
