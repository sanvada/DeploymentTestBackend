using DeploymentTestBackend.Models;
using DeploymentTestBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentTestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeployController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await DeploymentHelper.DeployAndConfigureAsync(new DeploymentDefinition
            {
                ElkTemplate = new ElkTemplate(),
                WindowsTemplates = new List<WindowsTemplate>
                {
                    new WindowsTemplate()
                }
            });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Deploy([FromBody] DeploymentDefinition deploymentDefinition)
        {
            var deploymentResult = await DeploymentHelper.DeployAndConfigureAsync(deploymentDefinition);

            return Ok(deploymentResult);
        }
    }
}