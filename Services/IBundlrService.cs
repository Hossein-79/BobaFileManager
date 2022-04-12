namespace BobaFileManager.Services
{
    public interface IBundlrService
    {
        decimal GetUploadFee(long fileSize);
        string UploadFile(string filePath);
    }
}