using HunterPie.Domain.Interfaces;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;

namespace HunterPie.Internal.Initializers
{
    internal class MenuInitializer : IInitializer
    {
        public void Init()
        {
            ISideBar menu = new DefaultSideBar();

            menu.Menu[0].ExecuteOnClick();

            SideBarContainer.SetMenu(menu);
        }
    }
}
