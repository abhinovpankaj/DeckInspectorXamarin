using System.Threading.Tasks;

namespace Mobile.Code
{
    public interface IScreenshotManager
    {
        Task<byte[]> CaptureAsync();
    }
}
