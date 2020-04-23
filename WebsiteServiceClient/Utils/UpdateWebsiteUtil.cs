using System;
using System.Collections.Generic;
using System.IO;
//using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebsiteServiceClient.Datas;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpCompress;
using System.IO;
using SharpCompress.Common;
using System.Text.RegularExpressions;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Writers.Zip;

namespace WebsiteServiceClient.Utils
{
    public class UpdateWebsiteUtil
    {
        public static void RestoreFile(WebsiteInfo websiteInfo)
        {
            var zipFile = GetRestoreFile(websiteInfo);
            if (zipFile == null) {
                Console.WriteLine("无备份文件..");

                return;
            }
            Console.WriteLine("开始恢复...");

            var dir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(zipFile));
            if (Directory.Exists(dir)) {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);

            //var fs = File.Open(zipFile, FileMode.CreateNew);
            //var ci = new ZipWriterOptions(CompressionType.LZMA);
            //ci.DeflateCompressionLevel = CompressionLevel.BestCompression;
            //using (ZipWriter w = new ZipWriter(fs, ci)) {
            //    var cDir = new DirectoryInfo(dir);
            //    ReadFolder(websiteInfo, cDir, w);
            //}
            //fs.Dispose();

            SharpCompress.Archives.ArchiveFactory.WriteToDirectory(zipFile, dir);
       

            var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            var app_offline = Path.Combine(websiteInfo.WebsiteFolder, "app_offline.htm");
            File.Create(app_offline).Close();
            foreach (var srcFile in files) {
                var tarFile = srcFile.Replace(dir, websiteInfo.WebsiteFolder);
                Directory.CreateDirectory(Path.GetDirectoryName(tarFile));
                while (true) {
                    try {
                        File.Copy(srcFile, tarFile, true);
                        break;
                    } catch (Exception) { }
                    System.Threading.Thread.Sleep(10);
                }
            }
            File.Delete(app_offline);
            Directory.Delete(dir, true);
            Console.WriteLine("恢复成功...");
        }

        public static string GetRestoreFile(WebsiteInfo websiteInfo)
        {
            var files = Directory.GetFiles(websiteInfo.BackupFolder, websiteInfo.Name + "_*.zip", SearchOption.AllDirectories).ToList();
            if (files.Count == 0) {
                return null;
            }
            files = files.OrderByDescending(q => q).ToList();
            return files[0];
        }

        public static void UpdateFiles(WebsiteInfo websiteInfo)
        {
            Console.WriteLine("项目：" + websiteInfo.Name);
            List<string> files = new List<string>();
            var cDir = new DirectoryInfo(websiteInfo.PreReleaseFolder);
            if (cDir.Exists == false) {
                Console.WriteLine("预发布文件不存在");
                return;
            }
            GetUpdateFile(websiteInfo, cDir, files);
            RemoveNoChnage(websiteInfo, websiteInfo.PreReleaseFolder, files);
            if (files.Count == 0) {
                Console.WriteLine("文件无更新");
                return;
            }
            FileBackup(websiteInfo);

            Console.WriteLine("开始更新...");
            var app_offline = Path.Combine(websiteInfo.WebsiteFolder, "app_offline.htm");
            File.Create(app_offline).Close();
            foreach (var srcFile in files) {
                var tarFile = srcFile.Replace(websiteInfo.PreReleaseFolder, websiteInfo.WebsiteFolder);
                Directory.CreateDirectory(Path.GetDirectoryName(tarFile));
                while (true) {
                    try {
                        File.Copy(srcFile, tarFile, true);
                        break;
                    } catch (Exception) { }
                    System.Threading.Thread.Sleep(10);
                }
            }
            File.Delete(app_offline);
            Console.WriteLine("更新成功...");
        }

        public static void RemoveNoChnage(WebsiteInfo websiteInfo, string rootFolder, List<string> outFiles)
        {
            for (int i = outFiles.Count - 1; i >= 0; i--) {
                var file = outFiles[i];
                var tarFile = file.Replace(rootFolder, websiteInfo.WebsiteFolder);
                if (File.Exists(tarFile) == false) { continue; } //新增的
                var src = new FileInfo(file);
                var tar = new FileInfo(tarFile);
                if (src.Length != tar.Length) { continue; } //修改的
                if (src.LastWriteTime != tar.LastWriteTime) { continue; } // 修改的
                outFiles.RemoveAt(i);// 未修改
            }
        }

