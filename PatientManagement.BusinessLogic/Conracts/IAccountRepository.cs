using Microsoft.AspNetCore.Identity;
using PatientManagement.Models;

namespace PatientManagement.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpModelAsync(SignUp signUp);
        Task<string> LoginAsync(SignIn signIn);
    }
}