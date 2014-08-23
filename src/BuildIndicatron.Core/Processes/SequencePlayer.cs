using System;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Shared.Models.Composition;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class SequencePlayer : ISequencePlayer
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IMp3Player _mp3Player;
		private readonly ITextToSpeech _textToSpeech;
		private readonly IVoiceEnhancer _voiceEnhancer;
		private readonly ISoundFilePicker _soundFilePicker;

		public SequencePlayer(ITextToSpeech textToSpeech, IMp3Player mp3Player, IVoiceEnhancer voiceEnhancer, ISoundFilePicker soundFilePicker)
		{
			_textToSpeech = textToSpeech;
			_mp3Player = mp3Player;
			_voiceEnhancer = voiceEnhancer;
			_soundFilePicker = soundFilePicker;
		}

		#region Implementation of ISequencePlayer

		public void Play(Sequences sequences)
		{
			if (sequences.Type == SequencesText2Speech.TypeName)
			{
				Play(sequences as SequencesText2Speech);
			}
			if (sequences.Type == SequencesGpIo.TypeName)
			{
				Play(sequences as SequencesGpIo);
			}
			if (sequences.Type == SequencesInsult.TypeName)
			{
				Play(sequences as SequencesInsult);
			}
			if (sequences.Type == SequencesOneLiner.TypeName)
			{
				Play(sequences as SequencesOneLiner);
			}
			if (sequences.Type == SequencesPlaySound.TypeName)
			{
				Play(sequences as SequencesPlaySound);
			}
			if (sequences.Type == SequencesQuotes.TypeName)
			{
				Play(sequences as SequencesQuotes);
			}
			if (sequences.Type == SequencesTweet.TypeName)
			{
				Play(sequences as SequencesTweet);
			}
			throw new Exception("Could not determine type");
		}

		#endregion

		
		public void Play(SequencesGpIo sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
		}


		public void Play(SequencesPlaySound sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
			_mp3Player.PlayFile(_soundFilePicker.PickFile(sequences.File));
		}

		public void Play(SequencesText2Speech sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
			PlayText(sequences.Text, sequences.DisableTransform);
		}

		public void Play(SequencesQuotes sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
			PlayText(RandomTextHelper.Quotes,false,sequences.SendTweet);
		}

		public void Play(SequencesInsult sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
			PlayText(RandomTextHelper.Insult, false, sequences.SendTweet);
		}

		public void Play(SequencesOneLiner sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
			PlayText(RandomTextHelper.OneLiner, false, sequences.SendTweet);
		}

		public void Play(SequencesTweet sequences)
		{
			if (sequences == null) throw new ArgumentNullException("sequences");
			_log.Info("Running: " + sequences.Dump(false));
		}

		private void PlayText(string text, bool disableTransform = false , bool sendTweet = false)
		{
			if (!disableTransform)
			{
				_textToSpeech.Play(text, _voiceEnhancer);
			}
			else
			{
				_textToSpeech.Play(text);
			}
		}
	}
}