        public static void GetUpdateFile(WebsiteInfo websiteInfo, DirectoryInfo cDir, List<string> outFiles)
        {
            var mainDir = websiteInfo.WebsiteFolder;
            var dirs = cDir.GetDirectories();
            foreach (var dir in dirs) {
                if (websiteInfo.BackupExclude.FolderName.Contains(dir.Name)) continue;
                GetUpdateFile(websiteInfo, dir, outFiles);
            }
            var files = cDir.GetFiles();
            foreach (var file in files) {
                bool Ignore = false;
                foreach (var item in websiteInfo.BackupExclude.FileName) {
                    var re = "^" + item.Replace(".", "\\.").Replace("*", ".*") + "$";
                    if (Regex.IsMatch(file.Name, re, RegexOptions.IgnoreCase)) Ignore = true;
                }
                if (Ignore) continue;
                outFiles.Add(file.FullName);
            }
        }

        public static void FileBackup(WebsiteInfo websiteInfo)
        {
            if (websiteInfo.UseBackup.ToLower() == "true") {
                var backupFileName = "";
                if (websiteInfo.BackupRate.ToLower() == "day") {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd") + ".zip");
                } else if (websiteInfo.BackupRate.ToLower() == "hour") {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd_HH") + ".zip");
                } else if (websiteInfo.BackupRate.ToLower() == "minute") {
                    backupFileName = Path.Combine(websiteInfo.BackupFolder, websiteInfo.Name + DateTime.Now.ToString("_yyyyMMdd_HHmm") + ".zip");
                } else {
                    Console.WriteLine("备份频率有误，不进行备份");
                    return;
                }

                if (File.Exists(backupFileName)) {
                    Console.WriteLine("已备份，不进行备份");
                    return;
                }
                Console.WriteLine("开始备份...");
                Directory.CreateDirectory(Path.GetDirectoryName(backupFileName));

                var fs = File.Open(backupFileName, FileMode.CreateNew);
                var ci = new ZipWriterOptions(CompressionType.LZMA);
                ci.DeflateCompressionLevel = CompressionLevel.BestCompression;
                using (ZipWriter w = new ZipWriter(fs, ci)) {
                    var cDir = new DirectoryInfo(websiteInfo.WebsiteFolder);
                    ReadFolder(websiteInfo, cDir, w);
                }
                fs.Dispose();

                //using (ZipFile zip = new ZipFile(Encoding.Default)) {
                //    var cDir = new DirectoryInfo(websiteInfo.WebsiteFolder);
                //    ReadFolder(websiteInfo, cDir, zip);
                //    var fs = File.Open(backupFileName, FileMode.CreateNew);
                //    zip.Save(fs);
                //    fs.Dispose();
                //}
                Console.WriteLine("备份成功...");
            } else {
                Console.WriteLine("不进行备份");
                return;
            }
        }


        public static void ReadFolder(WebsiteInfo websiteInfo, DirectoryInfo cDir, ZipWriter writer)
        {
            var mainDir = websiteInfo.WebsiteFolder;
            var dirs = cDir.GetDirectories();
            foreach (var dir in dirs) {
                if (websiteInfo.BackupExclude.FolderName.Contains(dir.Name)) continue;
                ReadFolder(websiteInfo, dir, writer);
            }
            var files = cDir.GetFiles();
            foreach (var file in files) {
                bool Ignore = false;
                foreach (var item in websiteInfo.BackupExclude.FileName) {
                    var re = "^" + item.Replace(".", "\\.").Replace("*", ".*") + "$";
                    if (Regex.IsMatch(file.Name, re, RegexOptions.IgnoreCase)) Ignore = true;
                }
                if (Ignore) continue;

                var filepath = file.FullName.Substring(mainDir.Length).TrimStart(@"\/".ToArray());

                writer.Write(filepath, File.Open(file.FullName, FileMode.Open), null);
            }
        }



    }
}
