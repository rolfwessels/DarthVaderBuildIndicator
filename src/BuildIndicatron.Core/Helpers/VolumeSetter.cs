using System;
using BuildIndicatron.Core.Chat;

namespace BuildIndicatron.Core.Helpers
{
    public class VolumeSetter : IVolumeSetter
    {
        static VolumeSetter()
        {
        }

        public  void SetVolume(int volumeLevel)
        {
            ProcessHelper.Run("amixer", String.Format("cset numid=1 --{0}%", volumeLevel.Map(0, 10, -2000, +500)));
        }
    }
}