using System;
using System.Collections.Generic;
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

    public interface IBackupFolderOptions {

        string ProcessKey {
            get;
        }
        
        string Src {
            get;
        }

        string Target {
            get;
            set;
        }
    }
}
