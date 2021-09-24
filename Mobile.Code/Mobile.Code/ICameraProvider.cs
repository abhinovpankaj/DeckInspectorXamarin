using System.Threading.Tasks;

namespace Mobile.Code
{
    public interface ICameraProvider
    {
        Task<CameraResult> TakePictureAsync();
    }
    public class CameraResult
    {
        public Xamarin.Forms.ImageSource Picture { get; set; }

        public string FilePath { get; set; }
    }
}
