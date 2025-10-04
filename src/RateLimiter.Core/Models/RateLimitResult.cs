// 限流结果模型，用于表示每次限流判断后的结果（Result Object），让 API 或中间件根据结果决定是否放行请求。
// 记录请求是否被允许，还剩多少请求，何时重置
// 文件路径: src/RateLimiter.Core/Models/RateLimitResult.cs
using System;

namespace RateLimiter.Core.Models
{
    // <summary>
    // 限流检查的结果，包含是否允许请求以及相关的元数据
    // </summary>
    public class RateLimitResult
    {
        // <summary>
        // 请求是否被允许
        // true: 请求在限制内，可以继续
        // false: 请求超过限制，应该被拒绝
        // </summary>
        public bool IsAllowed { get; set; }
        
        // <summary>
        // 当前时间窗口的限制数
        // </summary>
        public int Limit { get; set; }
        
        // <summary>
        // 当前时间窗口内剩余的请求配额
        // </summary>
        public int Remaining { get; set; }
        
        // <summary>
        // 限流重置时间（UTC时间）
        // 告诉客户端什么时候可以获得新的配额
        // </summary>
        public DateTime ResetsAt { get; set; }
        
        // <summary>
        // 拒绝原因的描述
        // </summary>
        public string Reason { get; set; } = string.Empty;
        
        // <summary>
        // 建议的重试时间间隔
        // 告诉客户端多久后可以重试
        // </summary>
        public TimeSpan RetryAfter { get; set; }
        
        // <summary>
        // 创建一个允许请求的结果,当限流算法执行完毕后（如滑动窗口、令牌桶算法），会返回 RateLimitResult
        // </summary>
        public static RateLimitResult Allowed(int limit, int remaining, DateTime resetsAt)
        {
            return new RateLimitResult
            {
                IsAllowed = true,
                Limit = limit,
                Remaining = remaining,
                ResetsAt = resetsAt,
                Reason = "Request allowed within rate limit",
                RetryAfter = TimeSpan.Zero
            };
        }
        
        // <summary>
        // 创建一个拒绝请求的结果
        // </summary>
        public static RateLimitResult Denied(int limit, DateTime resetsAt)
        {
            // 内部自动计算应等待时间
            var retryAfter = resetsAt - DateTime.UtcNow;
            if (retryAfter < TimeSpan.Zero) retryAfter = TimeSpan.Zero;
            
            return new RateLimitResult
            {
                IsAllowed = false,
                Limit = limit,
                Remaining = 0,
                ResetsAt = resetsAt,
                Reason = "Rate limit exceeded",
                RetryAfter = retryAfter
            };
        }
    }
}