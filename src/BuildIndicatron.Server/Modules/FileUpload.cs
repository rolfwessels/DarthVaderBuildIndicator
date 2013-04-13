using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
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


            Get[ApiPaths.FileUploadUpload] = x =>
                {
                    var model = new Index {Name = "Boss Hawg"};
                    return View["FileUpload", model];
                };

            Get[ApiPaths.FileUploadHasFileInArchive] = parameters =>
                {
                    return SaveCall(new FileUploadHasFileInArchiveResponse(), (call) =>
                        {
                            dynamic combine = Path.Combine(MainSettings.Default.FileStorage, parameters.FileName);
                            dynamic hasFile = File.Exists(combine);
                            Log.Info(string.Format("FileUploadHasFileInArchive {0} {1}", combine, hasFile));
                        });
                };

            Post[ApiPaths.FileUploadUpload] = x =>
                {
                    return SaveCall(new FileUploadUploadResponse(), call =>
                        {
                            var fileDetails = new List<string>();
                            foreach (HttpFile file in Request.Files)
                            {
                                Log.Warn("Post to file upload");

                                fileDetails.Add(string.Format("{3} - {0} ({1}) {2}bytes", file.Name, file.ContentType,
                                                              file.Value.Length, file.Key));
                                Log.Info(string.Format("FileUploadHasFileInArchive {0} ", fileDetails));
                                if (!Directory.Exists(MainSettings.Default.FileStorage))
                                {
                                    Directory.CreateDirectory(MainSettings.Default.FileStorage);
                                }
                                string combine = Path.Combine(MainSettings.Default.FileStorage, file.Name);
                                Log.Info(string.Format("CreatingFile {0}", Path.GetFullPath(combine)));
                                using (FileStream fileStream = File.OpenWrite(combine))
                                {
                                    file.Value.CopyTo(fileStream);
                                    fileStream.Flush();
                                }
                            }
                            call.FileDetails = fileDetails;
                        });
                };
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