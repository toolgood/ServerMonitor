using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using WebsiteService.MonitorTerminal.Datas;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public class FileController : ClientControllerBase
    {
        public FileController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        [HttpGet("File/GetFiles")]
        public IActionResult GetFiles(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }

            List<Datas.FolderInfo> folderFileInfos = new List<Datas.FolderInfo>();
            if (string.IsNullOrEmpty(path))
            {
                var drives = DriveInfo.GetDrives();
                for (int i = 0; i < drives.Length; i++)
                {
                    var d = drives[i];
                    if (d.DriveType != DriveType.Fixed) { continue; }
                    folderFileInfos.Add(new Datas.FolderInfo()
                    {
                        FileType = 2,
                        Path = d.Name,
                        Name = d.Name,
                    });
                }
                return Json(folderFileInfos);
            }

            var di = new DirectoryInfo(path);
            var directories = di.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                var d = directories[i];
                folderFileInfos.Add(new Datas.FolderInfo()
                {
                    FileType = 1,
                    Path = d.FullName,
                    Name = d.Name,
                    LastWriteTime = d.LastWriteTime,
                    CreationTime = d.CreationTime,
                });
            }
            var files = di.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                var f = files[i];
                folderFileInfos.Add(new Datas.FolderInfo()
                {
                    Path = f.FullName,
                    Name = f.Name,
                    Extension = f.Extension?.ToLower(),
                    Size = f.Length,
                    CreationTime = f.CreationTime,
                    LastWriteTime = f.LastWriteTime,
                });
            }
            return Json(folderFileInfos);
        }

        [HttpGet("File/GetOnlyFolders")]
        public IActionResult GetOnlyFolders(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }

            List<Datas.FolderInfo> folderFileInfos = new List<Datas.FolderInfo>();
            if (string.IsNullOrEmpty(path))
            {
                var drives = DriveInfo.GetDrives();
                for (int i = 0; i < drives.Length; i++)
                {
                    var d = drives[i];
                    if (d.DriveType != DriveType.Fixed) { continue; }
                    folderFileInfos.Add(new Datas.FolderInfo()
                    {
                        FileType = 2,
                        Path = d.Name,
                        Name = d.Name,
                    });
                }
                return Json(folderFileInfos);
            }

            var di = new DirectoryInfo(path);
            var directories = di.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                var d = directories[i];
                folderFileInfos.Add(new Datas.FolderInfo()
                {
                    FileType = 1,
                    Path = d.FullName,
                    Name = d.Name,
                    LastWriteTime = d.LastWriteTime,
                    CreationTime = d.CreationTime,
                });
            }
            return Json(folderFileInfos);
        }

        [HttpGet("File/RunFileExe")]
        public IActionResult RunFileExe(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            Process.Start(path);
            return Ok();
        }

        [HttpGet("File/DeleteFile")]
        public IActionResult DeleteFile(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                System.IO.File.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("File/MoveFile")]
        public IActionResult MoveFile(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.File.Move(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("File/CopyFile")]
        public IActionResult CopyFile(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.File.Copy(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("File/DeleteFolder")]
        public IActionResult DeleteFolder(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                System.IO.Directory.Delete(path, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("File/MoveFolder")]
        public IActionResult MoveFolder(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.Directory.Move(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("File/CopyFolder")]
        public IActionResult CopyFolder(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(500);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                CopyDirectory(path, tarFile);
                //System.IO.Directory.(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        private void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase)) { throw new Exception("父目录不能拷贝到子目录！"); }
            if (!source.Exists) { return; }
            if (!target.Exists) { target.Create(); }

            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                System.IO.File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }


        [HttpGet("File/GetImage")]
        public IActionResult GetImage(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            return PhysicalFile(path, "image/png");
        }

        [HttpGet("File/GetText")]
        public IActionResult GetText(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            return PhysicalFile(path, "plan/text");
        }

        [HttpGet("File/DownloadFile")]
        public IActionResult DownloadFile(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            return PhysicalFile(path, "application/octet-stream");
        }

        [HttpGet("File/UploadFile")]
        public IActionResult UploadFile(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {

            }
            return PhysicalFile(path, "plan/text");
        }

        [HttpGet("File/ZipFile")]
        public IActionResult ZipFile(string path, string fileName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            var archive = ArchiveFactory.Create(SharpCompress.Common.ArchiveType.Zip);
            archive.AddEntry(Path.GetFileName(path), new FileInfo(path));
            archive.SaveTo(Path.Combine(Path.GetDirectoryName(path), fileName), new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {

            });
            archive.Dispose();

            return Ok();
        }

        [HttpGet("File/ZipFolder")]
        public IActionResult ZipFolder(string path, string fileName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            //var di = new DirectoryInfo(path);
            var archive = ArchiveFactory.Create(SharpCompress.Common.ArchiveType.Zip);
            archive.AddAllFromDirectory(path);
           
            archive.SaveTo(Path.Combine(Path.GetDirectoryName(path), fileName), new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {

            });
            archive.Dispose();

            return Ok();
        }

        [HttpGet("File/UnZipFile")]
        public IActionResult UnZipFile(string path, string destinationDirectory, string password, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(500);
            }
            IArchive archive;
            if (string.IsNullOrEmpty(password))
            {
                archive = ArchiveFactory.Open(path);
            }
            else
            {
                archive = ArchiveFactory.Open(path, new SharpCompress.Readers.ReaderOptions()
                {
                    Password = password
                });
            }
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destinationDirectory, new SharpCompress.Common.ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true,
                        PreserveFileTime = true,
                        PreserveAttributes = true
                    });
                }
                else
                {
                    var dir = System.IO.Path.Combine(destinationDirectory, entry.Key);
                    Directory.CreateDirectory(dir);
                }
            }
            return Ok();
        }


    }
}