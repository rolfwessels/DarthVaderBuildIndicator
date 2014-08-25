using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Core.Processes
{
	public interface IPinManager
	{
		void SetPin(int pin, bool isOn);
		void SetPin(PinName target, bool isOn);
		void SetPin(GpIO target, bool isOn);
	}
}