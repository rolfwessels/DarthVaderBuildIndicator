﻿using BuildIndicatron.Server.Setup;
using Owin;

namespace BuildIndicatron.Server
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            
			BootStrap.Initialize(app);
			
		}
	}
}