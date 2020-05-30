using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/******************************************************************************
** Copyright © 2016 All rights reserved
*******************************************************************************
*******************************************************************************
** DO NOT DISCLOSE THESE MATERIALS TO ANY THIRD PARTY.
*******************************************************************************
*
* Author: Joel Caples
* Date: 
*
* Purpose: 
*
* Modification History
* JMC 05/27/2016 
*   -Initial version.
*******************************************************************************/
namespace jBackupAPI {

    public class cBackupEngine {

        public cBackupEngine() {
        }

        private class cFileMetrics {
            
            public int filesFound = 0;
            public int filesCopied = 0;
            public int foldersFound = 0;
            public int foldersCopied = 0;

            public DateTime lastPound = DateTime.Now;
            public DateTime lastInfo = DateTime.Now;


            public void WriteMetrics() {
                if(DateTime.Now.Subtract(lastPound).TotalSeconds >= 1) {
                    lastPound = DateTime.Now;
                    Console.Write('#');
                }
                
                if(DateTime.Now.Subtract(lastInfo).TotalSeconds >= 90) {
                    lastInfo = DateTime.Now;
                    Console.WriteLine();
                    Console.WriteLine("Folders found (" + foldersFound.ToString() + ") copied (" + foldersCopied.ToString() + ")");
                    Console.WriteLine("Files found (" + filesFound.ToString() + ") copied (" + filesCopied.ToString() + ")");
                }
            }
        }

        public void Go() {
            foreach(IBackupFolderOptions proc in AppConfiguration.BackupFolderList) {
                try {
                    CopyFiles(proc.Src, proc.Target);
                } catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Go(int index) {
            try {
                CopyFiles(AppConfiguration.BackupFolderList[index].Src, AppConfiguration.BackupFolderList[index].Target);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void CopyFiles(string src, string dest) {
            cFileMetrics objFileMetrics = new cFileMetrics();

            Console.WriteLine();
            Console.WriteLine("Processing folder: " + src);
            Console.Write("#");

            CopyFiles(
                src,
                dest,
                ref objFileMetrics);

            Console.WriteLine();
            Console.WriteLine("Total Folders found (" + objFileMetrics.foldersFound.ToString() + ") copied (" + objFileMetrics.foldersCopied.ToString() + ")");
            Console.WriteLine("Total Files found (" + objFileMetrics.filesFound.ToString() + ") copied (" + objFileMetrics.filesCopied.ToString() + ")");
        }

        private void CopyFiles(
            string src,
            string dest,
            ref cFileMetrics fileMetrics) {

            if(!Directory.Exists(dest)) {
                Directory.CreateDirectory(dest);
                fileMetrics.foldersCopied++;
            }

            Dictionary<string, FileInfo> sourceFilesDictonary = Directory.GetFiles(src)
                                      .Select(file => new FileInfo(file))
                                      .ToDictionary(f => f.Name, f => f);

            Dictionary<string, FileInfo> destFilesDictonary = Directory.GetFiles(dest)
                                      .Select(file => new FileInfo(file))
                                      .ToDictionary(f => f.Name, f => f);

            fileMetrics.foldersFound++;

            foreach(FileInfo fi in sourceFilesDictonary.Values) {
                
                fileMetrics.filesFound++;

                try {
                    if(destFilesDictonary.ContainsKey(fi.Name)) {
                        if(destFilesDictonary[fi.Name].LastWriteTimeUtc < sourceFilesDictonary[fi.Name].LastWriteTimeUtc) {
                            File.Copy(Path.Combine(src, fi.Name), Path.Combine(dest, fi.Name), true);
                            fileMetrics.filesCopied++;
                        }
                    } else {
                        File.Copy(Path.Combine(src, fi.Name), Path.Combine(dest, fi.Name));
                        fileMetrics.filesCopied++;
                    }
                } catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }

                fileMetrics.WriteMetrics();
            }

            fileMetrics.WriteMetrics();

            Dictionary<string, DirectoryInfo> srcDirectoriesDictonary = Directory.GetDirectories(src)
                                                 .Select(d => new DirectoryInfo(d))
                                                 .ToDictionary(d => d.Name, d => d);

            foreach(DirectoryInfo srcDirInfo in srcDirectoriesDictonary.Values) {
                if(!AppConfiguration.ExclusionFolders.Contains(srcDirInfo.Name)) {
                    if(!Directory.Exists(Path.Combine(dest, srcDirInfo.Name))) {
                        Directory.CreateDirectory(Path.Combine(dest, srcDirInfo.Name));
                        fileMetrics.foldersCopied++;
                    }
                    CopyFiles(
                        Path.Combine(src, srcDirInfo.Name),
                        Path.Combine(dest, srcDirInfo.Name),
                        ref fileMetrics);
                }
            }
        }
    }
}
