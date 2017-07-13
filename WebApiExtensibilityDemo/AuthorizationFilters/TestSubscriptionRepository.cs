using System.Threading;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.AuthorizationFilters
{
	public class TestSubscriptionRepository : ISubscriptionRepository
	{
		public async Task<bool> CheckForActiveSubscription(string userName, CancellationToken cancellationToken)
		{
			return userName == "user1";
		}
	}
}
