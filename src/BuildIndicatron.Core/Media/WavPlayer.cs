using System.IO;
using System.Media;

namespace BuildIndicatron.Core.Media
{
    public class WavPlayer
    {
        public WavPlayer()
        {
        }

        public void Play(string resourcesPlayPoliceSWav)
        {
            using (var file = new FileStream(resourcesPlayPoliceSWav, FileMode.Open, FileAccess.Read))
            {
                using (var player = new SoundPlayer(file))
                {
                    player.PlaySync();
                }
            }
        }
    }
}