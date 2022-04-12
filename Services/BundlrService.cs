using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public class BundlrService : IBundlrService
    {
        private Process cmd;

        private readonly string network = "https://devnet.bundlr.network";
        private readonly string provider = "https://rinkeby.boba.network";
        private readonly string contractAddress = "0xF5B97a4860c1D81A1e915C40EcCB5E4a5E6b8309";
        private readonly string privateKey = "90b94d5daf40b6bb0d4b9217281503b375397edf99d22678abd107138273e038";

        public decimal GetUploadFee(long fileSize)
        {
            var arg = "bundlr " +
                $"price {fileSize} " +
                $"-h {network} " +
                $"--provider-url {provider} " +
                $"--contract-address {contractAddress} " +
                "-c boba";

            MakeProcess(arg);
            var output = Execute();

            var strFee = output.Substring(output.IndexOf("(") + 1, output.IndexOf("boba)") - output.IndexOf("(") - 2);

            decimal fee;
            if (decimal.TryParse(strFee, out fee))
            {
                return fee;
            }
            else
            {
                return decimal.Zero;
            }

            //return fee;
        }

        public string UploadFile(string filePath)
        {
            var arg = "bundlr " +
                $"upload {filePath} " +
                $"-w {privateKey} " +
                $"-h {network} " +
                $"--provider-url {provider} " +
                $"--contract-address {contractAddress} " +
                "-c boba";

            try
            {
                MakeProcess(arg);
                var output = Execute();

                var fileUrl = output.Substring(output.IndexOf("to") + 3, output.Length - output.IndexOf("to") - 3);

                return fileUrl;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void MakeProcess(string arguments)
        {
            cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Windows\\system32\\cmd.exe",
                    Arguments = $"/c {arguments} /t",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };
        }

        private string Execute()
        {
            cmd.Start();
            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            cmd.Close();
            cmd.Dispose();

            return output;
        }
    }
}
