﻿using System.IO;
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
		private readonly IVoiceEnhancer _voiceEnhancer;

		public TextToSpeechController(ITextToSpeech textToSpeech,IVoiceEnhancer voiceEnhancer)
		{
			_textToSpeech = textToSpeech;
			_voiceEnhancer = voiceEnhancer;
		}

		[HttpGet]
		public TextToSpeechResponse Get(string id)
		{
			_textToSpeech.Play(id);
			return new TextToSpeechResponse() { };
		}

		[HttpGet]
		public TextToSpeechResponse EnhanceSpeech(string id)
		{
			_textToSpeech.Play(id, _voiceEnhancer);
			return new TextToSpeechResponse() { };
		}
	}
	
	
}