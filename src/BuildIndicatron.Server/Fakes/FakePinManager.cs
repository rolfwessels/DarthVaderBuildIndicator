using System.Collections.Concurrent;
using System.Reflection;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using log4net;

namespace BuildIndicatron.Server.Fakes
{
    public class FakePinManager : IPinManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<object, StateHolder> _dictionary = new ConcurrentDictionary<object, StateHolder>();

      
        #region Nested type: StateHolder

        private class StateHolder
        {
            private bool _currentState;

            public bool ChangeState(bool isOn)
            {
                if (_currentState == isOn) return false;
                _currentState = isOn;
                return true;
            }
        }

        #endregion

        #region Implementation of IPinManager

        public void SetPin(int pin, bool isOn)
        {
            var stateHolder = _dictionary.GetOrAdd(pin, a => new StateHolder());
            if (stateHolder.ChangeState(isOn))
                _log.Info(string.Format("Set int pin : {0} [{1}]", pin, isOn ? "On" : "Off"));
        }

        public void SetPin(PinName target, bool isOn)
        {
            var stateHolder = _dictionary.GetOrAdd(target, a => new StateHolder());
            if (stateHolder.ChangeState(isOn))
                _log.Info(string.Format("Set PinName : {0} [{1}]", target, isOn ? "On" : "Off"));
        }

        public void SetPin(GpIO target, bool isOn)
        {
            var stateHolder = _dictionary.GetOrAdd(target, a => new StateHolder());
            if (stateHolder.ChangeState(isOn))
                _log.Info(string.Format("Set GpIO : {0} [{1}]", target, isOn ? "On" : "Off"));
        }

        #endregion
    }
}