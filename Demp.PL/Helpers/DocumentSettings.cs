using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Demp.PL.Helpers
{
    public class DocumentSettings
    {
        //upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //1. get located folder path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            //2. get file name and make it unique
            string FileName = $"{file.FileName}";
            //3. get file path[folder path + file name]
            string FilePath = Path.Combine(FolderPath, FileName);
            //4. save file as streams 
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);
            //5.return file name
            return FileName;
        }
        //delete
        public static void DeleteFile(string FileName, string FolderName) 
        {
			//1.get file path
			string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, FileName);
			//2. check if file exsits or not
			if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

		}

	}
}
