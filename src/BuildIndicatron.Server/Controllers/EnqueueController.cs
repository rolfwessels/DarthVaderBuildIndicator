using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.ApiResponses;
using log4net;

namespace BuildIndicatron.Server.Controllers
{
	public class EnqueueController : ApiController
	{
		private readonly IStage _stage;
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public EnqueueController(IStage stage)
		{
			_stage = stage;
		}
		
		public EnqueueResponse Get()
		{
			return new EnqueueResponse() { QueueSize = _stage.Count };
		}

		public EnqueueResponse Post(ChoreographyModel choreography)
		{
			_log.Info(string.Format("Cor:[{0}]", choreography.Dump()));
			foreach (var sequencese in choreography.Sequences)
			{
				_stage.Enqueue(sequencese);
			}
			_stage.Play();
			return new EnqueueResponse() { QueueSize = _stage.Count };
		}

		public class ChoreographyModel
		{
			public List<dynamic> Sequences { get; set; }
		}
	}
}