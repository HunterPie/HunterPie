using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Activities.Presentation
{
    public class ActivityTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SubmarineTemplate { get; set; }
        public DataTemplate TrainingDojoTemplate { get; set; }
        public DataTemplate MeowcenariesTemplate { get; set; }
        public DataTemplate CohootTemplae { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IActivity activity)
            {
                return activity.Type switch
                {
                    ActivityType.HarvestBox => throw new NotImplementedException(),
                    ActivityType.Argosy => throw new NotImplementedException(),
                    ActivityType.Submarine => SubmarineTemplate,
                    ActivityType.Meowcenaries => MeowcenariesTemplate,
                    ActivityType.TrainingDojo => TrainingDojoTemplate,
                    ActivityType.Cohoot => CohootTemplae,
                    _ => throw new NotImplementedException(string.Format("Missing implementation for {0}", nameof(activity.Type)))
                };
            }

            throw new ArgumentException($"item must be an {nameof(IActivity)}");
        }
    }
}
