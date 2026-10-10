using BookStore.Shared.Services;

namespace BookStore.Services;

public sealed class MauiTokenStore : ITokenStore
{
    private const string TokenKey = "trang-sach-access-token";
    private const string RoleKey = "trang-sach-role";

    public Task<string?> GetAsync() => SecureStorage.Default.GetAsync(TokenKey);

    public Task<string?> GetRoleAsync() => SecureStorage.Default.GetAsync(RoleKey);

    public async Task SetAsync(string accessToken, string role)
    {
        await SecureStorage.Default.SetAsync(TokenKey, accessToken);
        await SecureStorage.Default.SetAsync(RoleKey, role);
    }

    public Task ClearAsync()
    {
        SecureStorage.Default.Remove(TokenKey);
        SecureStorage.Default.Remove(RoleKey);
        return Task.CompletedTask;
    }
}
