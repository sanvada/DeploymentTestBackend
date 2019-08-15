using Newtonsoft.Json;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace DeploymentTestBackend.Utilities
{
    public static class PowerShellProxy
    {
        public static async Task<string> ExecuteCommandAsync(string command, string host)
        {
            var initial = InitialSessionState.CreateDefault();
            using var runSpace = RunspaceFactory.CreateRunspace(initial);
            runSpace.Open();

            var scriptBlock = ScriptBlock.Create(command);

            using var results = await PowerShell.Create(runSpace)
                .AddCommand("Invoke-Command")
                .AddParameter("ComputerName", host)
                .AddParameter("ScriptBlock", scriptBlock)
                .InvokeAsync();

            runSpace.Close();

            return JsonConvert.SerializeObject(results);

        }

    }
}
