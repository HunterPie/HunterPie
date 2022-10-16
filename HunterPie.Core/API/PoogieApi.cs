using HunterPie.Core.API.Entities;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.API;

#nullable enable
public static class PoogieApi
{
    public static event EventHandler<BackupDeleteResponse>? OnBackupDeleted;

    public static async Task<PoogieApiResult<LoginResponse>?> Login(LoginRequest request) => await Post<LoginRequest, LoginResponse>(LOGIN, request);

    public static async Task<PoogieApiResult<LogoutResponse>?> Logout() => await Post<object, LogoutResponse>(LOGOUT, new());

    public static async Task<PoogieApiResult<VersionResponse>?> GetLatestVersion() => await Get<VersionResponse>(VERSION_PATH);

    public static async Task<PoogieApiResult<SupporterValidationResponse>?> ValidateSupporterToken() => await Get<SupporterValidationResponse>(SUPPORTER_PATH + "/verify");

    public static async Task<PoogieApiResult<Notification[]>?> GetNotifications() => await Get<Notification[]>(NOTIFICATIONS);

    public static async Task<PoogieApiResult<MyUserAccountResponse>?> GetMyUserAccount() => await Get<MyUserAccountResponse>(MY_ACCOUNT);

    public static async Task<PoogieApiResult<RegisterResponse>?> Register(RegisterRequest request) => await Post<RegisterRequest, RegisterResponse>(ACCOUNT, request);

    public static async Task<PoogieApiResult<MyUserAccountResponse>?> UploadAvatar(string fileName) => await PostFile<MyUserAccountResponse>(AVATAR_UPLOAD, fileName);

    public static async Task<PoogieApiResult<UserBackupDetailsResponse>?> GetUserBackups() => await Get<UserBackupDetailsResponse>(BACKUPS);

    public static async Task<PoogieApiResult<BackupResponse>?> UploadBackup(GameType gameType, string fileName) => await PostFile<BackupResponse>($"{BACKUPS}/upload/{gameType}", fileName);

    public static async Task<PoogieResponse> DownloadBackup(string backupId) => await Download($"{BACKUPS}/{backupId}");

    public static async Task<PoogieApiResult<BackupDeleteResponse>?> DeleteBackup(string backupId)
    {
        PoogieApiResult<BackupDeleteResponse>? result = await Delete<BackupDeleteResponse, object>($"{BACKUPS}/{backupId}", new());

        if (result?.Response is not null)
            OnBackupDeleted?.Invoke(null, result.Response);

        return result;
    }

    public static async Task<PoogieApiResult<PasswordChangeResponse>?> RequestPasswordResetCode(PasswordResetRequest request) => await Post<PasswordResetRequest, PasswordChangeResponse>($"{ACCOUNT}/password/reset", request);
    public static async Task<PoogieApiResult<PasswordChangeResponse>?> ChangePassword(ChangePasswordRequest request) => await Post<ChangePasswordRequest, PasswordChangeResponse>($"{ACCOUNT}/password", request);

    private const string VERSION_PATH = "/v1/version";
    private const string CRASH_PATH = "/v1/report/crash";
    private const string SUPPORTER_PATH = "/v1/supporter";
    private const string NOTIFICATIONS = "/v1/notifications";
    private const string LOGIN = "/v1/login";
    private const string LOGOUT = "/v1/logout";
    private const string ACCOUNT = "/v1/account";
    private const string MY_ACCOUNT = "/v1/user/me";
    private const string BACKUPS = "/v1/backup";
    private const string AVATAR_UPLOAD = "/v1/user/avatar/upload";
    private const string SUPPORTER_HEADER = "X-Supporter-Token";
    private const double DEFAULT_TIMEOUT = 20;
    private const int DEFAULT_RETRIES = 3;
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);

    private static async Task<PoogieApiResult<T>?> Get<T>(string path) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Get(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(DefaultTimeout)
            .WithRetry(DEFAULT_RETRIES)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : await resp.AsJson<T>();
    }

    private static async Task<PoogieResponse> Download(string path)
    {
        using Poogie request = PoogieFactory.Default()
            .Get(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithRetry(DEFAULT_RETRIES)
            .Build();

        return await request.RequestAsync();
    }

    private static async Task<PoogieApiResult<T>?> Post<P, T>(string path, P payload) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Post(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithJson(payload)
            .WithTimeout(DefaultTimeout)
            .WithRetry(DEFAULT_RETRIES)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : await resp.AsJson<T>();
    }

    private static async Task<PoogieApiResult<T>?> PostFile<T>(string path, string fileName) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Post(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(DefaultTimeout)
            .WithRetry(DEFAULT_RETRIES)
            .WithFile("file", fileName)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : await resp.AsJson<T>();
    }

    private static async Task<PoogieApiResult<T>?> Delete<T, P>(string path, P payload) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Delete(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(DefaultTimeout)
            .WithRetry(DEFAULT_RETRIES)
            .WithJson(payload)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : await resp.AsJson<T>();
    }
}
