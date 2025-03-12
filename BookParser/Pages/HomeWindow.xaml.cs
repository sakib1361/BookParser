using AngleSharp;
using Jdenticon;
using ParserEngine.Engine;
using ParserEngine.Engines;
using ParserEngine.Models;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using DesktopParser.Engine;

namespace BookParser.Pages
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window, IUrlParser
    {
        private bool IsDownloading;
        private TaskCompletionSource<AngleSharp.Dom.IDocument> _tcs;
        private Book _currentBook;
        private readonly BanglaLibraryEngine _engine = new();
        private readonly IBrowsingContext _context = BrowsingContext.New(Configuration.Default);
        public HomeWindow()
        {
            InitializeComponent();
            var logData = new ObservableCollection<string>();
            LogEngine.Init(logData);
            Browser.Address = "https://ebanglalibrary.com";
        }

        private string FolderAction()
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Time to select a folder",
                UseDescriptionForTitle = true,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                              + Path.DirectorySeparatorChar,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else return string.Empty;
        }

        private async void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var ht = await e.Frame.GetSourceAsync();
                if (IsDownloading)
                    await HandleDownloadSource(ht);
                else
                    await HandleWebSource(ht);
            }
        }

        private async Task HandleDownloadSource(string ht)
        {
            var document = await _context.OpenAsync(x => x.Content(ht));
            _tcs?.TrySetResult(document);
        }

        private async Task HandleWebSource(string ht)
        {
            var document = await _context.OpenAsync(x => x.Content(ht));
            _currentBook = _engine.GetBookInfo(document);
            await Dispatcher.InvokeAsync(() =>
            {
                DButton.Visibility = _currentBook == null ? Visibility.Collapsed : Visibility.Visible;
            });

        }

        public async Task<AngleSharp.Dom.IDocument> OpenAsync(string url)
        {
            _tcs?.TrySetCanceled();
            _tcs = null;
            DownloadOutput.Text = "Opening.... "+ url;
            await Task.Delay(500);
            _tcs = new TaskCompletionSource<AngleSharp.Dom.IDocument>();
            Browser.Address = url;
            return await _tcs.Task;
        }

        private async void DButton_Click(object sender, RoutedEventArgs e)
        {
            var address = Browser.Address;
            IsDownloading = true;
            TopGrid.Visibility = Visibility.Visible;
            var outFolder = FolderAction();

            var res = await _engine.Parse(_currentBook, this);
            if (res)
            {
                var outFile = Path.Combine(outFolder, _currentBook.Bookname + ".epub");
                await FileEngine.CreateBook(_currentBook, outFile);
            }

            IsDownloading =false;

            TopGrid.Visibility = Visibility.Collapsed;
            await Task.Delay(1000);
            Browser.Address = address;
        }
        //private async void Search_Click(object sender, RoutedEventArgs e)
        //{
        //    if (PrBox.Visibility == Visibility.Visible || string.IsNullOrEmpty(UrlBox.Text))
        //    {
        //        return;
        //    }
        //    _currentBook = null;
        //    PrBox.Visibility = Visibility.Visible;
        //    LogEngine.Clear();
        //    using var engine = new BanglaLibraryEngine();
        //    var book = await engine.GetBookInfo(UrlBox.Text);
        //    if (book != null)
        //    {
        //        _currentBook = book;
        //        NameBox.Text = _currentBook.Bookname;
        //        AuthorBox.Text = _currentBook.Author;

        //        byte[] binaryData = Convert.FromBase64String(_currentBook.EncodedImage);

        //        BitmapImage bi = new BitmapImage();
        //        bi.BeginInit();
        //        bi.StreamSource = new MemoryStream(binaryData);
        //        bi.EndInit();
        //        ImageBox.Source = bi;
        //    }
        //    PrBox.Visibility = Visibility.Hidden;
        //}


        //private async void DownButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_currentBook == null || PrBox.Visibility == Visibility.Visible)
        //    {
        //        return;
        //    }

        //    var outFolder = FolderAction();
        //    if (string.IsNullOrWhiteSpace(outFolder))
        //    {
        //        return;
        //    }
        //    LogEngine.Clear();
        //    _currentBook.Chapters.Clear();
        //    _currentBook.Author = AuthorBox.Text;
        //    _currentBook.Bookname = NameBox.Text;
        //    if (ResetImage.IsChecked.Value)
        //    {
        //        _currentBook.EncodedImage = null;
        //    }
        //    PrBox.Visibility = Visibility.Visible;
        //    using var engine = new BanglaLibraryEngine();
        //    var res = await engine.Parse(_currentBook);
        //    if (res)
        //    {
        //        var outFile = Path.Combine(outFolder, _currentBook.Bookname + ".epub");
        //        await FileEngine.CreateBook(_currentBook, outFile);
        //    }
        //    PrBox.Visibility = Visibility.Hidden;
        //}
    }
}
