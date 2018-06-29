using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerCrypto
{
    public class EnvironmentVariables
    {
        public static string FullPathToCertificatePfxFile
        {
            get
            {
                var workingDirectory = GetEnvironmentVariable("Fabric_Folder_App_Work");
                var certificatePfxFileName = GetEnvironmentVariable("Custom_PfxFileName");
                return Path.Combine(workingDirectory, certificatePfxFileName);
            }
        }

        public static string FullPathToCertificatePfxPasswordFile
        {
            get
            {
                var workingDirectory = GetEnvironmentVariable("Fabric_Folder_App_Work");
                var certificatePfxPasswordFileName = GetEnvironmentVariable("Custom_PfxPasswordFileName");
                return Path.Combine(workingDirectory, certificatePfxPasswordFileName);
            }
        }

        private static string GetEnvironmentVariable(string key)
        {
            var value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(value))
            {
                value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            }

            return value;
        }
    }
}
