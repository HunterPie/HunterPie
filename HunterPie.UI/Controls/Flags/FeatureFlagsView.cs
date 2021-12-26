using HunterPie.Core.Domain.Features.Domain;
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

        public ImageSource Icon => Application.Current.FindResource("ICON_FLAG") as ImageSource;

        private readonly ObservableCollection<ISettingElementType> _elements = new();
        public ObservableCollection<ISettingElementType> Elements => _elements;

        public FeatureFlagsView(IReadOnlyDictionary<string, IFeature> features)
        {
            foreach (var (featName, feat) in features)
            {
                ISettingElementType el = new SettingElementType(featName, featName, feat, feat.GetType().GetProperty(nameof(IFeature.IsEnabled)));
                _elements.Add(el);
            }
        }

        public void Add(ISettingElementType element)
        {
        }
    }
}
