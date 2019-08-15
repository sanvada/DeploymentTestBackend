namespace DeploymentTestBackend.Models
{
    public class WindowsTemplate
    {
        public bool EnableIis { get; set; }
        public bool EnableIisFileBeat { get; set; }
        public int MegabytesOfRam { get; set; }
        public int NumberOfCpus { get; set; }
    }
}
