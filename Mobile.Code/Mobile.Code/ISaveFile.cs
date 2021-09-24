using System.Threading.Tasks;

namespace Mobile.Code
{
    public interface ISaveFile
    {
        Task<string> SaveFiles(string filename, byte[] bytes);
        Task<string> SaveFilesForCameraApi(string filename, byte[] bytes);

    }
}
