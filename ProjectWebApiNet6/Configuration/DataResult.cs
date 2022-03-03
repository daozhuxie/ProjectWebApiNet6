/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2017-11
 *	Description:API返回消息定义类
 *------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// API接口返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T>// where T : class
    {
        /// <summary>
        /// Object 返回接口业务数据 --数据要自动转Json
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 状态码 1:代表成功 ,0:代表失败 ,2:或其他
        /// </summary>
        public int ResultCode { get; set; } = 0;
        /// <summary>
        /// 描述
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 数据描述
        /// </summary>
        public string DataDescription { get; set; } = "";
    }

    /// <summary>
    /// API输入消息定义类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataInput<T> where T : class
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public string LogUserId { get; set; }
        /// <summary>
        /// 登录用户Code
        /// </summary>
        public string LogCode { get; set; }
        /// <summary>
        /// 存储借阅主表的借阅备注
        /// -- 操作流程类型/借阅/续借/鉴定/销毁  renew:续借=1,preview:正常借阅=0
        /// </summary>
        public string Type { get; set; }
        ///// <summary>
        ///// 登录用户Token
        ///// </summary>
        ////[Required(ErrorMessage = "Token必填")]
        //public string Token { get; set; }

    }
}
