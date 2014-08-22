using System.Threading.Tasks;

namespace BuildIndicatron.Core.Processes
{
	public interface ITextToSpeech
	{
		void Play(string text);
	}
}