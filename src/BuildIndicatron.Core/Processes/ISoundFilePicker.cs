using System.Collections.Generic;
using System.IO;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Processes
{
	public interface ISoundFilePicker
	{
		IEnumerable<string> GetFolders();
		IEnumerable<string> GetAllSoundFiles(string directory);
		string PickFile(string id);
	}

	
}