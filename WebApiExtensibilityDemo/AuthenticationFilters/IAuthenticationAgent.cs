using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.AuthenticationFilters
{
	public interface IAuthenticationAgent
	{
		Task<IPrincipal> AuthenticateUser(string userName, string userPassword);
	}
}
