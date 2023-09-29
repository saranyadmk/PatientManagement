using Microsoft.AspNetCore.Identity;
using PatientManagement.Models;

namespace PatientManagement.Repository
{
    public interface ISecurityRepository
    {
        Task<IdentityResult> SignUpModelAsync(SignUp signUp);
        Task<string> LoginAsync(SignIn signIn);
    }
}