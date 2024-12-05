namespace Route.Talabat.Dashboard.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 01. Get Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            //02. set fileName unique
            var fileExtention = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtention;

            //03.Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            //04. Save File as Stream
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                //05. Copy my file into streams
                file.CopyTo(fileStream);
            };



            //06. return fileName
            return Path.Combine("images",folderName,fileName);
            
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
