using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Architecture;
using Microsoft.Win32;
using System;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountPreferencesViewModel(IAccountUseCase accountUseCase) : ViewModel
{
    private readonly IAccountUseCase _accountUseCase = accountUseCase;

    public string Username { get; set => SetValue(ref field, value); } = string.Empty;
    public string Email { get; set => SetValue(ref field, value); } = string.Empty;
    public string AvatarUrl { get; set => SetValue(ref field, value); } = "https://cdn.hunterpie.com/avatars/default.png";
    public bool IsSupporter { get; set => SetValue(ref field, value); }
    public bool IsFetchingAccount { get; set => SetValue(ref field, value); }
    public bool IsUploadingAvatar { get; set => SetValue(ref field, value); }

    public async void UploadAvatar()
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = Environment.CurrentDirectory,
            Filter = "Image Files|*.png;",
            RestoreDirectory = true
        };

        if (dialog.ShowDialog() != true)
            return;

        IsUploadingAvatar = true;
        await _accountUseCase.UploadAvatarAsync(dialog.FileName);
        IsUploadingAvatar = false;
    }
}