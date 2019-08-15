using System.Collections.Generic;

namespace DeploymentTestBackend.Models
{
    public class DeploymentResult
    {
        public string ElkIp { get; set; }
        public List<string> WindowsIps { get; set; }
    }
}
