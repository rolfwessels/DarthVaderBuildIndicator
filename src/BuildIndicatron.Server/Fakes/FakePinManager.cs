using System.Reflection;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using log4net;

namespace BuildIndicatron.Server.Fakes
{
    internal class FakePinManager : IPinManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Implementation of IPinManager

        public void SetPin(int pin, bool isOn)
        {
            _log.Info(string.Format("Set int pin : {0} [{1}]", pin, isOn ? "On" : "Off"));
        }

        public void SetPin(PinName target, bool isOn)
        {
            _log.Info(string.Format("Set PinName : {0} [{1}]", target, isOn ? "On" : "Off"));
        }

        public void SetPin(GpIO target, bool isOn)
        {
            _log.Info(string.Format("Set GpIO : {0} [{1}]", target, isOn ? "On" : "Off"));
        }

        #endregion
    }
}