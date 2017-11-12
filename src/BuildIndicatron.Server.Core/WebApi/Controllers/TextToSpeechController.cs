using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Mvc;

namespace BuildIndicatron.Server.Api.Controllers
{
    [Route(RouteHelper.TextToSpeechController)]
    public class TextToSpeechController : Controller
	{
		private readonly ITextToSpeech _textToSpeech;
		private readonly IVoiceEnhancer _voiceEnhancer;

		public TextToSpeechController(ITextToSpeech textToSpeech,IVoiceEnhancer voiceEnhancer)
		{
			_textToSpeech = textToSpeech;
			_voiceEnhancer = voiceEnhancer;
		}

		[HttpGet]
		public async Task<TextToSpeechResponse> Get(string id)
		{
			await _textToSpeech.Play(id);
			return new TextToSpeechResponse() { };
		}

		[HttpGet(RouteHelper.TextToSpeechControllerEnhanceSpeech)]
		public TextToSpeechResponse EnhanceSpeech(string id)
		{
			_textToSpeech.Play(id, _voiceEnhancer);
			return new TextToSpeechResponse() { };
		}
	}
	
	
}