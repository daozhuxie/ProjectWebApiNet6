using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectWebApi.Configuration;
using ProjectWebApi.Model;
using ProjectWebApiNet5.Configuration;
using ProjectWebApiNet5.Model.Public;
using ProjectWebApiNet5.Service;
using ProjectWebApiNet5.Service.LogMessages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectWebApiNet5.Controllers
{
    /// <summary>
    /// 测试Controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private String _StrValue = null;//全局值
        //private int _InCon = 0;//全局值
        private DataTable _dt;//全局dt
        //private string _StrTime;// 时间提示
        private LogMessagesService _logmessagesservice;
        private ValuesService _valuesservice;
        private readonly ILogger<LogHelper> _logger;
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="logmessagesservice">日志类，往表记录</param>
        /// <param name="valuesservice">测试类</param>
        /// <param name="logger">日志类，往Text文件输入</param>
        public TestController(IOptions<LogMessagesService> logmessagesservice, IOptions<ValuesService> valuesservice, ILogger<LogHelper> logger)
        {
            _logmessagesservice = logmessagesservice.Value;
            _valuesservice = valuesservice.Value;
            _logger = logger;
        }

        /// <summary>
        /// GetTest_测试接口通
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataResult<Object> GetTest()
        {
            DataResult<Object> result = new DataResult<object>();
            result.Data =  "测试接口调用成功放回Jons 接口测试成功";
            //NLogHelper.Default.Info("Hello NLogInfo");// 报错

            LogFactory loggerf = new LogFactory("/logs"); // route指自定义文件夹名字         
            loggerf.Info("Log:Startup/ConfigureServices: 方法调用成功-01");  //记录输入的请求的参数

            LogHelper.TextInfo("Info: GetEformEdrmsBorrowInfo-方法调用成功-02");
            LogHelper.ConsoleWriteLine();
            //Console.WriteLine("消息弹窗：不带标签：ConsoleWriteLine");

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Hello Info");
            logger.Error("Hello Error");
            NLog.Logger _loggers = NLog.LogManager.GetCurrentClassLogger();
            _loggers.Info("NLog.Logger Info");

            _logger.LogError(" LogError: GetEformEdrmsBorrowInfo-方法调用成功1");// 错误
            _logger.LogDebug(" LogDebug: GetEformEdrmsBorrowInfo-方法调用成功2");//调试 -- 捕捉不到
            _logger.LogTrace(" LogTrace: GetEformEdrmsBorrowInfo-方法调用成功3");//查出 -- 捕捉不到
            _logger.LogWarning(" LogWarning: GetEformEdrmsBorrowInfo-方法调用成功4");//警告
            _logger.LogCritical(" LogCritical: GetEformEdrmsBorrowInfo-方法调用成功5");//批评的
            _logger.LogInformation(" LogInformation: GetEformEdrmsBorrowInfo-方法调用成功6");//信息
            //LogHelperText log2 = new LogHelperText();
            //LogHelperText.Info("Message: GetEformEdrmsBorrowInfo-方法调用成功");
            //LogHelperText.Error("Message: GetEformEdrmsBorrowInfo-方法调用成功");
            //LogHelperText.Debug("Message: GetEformEdrmsBorrowInfo-方法调用成功");


            return result;
        }

        /// <summary>
        /// Request Payload 传参 - 带必传值限制
        /// </summary>
        /// <param name="tagetArchType">类型</param>
        /// <param name="modelCode">编码</param>
        /// <param name="uniqueCode">上级编码</param>
        /// <param name="input">信息</param>
        /// <returns></returns>
        [HttpPost("{tagetArchType}/{modelCode}/{uniqueCode}")]
        public IActionResult GetParams(int tagetArchType, string modelCode, string uniqueCode, [FromBody] DBTableAddress input)
        {
            var res = new DataResult<string>();
            res.ResultCode = 0;
            res.Message = $"GetParams - 当前时间{DateTime.Now}，测试成功！";
            res.Data = $"传参：tagetArchType={tagetArchType}|modelCode={modelCode}|uniqueCode={uniqueCode}";
            res.DataDescription = JsonConvert.SerializeObject(input);
            return Ok(res);
        }

        /// <summary>
        /// 测试-获取数据库列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public DataResult<object> GetEformEdrmsBorrowInfo()
        {
            DataResult<object> result = new DataResult<object>();
            _dt = _valuesservice.GetEformEdrmsBorrowInfo();
            result.Data = JsonHelper.SerializeObject(_dt);// Newtonsoft.Json
            return result;
        }

        /// <summary>
        /// 测试查询日志消息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public DataResult<object> GetLogMessagesPageList()
        {
            DataResult<object> result = new DataResult<object>();
            ValuesService model = new ValuesService();
            PagingModel pgmodel = new PagingModel();
            _StrValue = "";
            _dt = _logmessagesservice.GetLogMessagesPageList(pgmodel, out _StrValue);
            result.Data = JsonHelper.SerializeObject(_dt);
            return result;
        }
        /// <summary>
        /// 测试第二种输出方式
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetIActionResult(int Id)
        {
            _dt = _valuesservice.GetEformEdrmsBorrowInfo();
            return new JsonResult(_dt); // mvcjson插件
        }
        //// GET: api/<ValuesController>
        ///// <summary>
        ///// 未用到
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ValuesController>/5
        ///// <summary>
        ///// 未用到
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value="+ id;
        //}

        //// POST api/<ValuesController>
        ///// <summary>
        ///// 未用到
        ///// </summary>
        ///// <param name="value"></param>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        ///// <summary>
        ///// 未用到
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        ///// <summary>
        ///// 未用到
        ///// </summary>
        ///// <param name="id"></param>
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
