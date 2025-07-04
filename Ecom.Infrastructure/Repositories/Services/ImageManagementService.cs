using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImagesAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc = new List<string>();
            string imageDirictory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(imageDirictory) is not true)
            {
                Directory.CreateDirectory(imageDirictory);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName=file.FileName;
                    var imageSrc=$"/Images/{src}/{fileName}";
                    var root=Path.Combine(imageDirictory, fileName);
                    using (FileStream stream=new FileStream(root,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    saveImageSrc.Add(imageSrc);
                }
            }
            return saveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info=_fileProvider.GetFileInfo(src);
            var root=info.PhysicalPath;
            File.Delete(root);
        }
    }
}
