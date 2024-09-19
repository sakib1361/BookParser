using ParserEngine.Engine;
using ParserEngine.Engines;
using ParserEngine.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace BookParser.Pages
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private Book _currentBook;
        public HomeWindow()
        {
            InitializeComponent();
            var logData = new ObservableCollection<string>();
            LogEngine.Init(logData);
            LogBox.ItemsSource = logData;
            UrlBox.Text = "https://www.ebanglalibrary.com/books/%e0%a6%9c%e0%a7%80%e0%a6%ac%e0%a6%a8-%e0%a6%95%e0%a6%be%e0%a6%b9%e0%a6%bf%e0%a6%a8%e0%a6%bf-%e0%a6%b6%e0%a6%95%e0%a7%8d%e0%a6%a4%e0%a6%bf%e0%a6%aa%e0%a6%a6-%e0%a6%b0%e0%a6%be%e0%a6%9c/#google_vignette";
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            if (PrBox.Visibility == Visibility.Visible || string.IsNullOrEmpty(UrlBox.Text))
            {
                return;
            }
            _currentBook = null;
            PrBox.Visibility = Visibility.Visible;
            LogEngine.Clear();
            using var engine = new BanglaLibraryEngine();
            var book = await engine.GetBookInfo(UrlBox.Text);
            if (book != null)
            {
                _currentBook = book;
                NameBox.Text = _currentBook.Bookname;
                AuthorBox.Text = _currentBook.Author;

                byte[] binaryData = Convert.FromBase64String(_currentBook.EncodedImage);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(binaryData);
                bi.EndInit();
                ImageBox.Source = bi;
            }
            PrBox.Visibility = Visibility.Hidden;
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

        private async void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBook == null || PrBox.Visibility == Visibility.Visible)
            {
                return;
            }

            var outFolder = FolderAction();
            if (string.IsNullOrWhiteSpace(outFolder))
            {
                return;
            }
            LogEngine.Clear();
            _currentBook.Chapters.Clear();
            _currentBook.Author = AuthorBox.Text;
            _currentBook.Bookname = NameBox.Text;
            if (ResetImage.IsChecked.Value)
            {
                _currentBook.EncodedImage = null;
            }
            PrBox.Visibility = Visibility.Visible;
            using var engine = new BanglaLibraryEngine();
            var res = await engine.Parse(_currentBook);
            if (res)
            {
                var outFile = Path.Combine(outFolder, _currentBook.Bookname + ".epub");
                await FileEngine.CreateBook(_currentBook, outFile);
            }
            PrBox.Visibility = Visibility.Hidden;
        }
    }
}
