using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace BuildIndicatron.Console
{
    public class ProgramParameters
    {
        
        [Option('h',  HelpText = "Host to connect to")]
        public string Host { get; set; }

        [Option('v', HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Option('m', HelpText = "additional message")]
        public string Message { get; set; }

        [Option("setpassive", HelpText = "Set the passive")]
        public bool SetPassive { get; set; }

        [Option('s', HelpText = "set state [Success - Fail - InProgress]")]
        public string State { get; set; }

        [Option("ls", HelpText = "Set state for light saber")]
        public bool LightSaber { get; set; }

        [Option("glow", HelpText = "Set state for lower glow")]
        public bool Glow { get; set; }
        
        
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("BuildIndicatron", Assembly.GetEntryAssembly().GetName().Version.ToString()),
                Copyright = new CopyrightInfo("Rolf Wessels", 2012),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: BuildIndicatron.exe ");
            help.AddOptions(this);
            return help;
        }
    }
}