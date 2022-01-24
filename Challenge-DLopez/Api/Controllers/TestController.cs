using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpGet(Name = "health")]
		public string Health()
		{
			string result = "TestController funcionando";
			return result;	
		}
	}
}
