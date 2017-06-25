using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.AuthenticationFilters
{
	public class AuthenticationAgent : IAuthenticationAgent
	{
		private readonly Dictionary<string, string> userPasswords = new Dictionary<string, string>();
		private readonly Dictionary<string, string[]> userRoles = new Dictionary<string, string[]>();

		public AuthenticationAgent()
		{
			userPasswords.Add("user1", "password1");
			userPasswords.Add("user2", "password2");

			userRoles.Add("user1", new[] { "Administrator" });
			userRoles.Add("user2", new[] { "Administrator", "ContentUser" });
		}

		public Task<IPrincipal> AuthenticateUser(string userName, string userPassword)
		{
			string currUserPassword;
			if (!userPasswords.TryGetValue(userName, out currUserPassword))
			{
				//	Invalid username
				return null;
			}

			if (currUserPassword != userPassword)
			{
				//	Invalid password
				return null;
			}

			string[] currUserRoles;
			if (!userRoles.TryGetValue(userName, out currUserRoles))
			{
				currUserRoles = new string[] { };
			}

			IIdentity identity = new GenericIdentity(userName);
			IPrincipal principal = new GenericPrincipal(identity, currUserRoles);
			return Task.FromResult(principal);
		}
	}
}
