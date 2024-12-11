using System;

namespace HunterPie.Features.Account;

internal static class AccountNavigationService
{
    public static EventHandler<EventArgs> OnNavigateToSignIn;
    public static EventHandler<EventArgs> OnNavigateToSignUp;

    public static void NavigateToSignIn() => OnNavigateToSignIn?.Invoke(null, EventArgs.Empty);

    public static void NavigateToSignUp() => OnNavigateToSignUp?.Invoke(null, EventArgs.Empty);
}