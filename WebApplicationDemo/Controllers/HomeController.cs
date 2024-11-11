using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Controllers;

using DemoCode;
using Fanex.Data.Repository;

[ApiController]
[Route("[controller]")]
public class HomeController(IDynamicRepository repository) : ControllerBase
{
	[HttpGet]
	public string Get()
	{
		var site = repository.Fetch<Site>(new SiteGetAllCriteria()).First();
		
		return site?.SiteName;
	}
}