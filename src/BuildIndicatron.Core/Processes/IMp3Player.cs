using System.Threading.Tasks;

namespace BuildIndicatron.Core.Processes
{
	public interface IMp3Player
	{
		Task PlayFile(string fileName);
	}
}