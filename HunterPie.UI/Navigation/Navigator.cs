namespace HunterPie.UI.Navigation;

public static class Navigator
{
#pragma warning disable CS8618
    public static INavigator Body;
    public static INavigator App;
#pragma warning restore CS8618

    internal static void SetNavigators(INavigator body, INavigator app)
    {
        Body = body;
        App = app;
    }
}