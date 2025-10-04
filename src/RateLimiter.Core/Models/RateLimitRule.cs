// 限流规则模型，定义什么资源，多久内，允许多少请求
// 文件路径: src/RateLimiter.Core/Models/RateLimitRule.cs
using System;

namespace RateLimiter.Core.Models
{
    // <summary>
    // 定义限流规则，每个API端点可以有不同的限流配置
    // </summary>
    public class RateLimitRule
    {
        // <summary>
        // 资源标识符，比如 "api/users" 或 "api/orders"
        // </summary>
        public string Resource { get; set; } = "default";// api/users
        
        // <summary>
        // 时间窗口内允许的最大请求数
        // 比如：100表示在时间窗口内最多允许100个请求
        // </summary>
        public int Limit { get; set; } = 100; // max 100/min
        
        // <summary>
        // 时间窗口的大小
        // 比如：TimeSpan.FromMinutes(1) 表示1分钟的时间窗口
        // </summary>
        public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);
        
        // <summary>
        // 是否启用白名单功
        // </summary>
        public bool WhitelistEnabled { get; set; } = false;
        
        // <summary>
        // 白名单中的客户端标识符集合
        // 这些客户端不受限流控制
        // </summary>
        public HashSet<string> WhitelistClients { get; set; } = new HashSet<string>();
        
        // <summary>
        // 获取每分钟的请求数（RPM - Requests Per Minute）
        // 这是为了方便显示和配置
        // </summary>
        public int GetRPM()
        {
            // 计算每分钟允许的请求数RPM
            if (Window.TotalMinutes == 0) return 0;
            return (int)(Limit / Window.TotalMinutes);
        }
    }
}