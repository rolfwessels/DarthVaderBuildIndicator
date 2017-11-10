using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using log4net;
using Raspberry.IO.GeneralPurpose;

namespace BuildIndicatron.Integration.Raspberry
{
	public class PinManager : IPinManager , IDisposable
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly GpioConfiguration _configuration;
		private IGpioConnectionDriver _driver;
		private Dictionary<PinName, ProcessorPin> _pins;


		public PinManager(GpioConfiguration configuration)
		{
			_configuration = configuration;
			SetupPins(_configuration);
		}

		private void SetupPins(GpioConfiguration gpioConfiguration)
		{
			if (!PlatformHelper.IsLinux) return;
			_log.Info("Default driver");
			_driver = GpioConnectionSettings.DefaultDriver;
			_pins = new Dictionary<PinName, ProcessorPin>();
			foreach (var target in gpioConfiguration.Targets)
			{
				var processorPin = GetPin(target.Pin);
				_pins.Add(target.PinName, processorPin);
				_log.Info(string.Format("Add pin output {0}", target.PinName));
				_driver.Allocate(processorPin, PinDirection.Output);
				SetPin(target, false);
			}

			foreach (var target in gpioConfiguration.Buttons)
			{	
				var processorPin = GetPin(target.Pin);
				_pins.Add(PinName.MainButton, processorPin);
				_log.Info(string.Format("Add pin Input {0}", PinName.MainButton));
				_driver.Allocate(processorPin, PinDirection.Input);
			}	
		}

		public static void RunSample(ConnectorPin connectorPin)
		{
//			Console.Out.WriteLine("Pin:" + connectorPin);
//			ProcessorPin led = connectorPin.ToProcessor();
//			Console.Out.WriteLine("Load driver:" + led);
//			
//
//			Console.Out.WriteLine("Pin out");
//			
//			Console.Out.WriteLine("Pin on");
//			driver.Write(led, false);
//			Thread.Sleep(2000);
//			Console.Out.WriteLine("Pin off");
//			driver.Write(led, true);
//			Console.Out.WriteLine("Release");
			
		}

		#region Implementation of IPinManager

		public virtual void SetPin(int pin, bool isOn)
		{
			var targetFound = _configuration.Targets.FirstOrDefault(x => x.Pin.ToString().EndsWith("Pin" + pin));
			SetPin(targetFound, isOn);
		}
		
		public virtual void SetPin(ConnectorPin pin, bool isOn)
		{
			var targetFound = _configuration.Targets.FirstOrDefault(x => x.Pin == (GpIO) 25);
			SetPin(targetFound, isOn);
		}

		public virtual void SetPin(GpioConfiguration.Target target, bool isOn)
		{
			if (target != null && _pins != null)
			{
				_log.Info(string.Format("Setting pin {0} [{1}] to {2}", target.PinName, target.Pin, target.IsReverse));
				_driver.Write(_pins[target.PinName], target.IsReverse ? !isOn : isOn);
				return;
			}
			_log.Warn("Could not find that pin sorry ");		
		}

		public void SetPin(GpIO target, bool isOn)
		{
			var targetFound = _configuration.Targets.FirstOrDefault(x => x.Pin == target);
			SetPin(targetFound, isOn);
		}

		public virtual void SetPin(PinName target, bool isOn)
		{
			var targetFound = _configuration.Targets.FirstOrDefault(x => x.PinName == target);
			SetPin(targetFound, isOn);
		}

		

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			if (_pins != null)
				foreach (var processorPin in _pins.Values)
				{
					if (_driver != null) _driver.Release(processorPin);
				}
		}

		#endregion

		#region Private Methods


		private ProcessorPin GetPin(GpIO gpIo)
		{
			switch (gpIo)
			{
				case GpIO.GPIO2:
					return ProcessorPin.Pin2;
				case GpIO.GPIO3:
					return ProcessorPin.Pin3;
				case GpIO.GPIO4:
					return ProcessorPin.Pin4;
				case GpIO.GPIO17:
					return ProcessorPin.Pin17;
				case GpIO.GPIO27:
					return ProcessorPin.Pin27;
				case GpIO.GPIO22:
					return ProcessorPin.Pin22;
				case GpIO.GPIO10:
					return ProcessorPin.Pin10;
				case GpIO.GPIO9:
					return ProcessorPin.Pin9;
				case GpIO.GPIO11:
					return ProcessorPin.Pin11;
				case GpIO.GPIO7:
					return ProcessorPin.Pin7;
				case GpIO.GPIO8:
					return ProcessorPin.Pin8;
				case GpIO.GPIO25:
					return ProcessorPin.Pin25;
				case GpIO.GPIO24:
					return ProcessorPin.Pin24;
				case GpIO.GPIO23:
					return ProcessorPin.Pin23;
				case GpIO.GPIO18:
					return ProcessorPin.Pin18;
				case GpIO.GPIO15:
					return ProcessorPin.Pin15;
				case GpIO.GPIO14:
					return ProcessorPin.Pin14;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
		}

		#endregion
	}
}

	