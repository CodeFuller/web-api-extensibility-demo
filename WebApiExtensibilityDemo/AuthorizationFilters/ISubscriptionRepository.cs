using System.Threading;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.AuthorizationFilters
{
	public interface ISubscriptionRepository
	{
		Task<bool> CheckForActiveSubscription(string userName, CancellationToken cancellationToken);
	}
}
