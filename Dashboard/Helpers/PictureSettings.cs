using Talabat.APIs.Services;

namespace Route.Talabat.Dashboard.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 01. Get Folder Server Path
            var folderServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets","img", folderName);

            // Ensure the folder exists
            if (!Directory.Exists(folderServerPath))
            {
                Directory.CreateDirectory(folderServerPath);
            }


            //02. set fileName unique
            var fileExtention = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtention;

            //03.Get File Path
            var fileServerPath = Path.Combine(folderServerPath, fileName);

            //04. Save File as Stream
            using (var fileStream = new FileStream(fileServerPath, FileMode.Create))
            {
                //05. Copy my file into streams
                file.CopyTo(fileStream);
            };

            //06. return FileStaticFolderPath
            return Path.Combine("assets","img",folderName,fileName);
        }

        public static void DeleteFile(string PictureURL)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",PictureURL);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
