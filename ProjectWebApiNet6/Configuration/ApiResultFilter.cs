using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public class ApiResultFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                var result = context.ModelState.Keys
                        .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(404, x.ErrorMessage)))
                        .ToList();
#pragma warning restore CS8602 // 解引用可能出现空引用。
                Robj<object> robj = new Robj<object>();
                robj.Error(result[0].Result, (int)result[0].Code);
                ObjectResult objectResult = new ObjectResult(robj);
                context.Result = objectResult;
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 方法执行后,返回结果处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {   //执行成功，获取api返回信息
                ObjectResult? result = context.Result as ObjectResult;

                if (result != null)
                {   // 重新封装格式
                    Robj<object> robj = new Robj<object>();
                    if (result.StatusCode == 200)
                        robj.Success(result.Value);
                    else
                        robj.Error(result.Value, (int)200);//robj.Error(result.Value, (int)result.StatusCode);
                    ObjectResult objectResult = new ObjectResult(robj);
                    context.Result = objectResult;
                }
            }
            base.OnActionExecuted(context);
        }
    }

    /// <summary>
    /// 返回结果对象
    /// 默认RCode为成功,Message为成功.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Robj<T>
    {
        //T result = default(T);
        T result;
        int code = 200;
        string message = "操作成功";

        /// <summary>
        /// 结果
        /// </summary>
        public T Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        public int Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="result">返回结果</param>
        /// <param name="msg">提示消息</param>
        public void Success(T result, string msg = "操作成功")
        {
            this.code = 200;
            this.result = result;
            this.Message = msg;
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="result"></param>
        /// <param name="msg">提示消息</param>
        /// <param name="code"></param>
        public void Error(T result, int code, string msg = "操作失败")
        {
            this.code = code;
            this.result = result;
            this.Message = msg;
        }
    }

    ///// <summary>
    ///// 返回Code
    ///// </summary>
    //public enum RCode
    //{
    //    /// <summary>
    //    /// 成功
    //    /// </summary>
    //    [JsonProperty("200")]
    //    Success = 200,

    //    /// <summary>
    //    /// 程序异常
    //    /// </summary>
    //    [JsonProperty("300")]
    //    Exception = 3000,

    //    /// <summary>
    //    /// 系统错误
    //    /// </summary>
    //    [JsonProperty("400")]
    //    SysError = 4000
    //}
    /// <summary>
    /// 
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Result { get; }
        /// <summary>
        /// 构造请求参数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        public ValidationError(int code, string result)
        {
            Code = code;
            Result = result;
            Message = "操作失败";
        }
    }
}
