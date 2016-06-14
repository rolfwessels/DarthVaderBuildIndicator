using System;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Api
{
    public interface IMonitorJenkins
    {
        Task Check();
        Task StartMonitor(TimeSpan delay);
        void Stop();
    }
}