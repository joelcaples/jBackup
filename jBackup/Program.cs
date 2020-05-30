using System;
using System.Collections.Generic;

using jBackupAPI;

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
namespace jBackup {

    class Program {

        static void Main(string[] args) {

            if(args.Length > 0 && args[0].Trim() == "0") {
                new cBackupEngine().Go();
            } else {

                List<int> arSelectedOptions = ShowMenu();

                if(arSelectedOptions.Count == 1 && arSelectedOptions[0] == -1) {
                    new cBackupEngine().Go();
                } else if(arSelectedOptions.Count > 1 && arSelectedOptions.Contains(-1)) {
                    throw(new InvalidOperationException());
                } else {
                    foreach(int intOption in arSelectedOptions) {
                        new cBackupEngine().Go(intOption);
                    }
                }

                Console.WriteLine("");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
        }

        private static List<int> ShowMenu() {

            int intIndex=1;
            //int[] intResult = {};
            List<int> arResult = new List<int>();
            int intTemp;
            string strConsoleInput;
            string[] strTemp;

            Console.WriteLine("##########################################################");
            Console.WriteLine("# 0 - Run All");
            foreach(IBackupFolderOptions proc in AppConfiguration.BackupFolderList) {
                Console.WriteLine("# " + intIndex++.ToString() + " - " + proc.ProcessKey);
            }
            Console.WriteLine("##########################################################");

            while(arResult.Count == 0) {
                Console.WriteLine(string.Empty);
                Console.Write("Select a job: ");

                try {
                    strConsoleInput = Console.ReadLine();
                    if(int.TryParse(strConsoleInput, out intTemp) && intTemp >= 0 && intTemp  <= AppConfiguration.BackupFolderList.Count) {
                        //intResult = new int[] { --intTemp };
                        arResult.Add(--intTemp);
                    } else if(strConsoleInput.Length > 0) {
                        strTemp = strConsoleInput.Split(',');
                        for(int i = 0;i < strTemp.Length;++i) {
                            if(int.TryParse(strTemp[i], out intTemp) && intTemp >= 0 && intTemp <= AppConfiguration.BackupFolderList.Count) {
                                if(strTemp.Length > 1 && (intTemp == 0)) {
                                    throw (new InvalidInputException());
                                } else {
                                    arResult.Add(--intTemp);
                                }
                            } else {
                                throw (new InvalidInputException());
                            }
                        }
                    }

                } catch(InvalidInputException e) {
                    Console.WriteLine(e.Message);
                }
            }

            return (arResult);
        }
    }

    public class InvalidInputException : Exception {
        public override string Message {
            get {
                return("Invalid input defined.  Choose an option from the menu or a comma delimited list of options to run.");
            }
        }
    }
}
