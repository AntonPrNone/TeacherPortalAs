using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TeacherPortalAs.Services;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Microsoft.JSInterop;

namespace TeacherPortalAs;

public static class AuthConstants
{
    public const string ACCESS_TOKEN_KEY = "sb-access-token";
    public const string REFRESH_TOKEN_KEY = "sb-refresh-token";
}

public class CustomSessionHandler : IGotrueSessionPersistence<Session>
{
    private readonly IJSRuntime _jsRuntime;
    private const string ACCESS_TOKEN_KEY = AuthConstants.ACCESS_TOKEN_KEY;
    private const string REFRESH_TOKEN_KEY = AuthConstants.REFRESH_TOKEN_KEY;
    private Session? _cachedSession;

    public CustomSessionHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        Console.WriteLine("CustomSessionHandler created");
    }

    public void SaveSession(Session session)
    {
        if (session != null)
        {
            _cachedSession = session;
            Console.WriteLine($"SaveSession: Saving tokens - Access: {session.AccessToken?.Substring(0,10)}...");
            _jsRuntime.InvokeVoidAsync("localStorage.setItem", ACCESS_TOKEN_KEY, session.AccessToken);
            _jsRuntime.InvokeVoidAsync("localStorage.setItem", REFRESH_TOKEN_KEY, session.RefreshToken);
        }
    }

    public Session? LoadSession()
    {
        try 
        {
            if (_cachedSession != null) return _cachedSession;

            // Возвращаем пустую сессию при старте
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LoadSession Error: {ex.Message}");
            return null;
        }
    }

    public async Task<Session?> LoadSessionAsync()
    {
        try
        {
            var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ACCESS_TOKEN_KEY);
            var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", REFRESH_TOKEN_KEY);
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) return null;
            return new Session { AccessToken = accessToken, RefreshToken = refreshToken };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LoadSessionAsync Error: {ex.Message}");
            return null;
        }
    }

    public void DestroySession()
    {
        _cachedSession = null;
    }

    public async Task DestroySessionAsync()
    {
        Console.WriteLine("DestroySession: Called");
        _cachedSession = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", ACCESS_TOKEN_KEY);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", REFRESH_TOKEN_KEY);
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Application starting...");
        
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var url = "https://bijtljzesbucfxdwsmeb.supabase.co";
        var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJpanRsanplc2J1Y2Z4ZHdzbWViIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzkzODU3NjIsImV4cCI6MjA1NDk2MTc2Mn0.Q0ZOiHmZCTnCGzMr5SAy0tnxX5uuxDgUk0qlAa1pgUw";

        Console.WriteLine("Creating Supabase client...");

        builder.Services.AddSingleton<IGotrueSessionPersistence<Session>, CustomSessionHandler>();
        var provider = builder.Services.BuildServiceProvider();
        var sessionHandler = provider.GetRequiredService<IGotrueSessionPersistence<Session>>();

        // Пробуем загрузить существующую сессию
        Console.WriteLine("Attempting to load existing session...");
        var existingSession = sessionHandler.LoadSession();
        Console.WriteLine($"Existing session found: {existingSession != null}");

        var supabase = new Supabase.Client(url, key, new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            SessionHandler = sessionHandler
        });
        await supabase.InitializeAsync();
        
        // Асинхронно загружаем сессию
        var session = await ((CustomSessionHandler)sessionHandler).LoadSessionAsync();
        if (session != null)
        {
            await supabase.Auth.SetSession(session.AccessToken, session.RefreshToken);
        }

        builder.Services.AddSingleton(supabase);

        builder.Services.AddScoped<ISubjectService, SubjectService>();
        builder.Services.AddScoped<IMaterialService, MaterialService>();
        builder.Services.AddScoped<IBlogService, BlogService>();

        var app = builder.Build();

        Console.WriteLine("Application built, starting...");
        await app.RunAsync();
    }
}
