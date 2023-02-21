using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;

namespace HunterPie.Features.Account.Config;

[SettingsGroup("ACCOUNT_STRING", "ICON_ACCOUNT")]
internal class AccountConfig : ISettings
{

    [SettingField("ACCOUNT_ENABLE_BACKUP_STRING")]
    public Observable<bool> IsBackupEnabled { get; set; } = true;

    [SettingField("ACCOUNT_ENABLE_HUNT_UPLOAD_STRING")]
    public Observable<bool> IsHuntUploadEnabled { get; set; } = true;
}
