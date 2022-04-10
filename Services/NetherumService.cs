using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public class NetherumService : INetherumService
    {
        public NetherumService()
        {

        }

        public string VerifySignature(string message, string address, string signature)
        {
            var signer = new EthereumMessageSigner();

            var addressRecovered = signer.EncodeUTF8AndEcRecover(message, signature);

            if (addressRecovered.ToLower() == address.ToLower())
            {
                return addressRecovered;
            }
            else
            {
                return null;
            }
        }
    }
}
