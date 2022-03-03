namespace ProjectWebApiNet6.Model.Redis
{
    /// <summary>
    /// Redis 请求配置
    /// </summary>
    public class RedisModel
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务端地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 端口号Port默认是6379
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 连接密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 连接的超时时间
        /// </summary>
        public string Timeout { get; set; }
        /// <summary>
        /// 使用Redis的DB区，一般Redis的DB区默认是0到15
        /// </summary>
        public string Db { get; set; }
    }
}
