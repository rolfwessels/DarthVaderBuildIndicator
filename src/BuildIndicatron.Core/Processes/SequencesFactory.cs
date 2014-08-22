using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Shared.Models.Composition;
using Newtonsoft.Json;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class SequencesFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public Sequences From(dynamic sequencese)
		{
			_log.Debug("SequencesFactory:From data"+DebugHelper.Dump(sequencese));
			if (sequencese.Type == SequencesText2Speech.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesText2Speech>(sequencese);
			}
			if (sequencese.Type == SequencesGpIo.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesGpIo>(sequencese);
			}
			if (sequencese.Type == SequencesInsult.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesInsult>(sequencese);
			}
			if (sequencese.Type == SequencesOneLiner.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesOneLiner>(sequencese);
			}
			if (sequencese.Type == SequencesPlaySound.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesPlaySound>(sequencese);
			}
			if (sequencese.Type == SequencesQuotes.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesQuotes>(sequencese);
			}
			if (sequencese.Type == SequencesTweet.TypeName)
			{
				return DebugHelper.ToConcrete<SequencesTweet>(sequencese);
			}
			return DebugHelper.ToConcrete<Sequences>(sequencese);
		}
	}
}