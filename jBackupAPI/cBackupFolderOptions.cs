using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


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

    internal class cBackupFolderOptions : IBackupFolderOptions {

        private string _Key = string.Empty;
        private string _Src = string.Empty;
        private string _Target = string.Empty;

        public cBackupFolderOptions(cBackupFolderConfigElement processConfigSettings) {

            _Key = processConfigSettings.ProcessKey;
            _Src = processConfigSettings.Src;
            _Target = processConfigSettings.Target;
        }

        public string ProcessKey {
            get {
                return(_Key);
            }
            set {
                _Key = value;
            }
        }

        public string Src {
            get {
                return (_Src);
            }
            set {
                _Src = value;
            }
        }

        public string Target {
            get {
                return (_Target);
            }
            set {
                _Target = value;
            }
        }
    }
}
