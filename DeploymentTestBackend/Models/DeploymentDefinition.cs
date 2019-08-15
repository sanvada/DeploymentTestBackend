using System.Collections.Generic;

namespace DeploymentTestBackend.Models
{
    public class DeploymentDefinition
    {
        public ElkTemplate ElkTemplate { get; set; }

        public List<WindowsTemplate> WindowsTemplates { get; set; }
    }
}
