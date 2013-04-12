using System.Linq;
using BuildIndicatron.Server.Models;
using Nancy;

namespace BuildIndicatron.Server.Modules
{
    
    public class FileUpload : NancyModule
    {
        public FileUpload()
        {
            Get["/"] = parameters => {
                return View["staticview", this.Request.Url];
            };

           
            Get["/fileupload"] = x =>
            {
                var model = new Index() { Name = "Boss Hawg" };
                return View["FileUpload", model];
            };

            Get["/fileupload"] = x =>
            {
                var model = new Index() { Name = "Boss Hawg" };
                return View["FileUpload", model];
            };

            Post["/fileupload"] = x =>
            {
                var model = new Index() { Name = "Boss Hawg" };

                var file = this.Request.Files.FirstOrDefault();
                string fileDetails = "None";

                if (file != null)
                {
                    fileDetails = string.Format("{3} - {0} ({1}) {2}bytes", file.Name, file.ContentType, file.Value.Length, file.Key);
                }

                model.Posted = fileDetails;

                return View["FileUpload", model];
            };
        }
    }

}