using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TeacherPortalAs;
using TeacherPortalAs.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

try
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    Console.WriteLine("Initializing application...");

    // Конфигурация Supabase
    var url = "https://bijtljzesbucfxdwsmeb.supabase.co";
    var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJpanRsanplc2J1Y2Z4ZHdzbWViIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzkzODU3NjIsImV4cCI6MjA1NDk2MTc2Mn0.Q0ZOiHmZCTnCGzMr5SAy0tnxX5uuxDgUk0qlAa1pgUw";

    Console.WriteLine("Creating Supabase client...");
    var supabase = new Supabase.Client(url, key, new Supabase.SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    });

    try
    {
        Console.WriteLine("Initializing Supabase connection...");
        await supabase.InitializeAsync();
        Console.WriteLine("Supabase connection established successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to initialize Supabase: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw new Exception("Не удалось подключиться к базе данных. Пожалуйста, попробуйте позже.", ex);
    }

    // Регистрация сервисов
    Console.WriteLine("Registering services...");
    builder.Services.AddSingleton<Supabase.Client>(_ => supabase);
    builder.Services.AddSingleton<ISubjectService, SubjectService>();
    builder.Services.AddSingleton<IMaterialService, MaterialService>();
    builder.Services.AddSingleton<IBlogService, BlogService>();

    var app = builder.Build();
    Console.WriteLine("Application built successfully");

    Console.WriteLine("Starting application...");
    await app.RunAsync();
    Console.WriteLine("Application started");
}
catch (Exception ex)
{
    Console.WriteLine($"Critical application error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
    }
    
    throw new Exception("Произошла критическая ошибка при запуске приложения. Пожалуйста, обновите страницу.", ex);
}
