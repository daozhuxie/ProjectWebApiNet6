/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:配置文件帮助类
 *------------------------------------------------------*/
using Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 封装公共方法，获取appsettings.js配置的变量
    /// </summary>
    public class SiteConfigOutput
    {
        /// <summary>
        /// 公共的静态变量
        /// </summary>
        public static SiteConfig _siteConfig { get; set; }
        /// <summary>
        /// 公共的静态变量
        /// </summary>
        public static DBInfoService _dbInfoService { get; set; }

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static SiteConfigOutput()
        {
            //验证如果配置为空时-去获取新配置文件信息
            if (_siteConfig == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine("appsettings.json"), true, true);
                IConfigurationRoot _configs = builder.Build();
                _siteConfig = _configs.GetSection("SiteConfig").Get<SiteConfig>();
            }
            //验证如果数据库请教方法位空时- 去获取数据库请求的方法
            if (_dbInfoService == null)
            {
                _dbInfoService = new DBInfoService();
                _dbInfoService.GetNewInstance(_siteConfig.ConectionConfig.ProjectMySql, SqlSugar.DbType.MySql);
            }
        }
    }

    /// <summary>
    /// 设置主对象指向 appsettings.json 下的子对象
    /// </summary>
    public class SiteConfig
    {
        /// <summary>
        /// 全局连接接口动态配置
        /// </summary>
        public OrgSyncing OrgSyncing { get; set; }
        /// <summary>
        /// 全局连接数据库配置
        /// </summary>
        public ConectionConfig ConectionConfig { get; set; }
        /// <summary>
        /// Redis 配置
        /// </summary>
        public Redis Redis { get; set; }
    }

    /// <summary>
    /// 全局连接接口动态配置
    /// </summary>
    public class OrgSyncing
    {
        /// <summary>
        /// 整个系统对应的Ip地址
        /// </summary>
        public string EDRMS_HTTP { get; set; }
        /// <summary>
        /// 获取产品token地址
        /// </summary>
        public string EDoc2_token { get; set; }
        /// <summary>
        /// 默认永久token
        /// </summary>
        public string EDoc2_perm_token { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string EDoc2_IntegrationKey { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string EDoc2_AdminAccount { get; set; }
        /// <summary>
        /// EdocIP
        /// </summary>
        public string EDoc2_IpAddress { get; set; }
        /// <summary>
        /// 温/湿度导入
        /// </summary>
        public string EDoc2_InsertStorageRecord { get; set; }
        /// <summary>
        /// 产品文件夹/文件赋值权限
        /// </summary>
        public string EDoc2_SetFilePermission { get; set; }
        /// <summary>
        /// 产品系统发送邮件
        /// </summary>
        public string EDoc2_SendSysMailMessage { get; set; }
    }

    /// <summary>
    /// 全局连接数据库配置
    /// </summary>
    public class ConectionConfig
    {
        /// <summary>
        /// ProjectSql数据库配置
        /// </summary>
        public string ProjectSql { get; set; }
        /// <summary>
        /// ProjectOracle数据库配置
        /// </summary>
        public string ProjectOracle { get; set; }
        /// <summary>
        /// ProjectMySql数据库配置
        /// </summary>
        public string ProjectMySql { get; set; }
    }

    /// <summary>
    /// 全局Redis 配置
    /// </summary>
    public class Redis
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
