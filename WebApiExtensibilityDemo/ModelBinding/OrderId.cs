using System.ComponentModel;

namespace WebApiExtensibilityDemo.ModelBinding
{
	[TypeConverter(typeof(OrderIdTypeConverter))]
	public class OrderId
	{
		public string DepartmentId { get; }

		public int OrderNumber { get; }

		public OrderId(string departmentId, int orderNumber)
		{
			DepartmentId = departmentId;
			OrderNumber = orderNumber;
		}
	}
}
