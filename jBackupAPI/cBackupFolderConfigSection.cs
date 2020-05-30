using System;
using System.Collections.Generic;
using System.Configuration;
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
namespace jBackupAPI
{
   
    internal class cBackupFolderConfigSection : ConfigurationSection {
        /// <summary>
        /// The value of the property here "Process" needs to match that of the config file section
        /// </summary>
        [ConfigurationProperty("jBackupConfigSections")]
        public cBackupFolderConfigElementCollection ProcessItems {
            get {
                return ((cBackupFolderConfigElementCollection)(base["jBackupConfigSections"]));
            }
        }
    }

    /// <summary>
    /// The collection class that will store the list of each element/item that
    ///        is returned back from the configuration manager.
    /// </summary>
    [ConfigurationCollection(typeof(cBackupFolderConfigElement))]
    internal class cBackupFolderConfigElementCollection : ConfigurationElementCollection {

        protected override ConfigurationElement CreateNewElement() {
            return new cBackupFolderConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((cBackupFolderConfigElement)(element)).ProcessKey;
        }

        public cBackupFolderConfigElement this[int idx] {
            get {
                return (cBackupFolderConfigElement)BaseGet(idx);
            }
        }
    }
    /// <summary>
    /// The class that holds onto each element returned by the configuration manager.
    /// </summary>
    internal class cBackupFolderConfigElement : ConfigurationElement {
        private const string CONFIG_PROCESS_PROCESSKEY = "ProcessKey";
        private const string CONFIG_PROCESS_SRC = "Src";
        private const string CONFIG_PROCESS_TARGET = "Target";

        [ConfigurationProperty(CONFIG_PROCESS_PROCESSKEY, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ProcessKey {
            get {
                return ((string)(base[CONFIG_PROCESS_PROCESSKEY]));
            }
        }
        
        [ConfigurationProperty(CONFIG_PROCESS_SRC, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Src {
            get {
                return ((string)(base[CONFIG_PROCESS_SRC]));
            }
        }

        [ConfigurationProperty(CONFIG_PROCESS_TARGET, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Target {
            get {
                return ((string)(base[CONFIG_PROCESS_TARGET]));
            }
        }
    }
}
