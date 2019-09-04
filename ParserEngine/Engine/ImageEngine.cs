using Jdenticon;
using ParserEngine.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace DesktopParser.Engine
{
    public class ImageEngine
    {
        public async Task CreateImage(Book book, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            await Task.Run(() =>
            {
                using (var img = ConvertTextToImage(book.Author, book.Bookname))
                {
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                }
            });
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private Bitmap ConvertTextToImage(string author, string bookName)
        {
            var iconStyle = new IdenticonStyle
            {
                Padding = 0.10f,
                ColorSaturation = 0.4f,
                GrayscaleSaturation = 1.0f,
                ColorLightness = Range.Create(0.7f, 0.9f),
                GrayscaleLightness = Range.Create(0.3f, 0.9f)
            };
            var icon = Identicon.FromValue(bookName, 300);
            icon.Style = iconStyle;
            var bmp = new Bitmap(300,400);
            using (var graphics = Graphics.FromImage(bmp))
            {
                icon.Draw(graphics, new Jdenticon.Rendering.Rectangle(0, 0, bmp.Height, bmp.Height));
                var nameFont = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                var authorFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };

                SizeF s = graphics.MeasureString(bookName, nameFont);
                float fontScale = Math.Max(s.Width / bmp.Width, s.Height / bmp.Height);
                using (var font = new Font(FontFamily.GenericSansSerif, 20 / fontScale, GraphicsUnit.Point))
                {
                    graphics.DrawString(bookName, font, Brushes.Black, 150, 30, sf);
                }
                graphics.DrawString(author, authorFont, Brushes.Black, 150, 300, sf);
                graphics.Flush();
                nameFont.Dispose();
                authorFont.Dispose();
                graphics.Dispose();
            }
            return bmp;
        }
    }
}
