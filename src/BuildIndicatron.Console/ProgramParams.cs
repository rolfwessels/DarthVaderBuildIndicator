using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace BuildIndicatron.Console
{
    public class ProgramParameters
    {
        [Option("file", HelpText = "upload a file to server")]
        public string InputFile { get; set; }

        [Option("h", DefaultValue = Shared.ApiPaths.LocalHost, HelpText = "Host to connect to")]
        public string UrlToConnectTo { get; set; }

        [Option("length", DefaultValue = -1, HelpText = "The maximum number of bytes to process.")]
        public int MaximumLength { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Option('p', null, HelpText = "Play test file")]
        public bool Test { get; set; }

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