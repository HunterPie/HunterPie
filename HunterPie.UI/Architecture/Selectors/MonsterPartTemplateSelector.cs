using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Architecture.Selectors
{
    public class MonsterPartTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate SeverableTemplate { get; set; }
        public DataTemplate BreakableTemplate { get; set; }
        public DataTemplate Empty = new DataTemplate();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MonsterPartViewModel model)
            {
                return model.Type switch
                {
                    PartType.Flinch => DefaultTemplate,
                    PartType.Breakable => BreakableTemplate,
                    PartType.Severable => SeverableTemplate,
                    PartType.Invalid => Empty,
                    _ => throw new NotImplementedException(),
                };
            }

            throw new ArgumentException("item must be a MonsterPartViewModel");
        }
    }
}
