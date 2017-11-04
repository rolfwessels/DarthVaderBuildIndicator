using System;

namespace BuildIndicatron.Core.Helpers
{
    public class EnvSettings
    {

        private static readonly Lazy<EnvSettings> _instance = new Lazy<EnvSettings>(() => new EnvSettings());

        private EnvSettings()
        {
//            Need admin for this       
//            Environment.SetEnvironmentVariable("buildindicator_host", "", EnvironmentVariableTarget.Machine);
//            Environment.SetEnvironmentVariable("buildindicator_jenkinsuser", "", EnvironmentVariableTarget.Machine);
//            Environment.SetEnvironmentVariable("buildindicator_jenkinspassword", "", EnvironmentVariableTarget.Machine);

            JenkinsHost = Environment.GetEnvironmentVariable("buildindicator_host") ?? "http://jenkins:8080/";
            JenkinsUser = Environment.GetEnvironmentVariable("buildindicator_jenkinsuser") ?? "jenkins";
            JenkinsPassword = Environment.GetEnvironmentVariable("buildindicator_jenkinspassword") ?? "password";
            JenkinsPassword = Environment.GetEnvironmentVariable("buildindicator_jenkinspassword") ?? "password";

            SshHost = Environment.GetEnvironmentVariable("buildindicator_sshhost") ?? "SshHost";
            SshUser = Environment.GetEnvironmentVariable("buildindicator_sshuser") ?? "SshUser";
            SshPassword = Environment.GetEnvironmentVariable("buildindicator_sshpassword") ?? "SshPassword";
        }

        public string JenkinsUser { get;  }

        public string JenkinsPassword { get; }

        public string SshHost { get;  }
        public string SshUser { get;  }
        public string SshPassword { get;  }

        public string JenkinsHost { get;  }

        #region singleton

        public static EnvSettings Instance => _instance.Value;
        

        #endregion



    }
}