using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public interface INetherumService
    {
        string VerifySignature(string message, string address, string signature);
    }
}