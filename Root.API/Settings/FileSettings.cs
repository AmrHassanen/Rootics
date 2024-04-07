
namespace CaptionGenerator.CORE.Settings
{
    public static class FileSettings
    {
        public const string AllowedExtensions = ".jpg,.jpeg,.png,.gif,.bmp";
        public const int MaxFileSizeInMB = 4;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
