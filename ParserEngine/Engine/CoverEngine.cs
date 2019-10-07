using Jdenticon;
using ParserEngine.Models;
using SixLabors.Fonts;
using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace ParserEngine.Engine
{
    class CoverEngine
    {
        private const int height = 600;
        private const int width = 384;
        public async Task CreateImage(Book book, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            await Task.Run(() =>
            {
                using (var img = ConvertTextToImage(book.Author, book.Bookname))
                {

                    //img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                    var bData = File.ReadAllBytes(filePath);
                    book.EncodedImage = "data:image/jpeg;base64, " + Convert.ToBase64String(bData);
                }
            });
        }

        private IDisposable ConvertTextToImage(string author, string bookname)
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

            var temp = Path.GetTempFileName();
            using (var img = Image.Load("fb.jpg"))
            {
                // For production application we would recommend you create a FontCollection
                // singleton and manually install the ttf fonts yourself as using SystemFonts
                // can be expensive and you risk font existing or not existing on a deployment
                // by deployment basis.
                Font font = SystemFonts.Families.GetEnumerator().Current.CreateFont(10);// ("Arial", 10); // for scaling water mark size is largely ignored.

                using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(font, "A short piece of text", Color.HotPink, 5, false)))
                {
                    img2.Save("output/simple.png");
                }


                using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(font, LongText, Color.HotPink, 5, true)))
                {
                    img2.Save("output/wrapped.png");
                }
                return null;
            }
        }
        private static IImageProcessingContext ApplyScalingWaterMarkWordWrap(IImageProcessingContext processingContext,
   Font font,
   string text,
   Color color,
   float padding)
        {
            var imgSize = processingContext.GetCurrentSize();
            float targetWidth = imgSize.Width - (padding * 2);
            float targetHeight = imgSize.Height - (padding * 2);

            float targetMinHeight = imgSize.Height - (padding * 3); // must be with in a margin width of the target height

            // now we are working i 2 dimensions at once and can't just scale because it will cause the text to
            // reflow we need to just try multiple times

            var scaledFont = font;
            SizeF s = new SizeF(float.MaxValue, float.MaxValue);

            float scaleFactor = (scaledFont.Size / 2); // every time we change direction we half this size
            int trapCount = (int)scaledFont.Size * 2;
            if (trapCount < 10)
            {
                trapCount = 10;
            }

            bool isTooSmall = false;

            while ((s.Height > targetHeight || s.Height < targetMinHeight) && trapCount > 0)
            {
                if (s.Height > targetHeight)
                {
                    if (isTooSmall)
                    {
                        scaleFactor = scaleFactor / 2;
                    }

                    scaledFont = new Font(scaledFont, scaledFont.Size - scaleFactor);
                    isTooSmall = false;
                }

                if (s.Height < targetMinHeight)
                {
                    if (!isTooSmall)
                    {
                        scaleFactor = scaleFactor / 2;
                    }
                    scaledFont = new Font(scaledFont, scaledFont.Size + scaleFactor);
                    isTooSmall = true;
                }
                trapCount--;

                s = TextMeasurer.Measure(text, new RendererOptions(scaledFont)
                {
                    WrappingWidth = targetWidth
                });
            }

            var center = new PointF(padding, imgSize.Height / 2);
            var textGraphicOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                WrapTextWidth = targetWidth
            };
            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, center);
        }
    }
}
