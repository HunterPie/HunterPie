using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using System.Threading.Tasks;
using LoginRequest = HunterPie.Integrations.Poogie.Account.Models.LoginRequest;
using LoginResponse = HunterPie.Integrations.Poogie.Account.Models.LoginResponse;
using LogoutResponse = HunterPie.Integrations.Poogie.Account.Models.LogoutResponse;
using MyUserAccountResponse = HunterPie.Integrations.Poogie.Account.Models.MyUserAccountResponse;

namespace HunterPie.Integrations.Poogie.Account;

internal class PoogieAccountConnector
{
    private readonly PoogieConnector _connector = new PoogieConnector();

    private const string ACCOUNT_ENDPOINT = "/v1/account";
    private const string USER_ENDPOINT = "/v1/user";
    private const string LOGIN_ENDPOINT = "/v1/login";
    private const string LOGOUT_ENDPOINT = "/v1/logout";

    public async Task<PoogieResult<LoginResponse>> Login(LoginRequest request) =>
        await _connector.Post<LoginRequest, LoginResponse>(LOGIN_ENDPOINT, request);

    public async Task<PoogieResult<LogoutResponse>> Logout() =>
        await _connector.Post<Nothing, LogoutResponse>(LOGOUT_ENDPOINT, new Nothing());

    public async Task<PoogieResult<MyUserAccountResponse>> MyUserAccount() =>
        await _connector.Get<MyUserAccountResponse>(USER_ENDPOINT + "/me");

    public async Task<PoogieResult<RegisterResponse>> Register(RegisterRequest request) =>
        await _connector.Post<RegisterRequest, RegisterResponse>(ACCOUNT_ENDPOINT, request);

    public async Task<PoogieResult<MyUserAccountResponse>> UploadAvatar(string filename) =>
        await _connector.SendFile<MyUserAccountResponse>(USER_ENDPOINT + "/avatar/upload", filename);

    public async Task<PoogieResult<PasswordChangeResponse>> ForgotPassword(PasswordResetRequest request) =>
        await _connector.Post<PasswordResetRequest, PasswordChangeResponse>($"{ACCOUNT_ENDPOINT}/password/reset", request);

    public async Task<PoogieResult<PasswordChangeResponse>> ChangePassword(ChangePasswordRequest request) =>
        await _connector.Post<ChangePasswordRequest, PasswordChangeResponse>($"{ACCOUNT_ENDPOINT}/password", request);

    public async Task<PoogieResult<RequestAccountVerificationResponse>> RequestAccountVerification(RequestAccountVerifyRequest request) =>
        await _connector.Post<RequestAccountVerifyRequest, RequestAccountVerificationResponse>($"{ACCOUNT_ENDPOINT}/verify", request);

}
