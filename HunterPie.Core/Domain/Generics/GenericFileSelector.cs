using HunterPie.Core.Architecture;
using HunterPie.Core.Settings.Types;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Domain.Generics
{
    public class GenericFileSelector : Notifiable, IFileSelector
    {
        private string _current;
        private readonly string _filter;
        private readonly string _basePath;
        private readonly ObservableCollection<string> _elements = new ObservableCollection<string>();

        public string Current
        {
            get => _current;
            set { SetValue(ref _current, value); }
        }

        public ObservableCollection<string> Elements
        {
            get
            {
                _elements.Clear();
                foreach (string file in ListFiles())
                {
                    _elements.Add(file);
                }
                return _elements;
            }
        }


        public GenericFileSelector(string current, string filter, string basePath)
        {
            Current = current;
            _filter = filter;
            _basePath = basePath;
        } 

        private string[] ListFiles()
        {
            if (!Directory.Exists(_basePath))
                return Array.Empty<string>();

            return Directory.GetFiles(_basePath, _filter)
                .Select(f => Path.GetFileName(f))
                .ToArray();
        }
    }
}
