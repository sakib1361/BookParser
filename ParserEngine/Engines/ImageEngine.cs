using Jdenticon;
using ParserEngine.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace DesktopParser.Engine
{
    public class ImageEngine
    {
        private const int height = 640;
        private const int width = 400;


        public static async Task CreateImage(Book book, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            if (string.IsNullOrWhiteSpace(book.EncodedImage))
            {
                string fontName = FontFamily.GenericSansSerif.Name;
                using var img = ConvertTextToImage(book.Author, book.Bookname, fontName);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
            }
            else
            {
                await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(book.EncodedImage));
            }
        }

        public static void WriteImageTest(string name, string author, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            string fontName = FontFamily.GenericSansSerif.Name;
            using var img = ConvertTextToImage(author, name, fontName);
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
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

        public static Bitmap ConvertTextToImage(string author, string bookName, string fontname)
        {
            var fontFamily = FontFamily.Families.FirstOrDefault(x => x.Name == fontname);
           
            var iconStyle = new IdenticonStyle
            {
                Padding = 0.10f,
                ColorSaturation = 0.4f,
                GrayscaleSaturation = 1.0f,
                ColorLightness = Jdenticon.Range.Create(0.7f, 0.9f),
                GrayscaleLightness = Jdenticon.Range.Create(0.3f, 0.9f)
            };
            var icon = Identicon.FromValue(bookName, height);
            icon.Style = iconStyle;

            var bmp = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                icon.Draw(graphics, new Jdenticon.Rendering.Rectangle(0, 0, bmp.Height, bmp.Height));

                var nameFont = new Font(fontFamily, 20);
                var authorFont = new Font(fontFamily, 12);
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
                graphics.DrawString(author, authorFont, Brushes.Black, 50, 540, sf);
                graphics.Flush();
                nameFont.Dispose();
                authorFont.Dispose();
                graphics.Dispose();
            }
            return bmp;
        }

        internal async Task<string> Parse(string name, string author, string imgElement)
        {
            if (!string.IsNullOrEmpty(imgElement))
            {
                using var client = new HttpClient();
                var resp = await client.GetAsync(imgElement);
                if (resp.IsSuccessStatusCode)
                {
                    var stream = await resp.Content.ReadAsByteArrayAsync();
                    return Convert.ToBase64String(stream);
                }
                else
                {
                    imgElement+=".webp";
                    resp = await client.GetAsync(imgElement);
                    if (resp.IsSuccessStatusCode)
                    {
                        var stream = await resp.Content.ReadAsByteArrayAsync();
                        return Convert.ToBase64String(stream);
                    }
                }
            }

            var coverImg = Path.Combine("D:", "cover.jpeg");
            WriteImageTest(name, author, coverImg);
            var bData = File.ReadAllBytes(coverImg);
            return Convert.ToBase64String(bData);
        }

        private void WriteOnBitmap(Graphics graphics, Bitmap bmp, string bookName, string author)
        {
            var fontFamily = FontFamily.GenericSansSerif;
            var nameFont = new Font(fontFamily, 20);
            var authorFont = new Font(fontFamily, 12);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near
            };

            SizeF s = graphics.MeasureString(bookName, nameFont);
            float fontScale = s.Width / bmp.Width;

            using (var font = new Font(fontFamily.Name, 20 / fontScale, GraphicsUnit.Point))
            {
                graphics.DrawString(bookName, font, Brushes.Black, 0, 30, sf);
            }
            graphics.DrawString(author, authorFont, Brushes.Black, 50, 540, sf);
            graphics.Flush();
            nameFont.Dispose();
            authorFont.Dispose();
        }
    }
}
