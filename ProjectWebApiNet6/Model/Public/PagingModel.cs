using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApiNet5.Model.Public
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public class PagingModel
    {

        /// <summary>
        /// 精确查询添加Json
        /// </summary>
        public String SearchField { get; set; }
        /// <summary>
        /// 范围查询条件
        /// </summary>
        public String RangeField { get; set; }
        /// <summary>
        /// 模糊条件查询
        /// </summary>
        public String FuzzyField { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序顺序
        /// </summary>
        public string SortOrder { get; set; }
    }
}
