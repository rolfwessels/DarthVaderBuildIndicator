using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Http;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Properties;
using BuildIndicatron.Shared.Models.ApiResponses;
using log4net;
using System.Linq;

namespace BuildIndicatron.Server.Controllers
{
	public class SoundPlayerController : ApiController
	{
		private readonly IMp3Player _mp3Player;
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly string _baseDir;

		public SoundPlayerController(IMp3Player mp3Player)
		{
			_mp3Player = mp3Player;
			_baseDir = PlatformHelper.AsPath(Path.GetFullPath(Settings.Default.SoundFileLocation));

			_log.Info(string.Format("Setting sound folder to {0}", _baseDir));
		}


		public GetClipsResponse Get()
		{
			
			var getClipsResponse = new GetClipsResponse();
			var directories = Directory.GetDirectories(_baseDir);
			foreach (var directory in directories)
			{
				var files = Directory.GetFiles(directory, "*.mp3").Union(Directory.GetFiles(directory, "*.wav"));
				var folder = new Folder() {
					Name = Path.GetFileName(directory),
					Files = files.Select(Path.GetFileName).OrderBy(x=>x).ToList()
				};

				getClipsResponse.Folders.Add(folder);
			}
			return getClipsResponse;
		}

		
		public PlayMp3FileResponse Get(string folder,string file)
		{
			return Get(Path.Combine(folder,file));
		}

		public PlayMp3FileResponse Get(string id)
		{
			var fileName = Path.Combine(_baseDir, id);
			_log.Info(string.Format("Trying to play file [{0}]", fileName));
			var isFile = File.Exists(fileName);
			if (isFile)
			{
				_mp3Player.PlayFile(fileName);
				return new PlayMp3FileResponse() {FileName = fileName};
			}
			var isDirectory = Directory.Exists(fileName);
			if (isDirectory)
			{
				var strings = Directory.GetFiles(fileName, "*.*");
				var random = strings.Random();
				_mp3Player.PlayFile(random);
				return new PlayMp3FileResponse() {FileName = random};
			}
			throw new HttpResponseException(HttpStatusCode.NotFound);
		}
	}
}