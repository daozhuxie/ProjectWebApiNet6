using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 错误捕捉？
    /// </summary>
    public class UnionExceptionAttribute : IExceptionFilter, IFilterMetadata
    {
        /// <summary>
        /// 错误捕捉？
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            if (context.ExceptionHandled == false)
            {
                DataResult<Object> result = new DataResult<Object>();
                result.ResultCode = 0;// (int)ApiResponeState.SysError;
                //result.Message = "错误"; //EnumConvertor.ToDescString(ApiResponeState.SysError);
                result.Message = $"错误：{context.Exception.Message}";
                logger.Error("异常:" + context.Exception.Message);

                context.Result = new ContentResult
                {
                    Content = JsonHelper.SerializeObject(result),
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "text/html;charset=utf-8"
                };

            }
            context.ExceptionHandled = true;
        }
    }
}
