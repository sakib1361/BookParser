using Jdenticon;
using ParserEngine.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;

namespace DesktopParser.Engine
{
    public class ImageEngine
    {
        private const int height = 600;
        private const int width = 384;
        public async Task CreateImage(Book book, string filePath, string fontName = "")
        {
            
            if (File.Exists(filePath)) File.Delete(filePath);
            await Task.Run(() =>
            {
                using (var img = ConvertTextToImage(book.Author, book.Bookname, fontName))
                {
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                    var bData = File.ReadAllBytes(filePath);
                    book.EncodedImage = "data:image/jpeg;base64, " + Convert.ToBase64String(bData);
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

        private Bitmap ConvertTextToImage(string author, string bookName, string fontname)
        {
            if (string.IsNullOrWhiteSpace(fontname))
            {
                fontname = FontFamily.GenericSansSerif.Name;
            }
            
            var iconStyle = new IdenticonStyle
            {
                Padding = 0.10f,
                ColorSaturation = 0.4f,
                GrayscaleSaturation = 1.0f,
                ColorLightness = Range.Create(0.7f, 0.9f),
                GrayscaleLightness = Range.Create(0.3f, 0.9f)
            };
            var icon = Identicon.FromValue(bookName, height);
            icon.Style = iconStyle;

            var bmp = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                icon.Draw(graphics, new Jdenticon.Rendering.Rectangle(0, 0, bmp.Height, bmp.Height));

                var nameFont = new Font(fontname, 20, FontStyle.Bold);
                var authorFont = new Font(fontname, 12, FontStyle.Regular);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near
                };

                SizeF s = graphics.MeasureString(bookName, nameFont);
                float fontScale = s.Width / bmp.Width;

                using (var font = new Font(fontname, 20 / fontScale, GraphicsUnit.Point))
                {
                    graphics.DrawString(bookName, font, Brushes.Black, 0, 30, sf);
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
