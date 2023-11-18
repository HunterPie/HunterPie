using HunterPie.Core.Domain.Features.Domain;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Controls.Settings.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Flags;

public class FeatureFlagsView : ISettingElement
{
    public string Title => "Feature Flags";

    public string Description => "Feature flags configuration";

    public ImageSource Icon => Resources.Icon("ICON_FLAG");

    public ObservableCollection<ISettingElementType> Elements { get; } = new();

    public FeatureFlagsView(IReadOnlyDictionary<string, IFeature> features)
    {
        foreach ((string featName, IFeature feat) in features)
        {
            System.Reflection.PropertyInfo info = feat.GetType().GetProperty(nameof(IFeature.IsEnabled));
            ISettingElementType el = new SettingElementType(null, featName, featName, feat, info, true);
            Elements.Add(el);
        }
    }

    public void Add(ISettingElementType element)
    {
    }
}
