using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWebApiNet6.Service.Redis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectWebApiNet6.Controllers
{
    /// <summary>
    /// 测试Controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// GET: api ValuesController.GetEformEdrmsBorrowInfo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Getxxy()
        {
            RedisService redisService = new RedisService();
            
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/ValuesController/5
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST api/ValuesController
        /// </summary>
        /// <param name="value">值</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// PUT api/ValuesController/5
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="value">值</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// DELETE api/ValuesController/5
        /// </summary>
        /// <param name="id">编号</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
