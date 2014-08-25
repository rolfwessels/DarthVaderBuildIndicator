using System.Collections.Generic;
using BuildIndicatron.Shared.Enums;

namespace BuildIndicatron.Core.Processes
{
	public class GpioConfiguration
	{
		private List<Target> _targets;

		private List<Button> _buttons;

		public GpioConfiguration() : this(new Target[] {})
		{
		}

		public GpioConfiguration(params Target[] targets)
		{
			_buttons = new List<Button>();
			_targets = new List<Target>(targets);
		}

		public List<Button> Buttons
		{
			get { return _buttons; }

			set { _buttons = value; }
		}

		public List<Target> Targets
		{
			get { return _targets; }
			set { _targets = value; }
		}

		public class Target
		{
			private readonly PinName _target;
			private readonly GpIO _pin;
			private readonly bool _isReverse;

			public Target()
			{
			}

			public Target(PinName target, GpIO pin, bool isReverse = false)
			{
				_target = target;
				_pin = pin;
				_isReverse = isReverse;
			}

			public PinName PinName
			{
				get { return _target; }
			}

			public GpIO Pin
			{
				get { return _pin; }
			}

			public bool IsReverse
			{
				get { return _isReverse; }
			}
		}

		public void AddButton(GpIO p1Pin24, bool b)
		{
			_buttons.Add(new Button() {Pin = p1Pin24, IsToggle = false});
		}

		public class Button
		{
			public GpIO Pin { get; set; }

			public bool IsToggle { get; set; }
		}
	}
}