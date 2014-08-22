using System.Collections.Generic;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Core.Processes
{
	public class Stage : IStage
	{
		private readonly SequencesFactory _sequencesFactory;
		private Queue<Sequences> _sequenceses;

		public Stage(SequencesFactory sequencesFactory)
		{
			_sequencesFactory = sequencesFactory;
			_sequenceses = new Queue<Sequences>();
		}

		#region Implementation of IStage

		public void Enqueue(Sequences sequencese)
		{
			_sequenceses.Enqueue(sequencese);
		}

		public void Enqueue(dynamic sequencese)
		{
			Enqueue(_sequencesFactory.From(sequencese));
		}

		#endregion

		public int Count
		{
			get { return _sequenceses.Count; }
		}
	}
}