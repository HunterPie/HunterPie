using App = System.Windows.Application;

namespace HunterPie.UI.Assets.Application
{
    public static class Resources
    {

        public static T Icon<T>(string iconName)
        {
            return (T)App.Current.FindResource(iconName);
        }

        public static T Resource<T>(string resourceName)
        {
            return (T)App.Current.FindResource(resourceName);
        }
    }
}
