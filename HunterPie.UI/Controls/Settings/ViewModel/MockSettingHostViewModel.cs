using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    internal class MockSettingHostViewModel
    {
        public class MockSettingElement : ISettingElement
        {
            public string Title => "Mock";
            public string Description => "Mock";
            public ImageSource Icon => Resources.Icon("ICON_BUG");
            public string Mock = "Mock";

            public ObservableCollection<ISettingElementType> Elements { get; } = new();
            public ObservableCollection<GameProcess> Games { get; } = new();

            public Observable<GameProcess> SelectedGame => GameProcess.MonsterHunterWorld;

            public void Add(ISettingElementType element) { }

            public MockSettingElement()
            {
                for (int i = 0; i < 15; i++)
                    Elements.Add(new SettingElementType("Mock", "Mock", this, GetType().GetProperty(nameof(Mock)), false));

                foreach (GameProcess gameProcess in Enum.GetValues<GameProcess>())
                    Games.Add(gameProcess);
            }
        }

        public ObservableCollection<ISettingElement> Elements { get; } = new()
        {
            new MockSettingElement(),
            new MockSettingElement(),
        };
    }
}
