using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


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

    public static class AppConfiguration {

        private static List<IBackupFolderOptions> _MyBackupFolderList = null;
        private static List<string> _ExclusionFolders = null;

        public static List<IBackupFolderOptions> BackupFolderList {
            get {
                if(_MyBackupFolderList == null) {
                    _MyBackupFolderList = new List<IBackupFolderOptions>();

                    cBackupFolderConfigSection arClientProcess = (cBackupFolderConfigSection)ConfigurationManager.GetSection("jBackupSettings");

                    if(arClientProcess.ProcessItems.Count > 0) {
                        foreach(cBackupFolderConfigElement xproc in arClientProcess.ProcessItems) {
                            _MyBackupFolderList.Add(new cBackupFolderOptions(xproc));
                        }
                    }
                }

                return (_MyBackupFolderList);
            }
        }

        internal static List<string> ExclusionFolders {
            get {
                if(_ExclusionFolders == null) {

                    _ExclusionFolders = (ConfigurationManager.AppSettings["ExclusionFolders"] == null ? new List<string>() : ConfigurationManager.AppSettings["ExclusionFolders"].Split(',').ToList());

                    if(_ExclusionFolders.Count > 0) {
                        for(int i = 0;i < _ExclusionFolders.Count;++i) {
                            _ExclusionFolders[i] = _ExclusionFolders[i].Trim();
                        }
                    }
                }

                return (_ExclusionFolders);
            }
        }
    }
}
