﻿using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.ApiResponses
{
    public class FileUploadUploadResponse : BaseResponse
    {
        
        public List<string> FileDetails { get; set; }
    }
}
