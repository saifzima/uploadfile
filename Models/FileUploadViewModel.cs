using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPLOADING_FILE.Models
{
    public class FileUploadViewModel
    {
        public List<FileOnDataBaseModel> FileOnDataBaseModel {get; set;}
         public List<FileOnFileSystem> FileOnFileSystem {get; set;}
    }
}