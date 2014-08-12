using System;
using System.Configuration;
using NDesk.Options;

namespace BuildIndicatron.Console
{
  public class HelperCommand : CommandBase
  {
    public HelperCommand()
    {
      IsCommand("helper", "Various helper commands");
      
      var optionSet = new OptionSet
                {
                    {"password=", "Set jenkins password", s => SetPassword = s},
                    {"username=", "Set jenkins username", s => Username = s},
                    {"host=", "Set jenkins host", s => SetHost = s},
                    
                  
                };

      

      foreach (var option in optionSet)
      {
        Options.Add(option);
      }

    }

    public string SetHost { get; set; }

    public string Username { get; set; }

    public string SetPassword { get; set; }

    #region Overrides of CommandBase

    protected override int RunCommand(string[] remainingArguments)
    {
      if (!string.IsNullOrEmpty(SetPassword))
      {
        var simpleCrypt = new SimpleCrypt();
        string jenkenPassword = simpleCrypt.Encrypt(SetPassword);
        AppSettings.Default.JenkenPassword = jenkenPassword;
        AppSettings.Default.Save();
        System.Console.Out.WriteLine("Password has been set to {0}", jenkenPassword);
      }
      if (!string.IsNullOrEmpty(Username))
      {
        AppSettings.Default.JenkenUsername = Username;
        AppSettings.Default.Save();
        System.Console.Out.WriteLine("Username has been set to {0}", Username);
      }
      if (!string.IsNullOrEmpty(SetHost))
      {
        AppSettings.Default.Host = SetHost;
        AppSettings.Default.Save();
        System.Console.Out.WriteLine("Password has been set to {0}", SetHost);
      }

      System.Console.Out.WriteLine("Connecting to {0} with username [{1}] and [{2}]", AppSettings.Default.Host, AppSettings.Default.JenkenUsername, AppSettings.Default.JenkenPassword);
      return 0;
    }

    #endregion
  }
}