using Microsoft.AspNetCore.Identity;

namespace Mango.Service.AuthAPI.Model
{
	public class ApplicationUsers : IdentityUser
	{
		public string Name { get; set; }	
	}
}
