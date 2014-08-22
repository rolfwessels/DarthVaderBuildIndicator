using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.ApiResponses
{
    public class GetClipsResponse : BaseResponse
    {
	    public GetClipsResponse()
	    {
			Folders = new List<Folder>();
	    }

	    public List<Folder> Folders { get; set; }
    }

    public class Folder
    {
	    public Folder()
	    {
			Files = new List<string>();
	    }

	    public List<string> Files { get; set; }
        public string Name { get; set; }
    }
}