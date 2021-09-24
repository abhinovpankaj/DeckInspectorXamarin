namespace Mobile.Code.Media
{
    public enum MediaFileType
    {
        Image,
        Video
    }

    public class MediaFile
    {
        public string PreviewPath { get; set; }
        public string Path { get; set; }
        public MediaFileType Type { get; set; }
    }
    public interface IMediaService
    {
        void OpenGallery();
        void ClearFileDirectory();
    }
    public interface ICompressImages
    {
        string CompressImage(string path);
    }
    //public enum MediaFileType
    //{
    //    Image,
    //    Video
    //}

    //public class MediaFile
    //{
    //    public string PreviewPath { get; set; }
    //    public string Path { get; set; }
    //    public MediaFileType Type { get; set; }
    //}
}
