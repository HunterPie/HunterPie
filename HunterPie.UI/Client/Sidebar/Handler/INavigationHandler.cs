using HunterPie.UI.Client.Sidebar.Entity;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.UI.Client.Sidebar.Handler;

public interface INavigationHandler
{
    public Type ViewType { get; }

    public string Label { get; set; }

    public ImageSource Icon { get; set; }

    public SideBarButtonState State { get; set; }

    public bool IsActive { get; set; }

    public Task InitializeAsync();

    public Task ExecuteAsync();
}