using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Core.Processes
{
	public interface ISequencePlayer
	{
		void Play(Sequences sequences);
	}
}