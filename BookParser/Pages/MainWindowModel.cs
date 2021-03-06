﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using ParserEngine.Engine;
using ParserEngine.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BookParser
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowModel:ViewModelBase
    {
        private Website _website;
        private readonly CoreEngine CoreEngine;
        private readonly FileEngine FileEngine;
        public ObservableCollection<string> Logs { get; private set; }

        public string Url { get; set; }
        public string UrlPlaceholder { get; set; }
        public string Author { get; set; }
        public string Bookname { get; set; }
        public string ContentId { get; set; }
        public ParserType ContentIdType { get; set; }
        public string HeaderId { get; set; }
        public ParserType HeaderParserType { get; set; }
        public string NextPageId { get; set; }
        public ParserType NextPageIdType { get; set; }
        public string FolderPath { get; set; }
        public Website Website
        {
            get => _website;
            set => Update(value);
        }
        public List<Website> Websites => WebList.Websites;
        public List<ParserType> ParserTypes { get; private set; }
        public MainWindowModel()
        {
            CoreEngine = new CoreEngine();
            FileEngine = new FileEngine();
            ParserTypes = Enum.GetValues(typeof(ParserType)).Cast<ParserType>().ToList();
            Logs = new ObservableCollection<string>();
            LogEngine.InfoOccured += (s, e) => Logs.Insert(0, e);
            LogEngine.ErrorOccured += (s, e) => Logs.Insert(0, e);
        }

        private void Update(Website website)
        {
            _website = website;
            if (_website != null)
            {
                UrlPlaceholder = website.BaseUrl;
                ContentId = website.ContentData;
                ContentIdType = website.ContentType;
                HeaderId = website.HeaderData;
                HeaderParserType = website.HeaderType;
                NextPageId = website.NextData;
                NextPageIdType = website.NextType;
            }
        }
        public ICommand ParseCommand => new RelayCommand(ParseAction);
        public ICommand FolderCommand => new RelayCommand(FolderAction);

        public bool IsBusy { get; private set; }

        private void FolderAction()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    FolderPath = dialog.FileName;
                }
            }
        }

        private async void ParseAction()
        {
            if (string.IsNullOrEmpty(Url)) ShowMessage("Error", "Invalid Url");
            else if (string.IsNullOrEmpty(Author)) ShowMessage("Error", "Invalid Author");
            else if (string.IsNullOrEmpty(Bookname)) ShowMessage("Error", "Invalid bookname");
            else if (string.IsNullOrEmpty(ContentId))
                ShowMessage("Error", "You must define the class/id of the content");
            else if (string.IsNullOrEmpty(NextPageId))
                ShowMessage("Error", "You must define the class/id of the next page");
            else if (string.IsNullOrEmpty(HeaderId))
                ShowMessage("Error", "You must define the class/id of the next page");
            else if (string.IsNullOrEmpty(FolderPath))
                ShowMessage("Error", "You must define the export folder");
            else
            {
                var path = Path.GetTempPath() + Bookname;
                if (Directory.Exists(path)) Directory.Delete(path, true);
                Directory.CreateDirectory(path);
                var book = new Book()
                {
                    Author = Author,
                    Bookname = Bookname,
                    ContentInfo = new ParseInfo(ContentIdType, ContentId),
                    FilePath = path,
                    NextChapterInfo = new ParseInfo(NextPageIdType, NextPageId),
                    TitleInfo = new ParseInfo(HeaderParserType, HeaderId),
                    Url = Url
                };
                Logs.Clear();
                IsBusy = true;

                var parseRes = await CoreEngine.Parse(book);
                if (parseRes)
                {
                    var exportFile = Path.Combine(FolderPath, book.Bookname + ".epub");
                    await FileEngine.CreateBook(book, exportFile);
                }
                IsBusy = false;
            }
        }

        private void ShowMessage(string title, string message)
        {

        }
    }
}
