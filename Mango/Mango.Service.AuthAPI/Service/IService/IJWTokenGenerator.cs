using Mango.Service.AuthAPI.Model;

namespace Mango.Service.AuthAPI.Service.IService
{
	public interface IJWTokenGenerator
	{

		string GenerateToken(ApplicationUsers applicationUsers, IEnumerable<string>roles);

	}
}
