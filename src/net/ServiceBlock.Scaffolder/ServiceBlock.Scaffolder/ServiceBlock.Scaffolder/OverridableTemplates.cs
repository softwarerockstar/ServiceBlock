using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder
{
    class OverridableTemplates : IDisposable
    {
        private readonly List<string> _templateFolders;

        public OverridableTemplates(string originalTemplateFolder, string solutionFolder)
        {
            var tempTemplatePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var templatesOverrideFolder = Path.Combine(solutionFolder, @"Templates\ServiceBlockCodeGenarator");

            DirectoryCopy(originalTemplateFolder, tempTemplatePath, true);

            if (Directory.Exists(templatesOverrideFolder))
                DirectoryCopy(templatesOverrideFolder, tempTemplatePath, true);

            _templateFolders = new List<string> { tempTemplatePath };
        }

        public IList<string> TemplateFolders
        {
            get { return _templateFolders; }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #region IDisposable Implementation
        // Flag: Has Dispose already been called? 
        bool _isDisposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            try
            {
                Directory.Delete(_templateFolders[0], true);
            }
            catch (IOException) { }
            finally
            {
                _isDisposed = true;
            }
            
        } 
        #endregion
    }
}
