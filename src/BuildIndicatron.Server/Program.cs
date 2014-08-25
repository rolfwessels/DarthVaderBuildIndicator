using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared;
using Microsoft.Owin.Hosting;
using Raspberry.IO.GeneralPurpose;
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
		        _log.Info("Start server");

				if (args.Length > 0)
				{

					var line = "Exit";
					while (line != "")
					{
						System.Console.Out.WriteLine("Enter pin details:");
						//line = System.Console.In.ReadLine();

						PinManager.RunSample(ConnectorPin.P1Pin22);
					}
					System.Console.Out.WriteLine("Done");
					return;
				}

				Owin();
		        _log.Info("Closing");
	        }
	        catch (Exception e)
	        {
		        Console.Out.WriteLine(e.Message);
		        _log.Error(e.Message, e);
	        }
        }

	    private static void Owin()
	    {
		    var options = new StartOptions
			    {
				    ServerFactory = "Nowin",
				    Port = 8080
			    };

		    using (WebApp.Start<Startup>(options))
		    {
			    string localIp = "?";
				IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress ip in host.AddressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						localIp = ip.ToString();
					}
				}	
				Console.WriteLine(string.Format("Running a http server on port http://{0}:{1}",localIp, options.Port));
				while (true)
				{
					Thread.Sleep(1000);
				}
		    }
	    }

    }
}
