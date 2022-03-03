using ProjectWebApi.Configuration;
using ServiceStack.Redis;
using StackExchange.Redis;
using System.Data;

namespace ProjectWebApiNet6.Service.Redis
{
    
    /// <summary>
    /// Redis 
    /// </summary>
    public class RedisService
    {
        private DataTable _dt;//全局dt
        public volatile ConnectionMultiplexer _redisConnection;//= new ConnectionMultiplexer();
        public readonly object _redisConnectionLock = new object();
        public readonly ConfigurationOptions _configOptions;

        public RedisService()
        {
            ConfigurationOptions options = ReadRedisSetting();
            if (options == null)
            {
                string strmsg = "options 为空";
                throw new ArgumentNullException(nameof(options));
            }
            this._configOptions= options;
            this._redisConnection = ConnectionRedis();
        }
        /// <summary>
        /// 获取 Redis 配置信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ConfigurationOptions ReadRedisSetting()
        {
            try
            {
                List<RedisConfig> config = new List<RedisConfig>();// AppHelper.ReadAppSettings<RedisConfig>(new string[] { "Redis" }); // 读取Redis配置信息
                //if (config.Any())
                //{
                    ConfigurationOptions options = new ConfigurationOptions
                    {
                        EndPoints =
                            {
                                {
                                    SiteConfigOutput._siteConfig.Redis.Ip,
                                    Convert.ToInt32(SiteConfigOutput._siteConfig.Redis.Port)
                                    //config.FirstOrDefault().Ip,
                                    //config.FirstOrDefault().Port

                                }
                            },
                        ClientName = SiteConfigOutput._siteConfig.Redis.Name, //config.FirstOrDefault().Name,
                        Password = SiteConfigOutput._siteConfig.Redis.Password,//config.FirstOrDefault().Password,
                        ConnectTimeout = Convert.ToInt32(SiteConfigOutput._siteConfig.Redis.Timeout),//config.FirstOrDefault().Timeout,
                        DefaultDatabase = Convert.ToInt32(SiteConfigOutput._siteConfig.Redis.Db),//config.FirstOrDefault().Db,
                    };
                    return options;
                //}
                //return null;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"获取Redis配置信息失败：{ex.Message}");
                throw new Exception("获取Redis配置信息失败:" + ex);
                //return null;
            }

        }
        /// <summary>
        /// 开始 Redis 连接操作
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ConnectionMultiplexer ConnectionRedis()
        {

            if (this._redisConnection != null && this._redisConnection.IsConnected)
            {
                return this._redisConnection; // 已有连接，直接使用
            }
            lock (_redisConnectionLock)
            {
                if (this._redisConnection != null)
                {
                    this._redisConnection.Dispose(); // 释放，重连
                }
                try
                {
                    this._redisConnection = ConnectionMultiplexer.Connect(_configOptions);
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Redis服务启动失败：{ex.Message}");
                    throw new Exception("Redis服务启动失败:" + ex);
                }
            }
            return this._redisConnection;
        }
    }

    
}
