using System.Threading.Tasks;
using System.Web.Http;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{

    /// <summary>
	///     Api controller for managing all the project
	/// </summary>
    [RoutePrefix("api/ping")]
    public class ProjectController : ApiController
    {

        /// <summary>
        ///     Returns list of all the projects as references
        /// </summary>
        /// <returns>
        /// </returns>
        [Route,HttpGet]
        public Task<string[]> Get()
        {
            return Task.FromResult(new[] {"asdf", "fasdasdf"});
        }
        
	}
}

