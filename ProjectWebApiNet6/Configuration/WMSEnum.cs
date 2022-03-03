using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 控制智能密集架转换
    /// </summary>
    public class WMSEnum
    {
        private static string _StrValue = null;//全局值

        /// <summary>
        /// 智能密集架接口：返回状态说明
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string state(string state)
        {
            switch (state)

            {
                case "-2":
                    _StrValue = "未加载到库区信息";
                    break;
                case "-1":
                    _StrValue = "暂未支持此功能";
                    break;
                case "00":
                    _StrValue = "操作成功";
                    break;
                case "01":
                    _StrValue = "操作失败";
                    break;
                case "02":
                    _StrValue = "库区未连接指令发送失败 ";
                    break;
                case "03":
                    _StrValue = "库区未找到指令发送失败 ";
                    break;
                case "10":
                    _StrValue = "数据帧错误 ";
                    break;
                case "11":
                    _StrValue = "柜体锁定,控制失败";
                    break;
                default:
                    Console.WriteLine("输入的密集架接口码有误，系统未能解析！");
                    _StrValue = "暂未支持的密集架接口码";
                    break;
            }

            return _StrValue;
        }

        /// <summary>
        /// 柜体控制码说明 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string command(string state)
        {
            switch (state)

            {
                case "01":
                    _StrValue = "开架柜体左右移动";
                    break;
                case "02":
                    _StrValue = "保留未使用 ";
                    break;
                case "03":
                    _StrValue = "闭架 ";
                    break;
                case "04":
                    _StrValue = "通风";
                    break;
                case "05":
                    _StrValue = "急停  ";
                    break;
                case "06":
                    _StrValue = "锁定 ";
                    break;
                case "07":
                    _StrValue = "解锁 ";
                    break;
                default:
                    Console.WriteLine("输入的柜体控制码有误，系统未能解析！");
                    _StrValue = "暂未支持的柜体控制码";
                    break;
            }

            return _StrValue;
        }


    }
}
