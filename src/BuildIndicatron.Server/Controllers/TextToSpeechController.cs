using System.IO;
using System.Net;
using System.Web.Http;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.ApiResponses;

namespace BuildIndicatron.Server.Controllers
{
	public class TextToSpeechController : ApiController
	{
		private readonly ITextToSpeech _textToSpeech;

		public TextToSpeechController(ITextToSpeech textToSpeech)
		{
			_textToSpeech = textToSpeech;
		}

		public TextToSpeechResponse Get(string id)
		{
			_textToSpeech.Play(id);
			return new TextToSpeechResponse() { };
		}
	}
}