using System.Threading.Tasks;
using RestSharp;

namespace BuildIndicatron.Core.Helpers
{
    public interface IHttpLookup
    {
        Task<IRestResponse> Download(string baseUrl, int timeout = 5000);
    }
}