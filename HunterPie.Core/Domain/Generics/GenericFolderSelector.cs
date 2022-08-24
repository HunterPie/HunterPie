using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using HunterPie.Core.Settings.Types;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace HunterPie.Core.Domain.Generics
{
    [JsonConverter(typeof(FileSelectorConverter))]
    public class GenericFolderSelector : Bindable, IFileSelector
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

        [JsonConstructor]
        public GenericFolderSelector(string current)
        {
            Current = current;
        }

        public GenericFolderSelector(string current, string filter, string basePath)
        {
            Current = current;
            _filter = filter;
            _basePath = basePath;
        }

        private string[] ListFiles()
        {
            if (!Directory.Exists(_basePath))
                return Array.Empty<string>();

            return Directory.GetDirectories(_basePath, _filter)
                .Select(f => Path.GetFileName(f))
                .ToArray();
        }

        public static implicit operator string(GenericFolderSelector selector) => selector.Current;
    }
}
