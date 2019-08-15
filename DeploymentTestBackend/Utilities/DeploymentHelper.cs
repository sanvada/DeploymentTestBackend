using DeploymentTestBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeploymentTestBackend.Utilities
{
    public static class DeploymentHelper
    {
        public static async Task<DeploymentResult> DeployAndConfigureAsync(DeploymentDefinition deploymentDefinition)
        {
            var windowsHostNames = new List<string>();

            var deployTasks = deploymentDefinition.WindowsTemplates
                .Select(s =>
                {
                    var hostName = "win_" + Guid.NewGuid();
                    windowsHostNames.Add(hostName);
                    return PowerShellProxy.ExecuteCommandAsync(CreateWindowsDeploymentScriptBlock(s, hostName), "bajor");
                }).ToList();


            var elkHostname = "elk_" + Guid.NewGuid();
            deployTasks.Add(
                PowerShellProxy.ExecuteCommandAsync(CreateElkDeploymentScriptBlock(deploymentDefinition.ElkTemplate, elkHostname),
                    "bajor"));


            await Task.WhenAll(deployTasks);

            var elkIp = await GetIpAddressFromVmName(elkHostname);

            var windowsIps = new List<string>();

            foreach (var hostname in windowsHostNames)
            {
                var winIp = await GetIpAddressFromVmName(hostname);
                windowsIps.Add(winIp);

                // Configure Windows VM
            }

            return new DeploymentResult
            {
                ElkIp = elkIp,
                WindowsIps = windowsIps
            };
        }

        /// <summary>
        /// Configure Windows based on template
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="windowsTemplate"></param>
        /// <returns></returns>
        private static async Task ConfigureWindows(string ipAddress, WindowsTemplate windowsTemplate)
        {
            if (windowsTemplate.EnableIis)
            {
                // configure IIS
            }
        }

        private static string CreateElkDeploymentScriptBlock(ElkTemplate elkTemplate, string hostname)
        {
            return File.ReadAllText(@"DeploymentScripts\deploy_elk.ps1").Replace("<name>", hostname);
        }

        private static string CreateWindowsDeploymentScriptBlock(WindowsTemplate windowsTemplate, string hostname)
        {
            return File.ReadAllText(@"DeploymentScripts\deploy_win.ps1").Replace("<name>", hostname);
        }

        private static async Task<string> GetIpAddressFromVmName(string vmName)
        {
            var command = File.ReadAllText(@"DeploymentScripts\get_ip_by_name.ps1").Replace("<name>", vmName);

            var remoteResult = await PowerShellProxy.ExecuteCommandAsync(command, "galaxy");

            return IpAddressRegex.Match(remoteResult).Value;
        }

        private static readonly Regex IpAddressRegex = new Regex(@"10.0.0.1\d\d", RegexOptions.Compiled);
    }
}
