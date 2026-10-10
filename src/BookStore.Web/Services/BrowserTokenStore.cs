using BookStore.Shared.Services;
using Microsoft.JSInterop;

namespace BookStore.Web.Services;

public sealed class BrowserTokenStore(IJSRuntime jsRuntime) : ITokenStore
{
    private const string TokenKey = "trang-sach-access-token";
    private const string RoleKey = "trang-sach-role";

    public async Task<string?> GetAsync() => await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", TokenKey);

    public async Task<string?> GetRoleAsync() => await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", RoleKey);

    public async Task SetAsync(string accessToken, string role)
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TokenKey, accessToken);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", RoleKey, role);
    }

    public async Task ClearAsync()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", RoleKey);
    }
}
