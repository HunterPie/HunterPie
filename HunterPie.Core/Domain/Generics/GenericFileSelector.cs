using HunterPie.Core.Settings.Types;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Domain.Generics
{
    public class GenericFileSelector : IFileSelector
    {
        private readonly string _filter;
        private readonly string _basePath;
        private readonly ObservableCollection<object> _elements = new ObservableCollection<object>();

        public object Current { get; set; }

        public ObservableCollection<object> Elements
        {
            get
            {
                _elements.Clear();
                foreach (object file in ListFiles())
                {
                    _elements.Add(file);
                }
                return _elements;
            }
        }

        public GenericFileSelector(object current, string filter, string basePath)
        {
            Current = current;
            _filter = filter;
            _basePath = basePath;
        }

        private object[] ListFiles()
        {
            if (!Directory.Exists(_basePath))
                return Array.Empty<string>();

            return Directory.GetFiles(_basePath, _filter)
                .Select(f => Path.GetFileName(f))
                .ToArray();
        }
    }
}
