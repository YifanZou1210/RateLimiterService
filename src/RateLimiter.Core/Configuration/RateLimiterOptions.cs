// 限流器配置选项
// 文件路径: src/RateLimiter.Core/Configuration/RateLimiterOptions.cs
using System;
using System.Collections.Generic;
using RateLimiter.Core.Models;

namespace RateLimiter.Core.Configuration
{
    /// <summary>
    /// 限流器的配置选项，可以通过appsettings.json或环境变量配置
    /// </summary>
    public class RateLimiterOptions
    {
        /// <summary>
        /// 默认的请求限制数
        /// </summary>
        public int DefaultLimit { get; set; } = 100;
        
        /// <summary>
        /// 默认的时间窗口（以秒为单位）
        /// </summary>
        public int DefaultWindowSeconds { get; set; } = 60;
        
        /// <summary>
        /// 默认的时间窗口（TimeSpan类型）
        /// </summary>
        public TimeSpan DefaultWindow => TimeSpan.FromSeconds(DefaultWindowSeconds);
        
        /// <summary>
        /// 是否启用限流器
        /// 在某些环境（如开发环境）可能想要禁用限流
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; } = "localhost:6379";
        
        /// <summary>
        /// 针对不同资源的限流规则
        /// Key: 资源路径（如 "api/users"）
        /// Value: 该资源的限流规则
        /// </summary>
        public Dictionary<string, RateLimitRule> Rules { get; set; } = new Dictionary<string, RateLimitRule>();
        
        /// <summary>
        /// 全局白名单客户端
        /// 这些客户端不受任何限流控制
        /// </summary>
        public HashSet<string> GlobalWhitelist { get; set; } = new HashSet<string>();
        
        /// <summary>
        /// 是否在响应头中包含限流信息
        /// </summary>
        public bool IncludeRateLimitHeaders { get; set; } = true;
        
        /// <summary>
        /// 获取指定资源的限流规则
        /// 如果没有特定规则，返回默认规则
        /// </summary>
        public RateLimitRule GetRuleForResource(string resource)
        {
            // 首先尝试精确匹配
            if (Rules.TryGetValue(resource, out var rule))
            {
                return rule;
            }
            
            // 尝试通配符匹配（比如 "api/*" 匹配所有api开头的路径）
            foreach (var kvp in Rules)
            {
                if (kvp.Key.EndsWith("*"))
                {
                    var prefix = kvp.Key.TrimEnd('*');
                    if (resource.StartsWith(prefix))
                    {
                        return kvp.Value;
                    }
                }
            }
            
            // 返回默认规则
            return new RateLimitRule
            {
                Resource = resource,
                Limit = DefaultLimit,
                Window = DefaultWindow
            };
        }
    }
}