using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using VessiFlowers.Business.Models.DomainModels;

namespace VessiFlowers.Web.App_Helpers
{
    public class MediaHelper
    {
        private const string imagesFolder = "~/Images";
        private const string thumbsFolder = imagesFolder + "/thumbs";

        private readonly int thumbSize = int.Parse(ConfigurationManager.AppSettings["thumbSize"]);

        private string fileName;
        private string fullPath;

        public void Save(HttpServerUtilityBase server, MediaViewModel mvm)
        {
            if (mvm.File != null && mvm.File.ContentLength > 0)
            {
                this.SaveImage(server, mvm);
                this.SaveThumbnail(server);

                mvm.Url = string.Format("{0}/{1}", imagesFolder.Substring(1, imagesFolder.Length - 1), fileName);
                mvm.DataSize = this.GetDataSize(fullPath);
            }
        }

        public void Delete(HttpServerUtilityBase server, string url)
        {
            var fileName = url.Split('/').Last();
            var filePath = Path.Combine(server.MapPath(imagesFolder), fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var thumbPath = Path.Combine(server.MapPath(thumbsFolder), fileName);
            if (File.Exists(thumbPath))
            {
                File.Delete(thumbPath);
            }
        }

        private void SaveImage(HttpServerUtilityBase server, MediaViewModel mvm)
        {
            this.fileName = mvm.File.FileName;
            this.fullPath = Path.Combine(server.MapPath(imagesFolder), this.fileName);

            string fileNameOnly = Path.GetFileNameWithoutExtension(this.fullPath);
            string extension = Path.GetExtension(this.fullPath);

            int count = 1;
            while (File.Exists(this.fullPath))
            {
                this.fileName = string.Format("{0}.{1}{2}", fileNameOnly, count++, extension);
                this.fullPath = Path.Combine(server.MapPath(imagesFolder), this.fileName);
            }

            mvm.File.SaveAs(this.fullPath);
        }

        private void SaveThumbnail(HttpServerUtilityBase server)
        {
            using (Image imgPhoto = Image.FromFile(this.fullPath))
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;

                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipX);
                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                int height = 0;
                int width = 0;
                if (sourceHeight > sourceWidth)
                {
                    ratio = (float)sourceHeight / sourceWidth;
                    height = thumbSize;
                    width = (int)(thumbSize / ratio);
                }
                else
                {
                    ratio = (float)sourceWidth / sourceHeight;
                    height = (int)(thumbSize / ratio);
                    width = thumbSize;
                }          

                using (Image thumbnail = imgPhoto.GetThumbnailImage(width, height, () => false, IntPtr.Zero))
                {
                    string fileName = this.fullPath.Split('\\').Last();
                    string path = Path.Combine(server.MapPath(thumbsFolder), fileName);
                    thumbnail.Save(path);
                }
            }
        }

        private string GetDataSize(string filePath)
        {
            string dataSize = string.Empty;

            using (Image imgPhoto = Image.FromFile(filePath))
            {
                dataSize = string.Format("{0}x{1}", imgPhoto.Width, imgPhoto.Height);
            }

            return dataSize;
        }
    }
}