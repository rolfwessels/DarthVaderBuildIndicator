using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
using BuildIndicatron.Shared.Models.ApiResponses;
using Nancy;
using log4net;

namespace BuildIndicatron.Server.Modules
{
    public class FileUpload : NancyModule
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public FileUpload()
        {
            Get["/"] = parameters => { return View["staticview", Request.Url]; };


            
        }

        private object SaveCall<T>(T input, Action<T> action)
        {
            Response asJson;
            try
            {
                Log.Debug("FileUpload:SaveCall Start action");
                action(input);
                Log.Debug("FileUpload:SaveCall done");
                asJson = Response.AsJson(input);
                Log.Debug("FileUpload:SaveCall Done with json");
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return View["FileUpload", input];
            }
            return asJson;
        }
    }
}