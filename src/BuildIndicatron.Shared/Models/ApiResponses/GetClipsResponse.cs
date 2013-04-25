using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.ApiResponses
{
    public class GetClipsResponse : BaseResponse
    {
        public List<Folder> Folders { get; set; }
    }

    public class Folder
    {
        public List<string> Files { get; set; }
        public string Name { get; set; }
    }
}