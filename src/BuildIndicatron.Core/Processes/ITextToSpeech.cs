using System.Threading.Tasks;

namespace BuildIndicatron.Core.Processes
{
	public interface ITextToSpeech
	{
		Task Play(string text);
		Task Play(string text, IMp3Player voiceEnhancer);
	}
}