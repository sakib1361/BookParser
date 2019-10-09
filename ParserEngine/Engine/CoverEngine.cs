using Jdenticon;
using ParserEngine.Models;
using SixLabors.Fonts;
using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.Linq;

namespace ParserEngine.Engine
{
    public class CoverEngine
    {
        private const int height = 600;
        private const int width = 384;
        public async Task CreateImage(Book book, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            await Task.Run(() =>
            {
                ConvertTextToImage(book.Author, book.Bookname, filePath);
                var bData = File.ReadAllBytes(filePath);
                book.EncodedImage = "data:image/jpeg;base64, " + Convert.ToBase64String(bData);
            });
        }

        private void ConvertTextToImage(string author, string bookname, string filePath)
        {
            var iconStyle = new IdenticonStyle
            {
                Padding = 0.10f,
                ColorSaturation = 0.4f,
                GrayscaleSaturation = 1.0f,
                ColorLightness = Range.Create(0.7f, 0.9f),
                GrayscaleLightness = Range.Create(0.3f, 0.9f)
            };
            var icon = Identicon.FromValue(bookname, height);
            icon.Style = iconStyle;
            icon.SaveAsPng(filePath);

            var temp = Path.GetTempFileName();
            using (var img = Image.Load(filePath))
            {
                // For production application we would recommend you create a FontCollection
                // singleton and manually install the ttf fonts yourself as using SystemFonts
                // can be expensive and you risk font existing or not existing on a deployment
                // by deployment basis.
                FontFamily fontFamily = GetFont();
                
                Font font = fontFamily.CreateFont(40);
                Font book = fontFamily.CreateFont(30);
                img.Mutate(m => m.Resize(width, height));
                img.Mutate(m => DrawData(m, font, bookname, Color.Black, 50,15));
                img.Mutate(m => DrawData(m, book, author, Color.Black, height-50,15));
                img.Save(filePath);
            }
        }

        private static FontFamily FontFamily;
        private FontFamily GetFont()
        {
            if (FontFamily == null)
            {
             
                if (SystemFonts.TryFind("Nirmala Ui", out FontFamily win))
                {
                    FontFamily = win;
                }
                else if (SystemFonts.TryFind("San Francisco", out FontFamily mac))
                {
                    FontFamily = mac;
                }
                else if (SystemFonts.TryFind("DejaVu Sans", out FontFamily linux))
                {
                    FontFamily = linux;
                }
                else if (SystemFonts.TryFind("Lucida", out FontFamily other))
                {
                    FontFamily = other;
                }
                else
                {
                    FontFamily = SystemFonts.Families.FirstOrDefault();
                }
            }
            return FontFamily;
        }

        private void DrawData(IImageProcessingContext processingContext,
                              Font font,
                              string text,
                              Color color,
                              float paddingTop,
                              float paddingLeft)
        {
                var imgSize = processingContext.GetCurrentSize();
                float targetWidth = imgSize.Width - (paddingLeft * 2);
                var center = new PointF(paddingLeft, paddingTop);
                var textGraphicOptions = new TextGraphicsOptions(true)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    WrapTextWidth = targetWidth,
                };
                processingContext.DrawText(textGraphicOptions, text, font, color, center);
            }
    }
}
