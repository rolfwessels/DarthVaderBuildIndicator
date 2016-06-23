using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Shared.Enums;
using log4net;
using Raspberry.IO.GeneralPurpose;

namespace BuildIndicatron.Core.Api
{
    public class MonitorJenkins : IMonitorJenkins
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IJenkinsFactory _jenkinsFactory;
        private readonly IPinManager _pinManager;
        private readonly ISettingsManager _settingsManager;
        private bool _isStarted;

        public MonitorJenkins(IJenkinsFactory jenkinsFactory, IPinManager pinManager, ISettingsManager settingsManager)
        {
            _jenkinsFactory = jenkinsFactory;
            _pinManager = pinManager;
            _settingsManager = settingsManager;
        }

        #region Implementation of IMonitorJenkins

        public async Task Check()
        {
            var allProjects = await _jenkinsFactory.GetBuilder().GetAllProjects();
            _pinManager.SetPin(PinName.MainLightRed, allProjects.Jobs.Any(x => x.IsFailed()));
            //_pinManager.SetPin(PinName.SecondaryLightGreen, allProjects.Jobs.All(x => !x.IsFailed()));
            var allProjects2 = _settingsManager.GetMyBuildingJobs(allProjects).ToArray();
            _pinManager.SetPin(PinName.SecondaryLightRed, allProjects2.Any(x => x.IsFailed()));
            _pinManager.SetPin(PinName.MainLightBlue, allProjects2.Any(x => x.IsProcessing()));
            _pinManager.SetPin(PinName.MainLightGreen, allProjects2.All(x => x.IsPassed()));
        }

        #endregion

        public async Task StartMonitor(TimeSpan delay)
        {
            if (_isStarted) throw new Exception("Alread started.");
            _isStarted = true;
            while (_isStarted)
            {
                try
                {
                    await Check();
                }
                catch (Exception e)
                {
                    _log.Error("MonitorJenkins:StartScanningJenkins " + e.Message);
                }
                await Task.Delay(delay);
            }
        }

        public void Stop()
        {
            _isStarted = false;
        }
    }

  
}