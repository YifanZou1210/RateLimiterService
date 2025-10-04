// 限流器接口
// 文件路径: src/RateLimiter.Core/Services/IRateLimiter.cs
using System.Threading.Tasks;
using RateLimiter.Core.Models;

namespace RateLimiter.Core.Services
{
    /// <summary>
    /// 限流器接口，定义了限流器必须实现的方法
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// 检查请求是否被允许
        /// </summary>
        /// <param name="clientId">客户端标识符（可以是IP地址、用户ID、API密钥等）</param>
        /// <param name="resource">资源标识符（API端点路径）</param>
        /// <param name="cost">本次请求消耗的配额（默认为1）</param>
        /// <returns>包含是否允许请求以及相关元数据的结果</returns>
        Task<RateLimitResult> AllowRequestAsync(
            string clientId, 
            string resource = "default",
            int cost = 1);
        
        /// <summary>
        /// 获取客户端在指定资源上的剩余配额
        /// </summary>
        /// <param name="clientId">客户端标识符</param>
        /// <param name="resource">资源标识符</param>
        /// <returns>剩余的请求配额数</returns>
        Task<int> GetRemainingQuotaAsync(string clientId, string resource = "default");
        
        /// <summary>
        /// 重置客户端在指定资源上的限流计数
        /// 通常用于管理员操作或特殊情况
        /// </summary>
        /// <param name="clientId">客户端标识符</param>
        /// <param name="resource">资源标识符</param>
        Task ResetAsync(string clientId, string resource = "default");
        
        /// <summary>
        /// 检查客户端是否在白名单中
        /// </summary>
        /// <param name="clientId">客户端标识符</param>
        /// <returns>true如果客户端在白名单中</returns>
        Task<bool> IsWhitelistedAsync(string clientId);
    }
}