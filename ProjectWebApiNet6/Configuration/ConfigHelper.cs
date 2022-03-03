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
        /// 全局参数配置
        /// </summary>
        public ConfigParameter ConfigParameter { get; set; }
        /// <summary>
        /// 自动服务配置
        /// </summary>
        public AutoServiceConfig AutoServiceConfig { get; set; }
        /// <summary>
        /// 钉钉集成配置
        /// </summary>
        public DingDingConfig DingDingConfig { get; set; }
        /// <summary>
        /// 钉钉集成配置
        /// </summary>
        public DingDingMobileConfig DingDingMobileConfig { get; set; }
        /// <summary>
        /// 报表相关信息配置
        /// </summary>
        public ReportConfig ReportConfig { get; set; }
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
    /// 全局参数配置
    /// </summary>
    public class ConfigParameter
    {
        /// <summary>
        /// 仓库智能密集架
        /// </summary>
        public string WMSUrl { get; set; }
        /// <summary>
        /// BPM(档案流程申请)
        /// </summary>
        public string BPMUrl { get; set; }
    }

    #region 自动服务配置
    /// <summary>
    /// 服务配置基础类
    /// </summary>
    public class ServiceCfgBase
    {
        /// <summary>
        /// 开关
        /// </summary>
        public string Switch { get; set; } = "OFF";
        /// <summary>
        /// 服务间隔时间(单位：分钟)
        /// </summary>
        public string TimeInterval { get; set; } = "1";
    }

    /// <summary>
    /// 服务配置
    /// </summary>
    public class AutoServiceConfig
    {
        /// <summary>
        /// 【会计档案凭证】自动同步服务
        /// </summary>
        public AccountingArchivesServiceCfg AccountingArchivesServiceCfg { get; set; }

        /// <summary>
        /// 【自动整编预归档库中的数据】定时服务
        /// </summary>
        public PreArchivedDataServiceCfg PreArchivedDataServiceCfg { get; set; }

        /// <summary>
        /// 【BMP】自动服务
        /// </summary>
        public BMPServiceCfg BMPServiceCfg { get; set; }

        /// <summary>
        /// 档案过期自动提醒服务
        /// </summary>
        public ArchivesExpirationAutoReminderServiceCfg ArchivesExpirationAutoReminderServiceCfg { get; set; }
    }

    /// <summary>
    /// 【会计档案凭证】自动同步服务
    /// </summary>
    public class AccountingArchivesServiceCfg : ServiceCfgBase
    {
        /// <summary>
        /// SAP提交元数据所用SourceId，来源：http://172.16.2.48/wcm/edrms#jiekoulaiyuanpeizhiefrom
        /// </summary>
        public string SourceId { get; set; }
        /// <summary>
        /// 延迟天数；当前日期减去当前配置天数为服务截止日期。
        /// </summary>
        public string DelayDays { get; set; }
        /// <summary>
        /// SAP服务中，未匹配到全宗代码时使用默认的全宗代码
        /// </summary>
        public string DefaultFondsId { get; set; }
    }

    /// <summary>
    /// 【自动整编预归档库中的数据】定时服务
    /// </summary>
    public class PreArchivedDataServiceCfg
    {
        /// <summary>
        /// 开关
        /// </summary>
        public string Switch { get; set; } = "OFF";
        /// <summary>
        /// 服务运行时间点
        /// </summary>
        public string ExecDate { get; set; }
        /// <summary>
        /// 发送服务运行结果至该提醒组成员。若为空，则不提醒
        /// </summary>
        public string RemindGroup { get; set; }
    }

    /// <summary>
    /// 档案到期自动提醒服务
    /// </summary>
    public class ArchivesExpirationAutoReminderServiceCfg : ServiceCfgBase { }

    /// <summary>
    /// 【BMP】自动服务
    /// </summary>
    public class BMPServiceCfg : ServiceCfgBase { }
    #endregion

    /// <summary>
    /// 钉钉集成配置
    /// </summary>
    public class DingDingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string CorpId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppSecret { get; set; }
    }

    /// <summary>
    /// 钉钉移动端集成配置
    /// </summary>
    public class DingDingMobileConfig : DingDingConfig
    {
        /// <summary>
        /// 钉钉配置地址
        /// </summary>
        public string RedirectEdrmsLoginUrl { get; set; }
    }

    /// <summary>
    /// 报表相关信息配置
    /// </summary>
    public class ReportConfig
    {
        /// <summary>
        /// 报表上传临时存储文件夹
        /// </summary>
        public string UploadFolderId { get; set; }
    }
}
