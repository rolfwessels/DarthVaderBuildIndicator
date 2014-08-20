using System;
using System.IO;
using System.Reflection;
using BuildIndicatron.Shared;
using Nancy.Hosting.Self;
using log4net;
using log4net.Config;

namespace BuildIndicatron.Server
{
    public class Program
    {
	    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {
	        try
	        {
		        XmlConfigurator.Configure();
		        _log.Info("Start");
		        Console.Out.WriteLine("Hello");
//		        var nancyHost = new NancyHost(new Uri(ApiPaths.LocalHost));
//		        nancyHost.Start();
//		        Console.WriteLine("Nancy now listening - navigating to {0}. Press enter to stop", ApiPaths.LocalHost);
//		        Console.ReadKey();
//		        nancyHost.Stop();
//		        Console.WriteLine("Stopped. Good bye!");
		        _log.Info("Closing");
	        }
	        catch (Exception e)
	        {
		        _log.Error(e.Message, e);
	        }
        }


       
    }
}
