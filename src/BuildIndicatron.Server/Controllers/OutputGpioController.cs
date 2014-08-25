using System;
using System.Reflection;
using System.Web.Http;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.ApiResponses;
using log4net;

namespace BuildIndicatron.Server.Controllers
{
	public class OutputGpioController : ApiController
	{
		private readonly IPinManager _pinManager;
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public OutputGpioController(IPinManager pinManager)
		{
			_pinManager = pinManager;
		}

		public GpIoOutputResponse Get()
		{
			return new GpIoOutputResponse() {};
		}


		
		[Route("api/ouputgpio/{pin}/{ison}")]
		public GpIoOutputResponse Get(string pin, bool ison)
		{
			_log.Info(string.Format("OutputGpioController:Get pin:{0} ison:{1}", pin, ison));
			int pinId;
			if (int.TryParse(pin, out pinId))
			{
				_pinManager.SetPin(pinId, ison);
			}
			else
			{
				PinName result;
				if (Enum.TryParse(pin, true, out result))
				{
					_pinManager.SetPin(result, ison);
				}
				else
				{
					GpIO gpioDirection;
					if (Enum.TryParse(pin, true, out gpioDirection))
					{
						_pinManager.SetPin(gpioDirection, ison);
					}
				}
			}
			
			
			
			return new GpIoOutputResponse() {};
		}
	}
}