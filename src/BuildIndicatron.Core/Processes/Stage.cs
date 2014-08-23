using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Shared.Models.Composition;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class Stage : IStage
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly SequencesFactory _sequencesFactory;
		private readonly ISequencePlayer _sequencePlayer;
		private readonly Queue<Sequences> _queue;
		private Task _currentPlayer;

		public Stage(SequencesFactory sequencesFactory, ISequencePlayer sequencePlayer)
		{
			_sequencesFactory = sequencesFactory;
			_sequencePlayer = sequencePlayer;
			_queue = new Queue<Sequences>();
		}

		#region Implementation of IStage

		public void Enqueue(Sequences sequencese)
		{
			_queue.Enqueue(sequencese);
		}

		public void Enqueue(dynamic sequencese)
		{
			Enqueue(_sequencesFactory.From(sequencese));
		}

		#endregion

		public int Count
		{
			get { return _queue.Count; }
		}

		public Task Play()
		{
			return _currentPlayer ?? (_currentPlayer = Task.Run(() => ReadQueue()));
		}

		private void ReadQueue()
		{
			while (_queue.Any())
			{
				var sequences = _queue.Dequeue();
				_log.Info("Dequeue sequence: " + sequences.Type);
				if (sequences.BeginTime > 0)
				{
					_log.Info("Sleeping: " + sequences.BeginTime);
					Task.Delay(sequences.BeginTime).Wait();
				}
				_sequencePlayer.Play(sequences);
			}
			_currentPlayer = null;
			_log.Debug("Done with queue");
		}
	}
}