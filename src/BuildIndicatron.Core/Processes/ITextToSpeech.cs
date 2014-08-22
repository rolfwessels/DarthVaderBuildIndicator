using System.Threading.Tasks;

namespace BuildIndicatron.Core.Processes
{
	public interface ITextToSpeech
	{
		void Play(string text);
		void Play(string text, IMp3Player voiceEnhancer);
	}
}