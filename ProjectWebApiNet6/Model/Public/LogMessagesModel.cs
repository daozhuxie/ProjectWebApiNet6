using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApiNet5.Model.Public
{
    /// <summary>
    /// 公共记录日志消息
    /// </summary>
    public class LogMessagesModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public string createId { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string createName { get; set; }
        /// <summary>
        /// 操作类型编码（1:进入方法，2:请求失败，3:离开方法）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string typeNote { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string methodName { get; set; }
        /// <summary>
        /// 输入参数
        /// </summary>
        public string inputParameter { get; set; }
        /// <summary>
        /// 输出参数
        /// </summary>
        public string outputParameter { get; set; }
        /// <summary>
        /// 内容消息
        /// </summary>
        public string contentMessages { get; set; }
    }

    
}
