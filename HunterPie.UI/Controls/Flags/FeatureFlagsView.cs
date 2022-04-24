using HunterPie.Core.Domain.Features.Domain;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Controls.Settings.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Flags
{
    public class FeatureFlagsView : ISettingElement
    {
        public string Title => "Feature Flags";

        public string Description => "Feature flags configuration";

        public ImageSource Icon => Resources.Icon("ICON_FLAG");

        private readonly ObservableCollection<ISettingElementType> _elements = new();
        public ObservableCollection<ISettingElementType> Elements => _elements;

        public FeatureFlagsView(IReadOnlyDictionary<string, IFeature> features)
        {
            foreach (var (featName, feat) in features)
            {
                var info = feat.GetType().GetProperty(nameof(IFeature.IsEnabled));
                //string name = featName.Replace("_", "__");
                ISettingElementType el = new SettingElementType(featName, featName, feat, info, true);
                _elements.Add(el);
            }
        }

        public void Add(ISettingElementType element)
        {
        }
    }
}
