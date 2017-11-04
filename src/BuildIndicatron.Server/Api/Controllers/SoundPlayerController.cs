using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.ApiResponses;
using log4net;

namespace BuildIndicatron.Server.Api.Controllers
{
    [Route(RouteHelper.SoundPlayerController)]
    public class SoundPlayerController : Controller
	{
		private readonly IMp3Player _mp3Player;
		private readonly ISoundFilePicker _soundFilePicker;
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		

		public SoundPlayerController(IMp3Player mp3Player, ISoundFilePicker soundFilePicker)
		{
			_mp3Player = mp3Player;
			_soundFilePicker = soundFilePicker;	
		}

		public GetClipsResponse Get()
		{
			var getClipsResponse = new GetClipsResponse();
			var directories = _soundFilePicker.GetFolders();
			foreach (var directory in directories)
			{
				var files = _soundFilePicker.GetAllSoundFiles(directory);
				var folder = new Folder() {
					Name = Path.GetFileName(directory),
					Files = files.Select(Path.GetFileName).OrderBy(x=>x).ToList()
				};

				getClipsResponse.Folders.Add(folder);
			}
			return getClipsResponse;
		}

        [Microsoft.AspNetCore.Mvc.HttpGet("api/soundplayer/{folder}/{file}")]
		public PlayMp3FileResponse Get(string folder,string file)
		{
			return Get(Path.Combine(folder,file));
		}

		public PlayMp3FileResponse Get(string id)
		{
			var pickFile = _soundFilePicker.PickFile(id);
			if (pickFile != null)
			{
				_mp3Player.PlayFile(pickFile);
				return new PlayMp3FileResponse() {FileName = pickFile};
			}
			throw new HttpResponseException(HttpStatusCode.NotFound);
		}
	}

	
}