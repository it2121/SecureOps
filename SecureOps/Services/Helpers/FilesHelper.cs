namespace SecureOps.Services.NewFolder
{
    public class FilesHelper
    {
        string _baseUrl;
        int _empId;
        public FilesHelper(string baseUrl, int empId)
        {
            _baseUrl = baseUrl;
            _empId = empId;
        }
        public async Task<string> GetAvatarUrlAsync()
        {
            try
            {
                string relativePath = "";
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars");

                string[] filePaths = Directory.GetFiles(basePath + "\\" + _empId);
                foreach (var filePath in filePaths)
                {


                    relativePath = filePath;


                }

                string FolderPathHolder = relativePath.StartsWith(@"wwwroot\")
                       ? relativePath.Substring(@"wwwroot\".Length)
                       : relativePath;

                int index = relativePath.IndexOf("wwwroot");
                if (index >= 0)
                {
                    relativePath = relativePath.Substring(index + "wwwroot".Length);
                }

                return relativePath;
            
            }
            catch
            {
                return $"images/default-avatar.png";


            }
        }
    }
}
