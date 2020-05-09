using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using WebsiteService.MonitorTerminal.Datas;
using WebsiteService.MonitorTerminal.Mime;

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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            Process.Start(path);
            return Ok();
        }

        #region 删除 复制 移动 文件
        [HttpGet("File/DeleteFile")]
        public IActionResult DeleteFile(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            try
            {
                System.IO.File.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        [HttpGet("File/MoveFile")]
        public IActionResult MoveFile(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.File.Move(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        [HttpGet("File/CopyFile")]
        public IActionResult CopyFile(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.File.Copy(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }
        #endregion

        #region 创建 删除 复制 移动 文件夹
        [HttpGet("File/CreateFolder")]
        public IActionResult CreateFolder(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                System.IO.Directory.CreateDirectory(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }

        }

        [HttpGet("File/DeleteFolder")]
        public IActionResult DeleteFolder(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(404);
            }
            try
            {
                System.IO.Directory.Delete(path, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        [HttpGet("File/MoveFolder")]
        public IActionResult MoveFolder(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(404);
            }
            try
            {
                var tarFile = Path.Combine(folderName, Path.GetFileName(path));
                System.IO.Directory.Move(path, tarFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        [HttpGet("File/CopyFolder")]
        public IActionResult CopyFolder(string path, string folderName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(404);
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
                return StatusCode(404);
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
        #endregion


        [HttpGet("File/GetImage")]
        public IActionResult GetImage(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            return PhysicalFile(path, "plan/text");
        }

        #region 上传 下载 文件

        [HttpGet("File/DownloadFile")]
        public IActionResult DownloadFile(string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(path)] = path.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            var mime = new MimeMapper().GetMimeFromPath(path);
            return PhysicalFile(path, mime,true);
        }

        [HttpGet("File/UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, string path, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(file.FileName)] = file.FileName.ToString();
                keys[nameof(path)] = path.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            var fileName = file.FileName;
            var filePath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok();
            }
            catch (Exception) { }
            return StatusCode(404);
        }
        #endregion

        #region 压缩 解压
        [HttpGet("File/ZipFile")]
        public IActionResult ZipFile(string path, string fileName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
            }
            var archive = ArchiveFactory.Create(SharpCompress.Common.ArchiveType.Zip);
            archive.AddEntry(Path.GetFileName(path), new FileInfo(path));
            archive.SaveTo(Path.Combine(Path.GetDirectoryName(path), fileName), new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {
                ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding() { Default = Encoding.UTF8, }
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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.Directory.Exists(path) == false)
            {
                return StatusCode(404);
            }
            var archive = ArchiveFactory.Create(SharpCompress.Common.ArchiveType.Zip);
            var rootLength = Path.GetDirectoryName(path).Length;
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var key = file.Substring(rootLength + 1);
                archive.AddEntry(key, new FileInfo(file));
            }

            archive.SaveTo(Path.Combine(Path.GetDirectoryName(path), fileName), new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {
                ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding() { Default = Encoding.UTF8, }
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
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (System.IO.File.Exists(path) == false)
            {
                return StatusCode(404);
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

        #endregion

    }
}