using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace BuildIndicatron.Server.Pi.AppStartup
{
    public class SimpleFileServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IEnumerable<string> _possibleWebBasePath;


        public static IEnumerable<string> PossibleWebBasePath
        {
            get
            {
                if (_possibleWebBasePath != null)
                    foreach (var path in _possibleWebBasePath)
                    {
                        yield return path;
                    }
                string combine = Path.Combine(new Uri(Assembly.GetExecutingAssembly().CodeBase).PathAndQuery,
                    @"..\..\..\");
                yield return
                    Path.GetFullPath(Path.Combine(combine, @"MainSolutionTemplate.Website2\dist"));
                yield return
                    Path.GetFullPath(Path.Combine(combine, @"MainSolutionTemplate.Website\build\debug"));
                yield return
                    Path.GetFullPath(Path.Combine(combine, @"MainSolutionTemplate.Website\dist"));
                
            }
            set { _possibleWebBasePath = value; }
        }

        public static void Initialize(IAppBuilder appBuilder)
        {
            string webBasePath = "wwwroot";
            if (!Directory.Exists(webBasePath))
            {
                foreach (string path in PossibleWebBasePath)
                {
                    if (Directory.Exists(path))
                    {
                        _log.Warn("Using alternative path to base path:" + Path.GetFullPath(path));
                        webBasePath = path;
                        break;
                    }
                    _log.Debug(string.Format("SimpleFileServer:Initialize Tried path {0}", path));
                }
            }
            var options = new FileServerOptions
            {
                FileSystem = new PhysicalFileSystem(webBasePath)
            };
            appBuilder.UseFileServer(options);
        }
    }
}