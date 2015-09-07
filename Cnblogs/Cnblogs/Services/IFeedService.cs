using Cnblogs.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App96.Services
{
	public interface IFeedService
    {
        Task<TResult> SendRequst<TResult>(ServiceType type, NewsData data) where TResult : class;
    }
}
