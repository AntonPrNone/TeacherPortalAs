using System;
using System.Threading.Tasks;
using Supabase;

namespace TeacherPortalAs.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;

        public AuthService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<bool> SignIn(string email, string password)
        {
            try
            {
                Console.WriteLine($"Attempting login for {email}");
                var response = await _supabase.Auth.SignIn(email, password);
                Console.WriteLine($"Login response: {response?.User?.Email}");
                return response?.User != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }
    }
} 