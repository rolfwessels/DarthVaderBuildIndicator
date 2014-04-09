using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Shared.Models.Composition;
using log4net;

namespace BuildIndicatron.Console
{
    public class CommandSuccessfulBuild : CommandBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected bool _isSuccess;

        public CommandSuccessfulBuild()
        {
            IsCommand("successful", "Mark a successful build");
            HasAdditionalArguments(1, "<Name of project>");
            _isSuccess = true;
        }

        #region Overrides of CommandBase

        protected override int RunCommand(string[] remainingArguments)
        {
            var allProjects = AllProjects();
            var projectName = remainingArguments[0].ToUpper();
            System.Console.Out.WriteLine("Inspecting project \"{0}\" ", projectName);
            
            var choreography = AddMainColor();
            AddProjectStatusSounds(allProjects, projectName, choreography);
            
            AddCoreProjectStatus(choreography, projectName, _isSuccess);
            AddJenkensStatsToButton();
            BuildIndicationApi.Enqueue(choreography).Wait();
            //add the jenkens stats
            return 0;
        }

        
        protected virtual Choreography AddMainColor()
        {
            var choreography = new Choreography
                {
                    Sequences = SwitchOnPin(0, AppSettings.Default.LsGreenPin).Cast<Sequences>().ToList()
                };
            return choreography;
        }

        protected virtual void AddProjectStatusSounds(Task<JenkensProjectsResult> allProjects, string projectName, Choreography choreography)
        {
            allProjects.Wait();
            var project = allProjects.Result.Jobs.FirstOrDefault(x => x.Name.ToUpper() == projectName);
            if (project != null)
            {
                var successfulBuildInARow = JenkensTextConverter.SuccessfulBuildInARow(project.Builds);
                System.Console.Out.WriteLine(string.Format("The build has {0} successful builds in a row", successfulBuildInARow));

                if (successfulBuildInARow > 20 && successfulBuildInARow%10 == 0)
                {
                    choreography.Sequences.Add(new SequencesText2Speech()
                        {
                            BeginTime = 300,
                            Text =
                                projectName + " completed with " + successfulBuildInARow +
                                " successful builds in a row. The force is strong with this one"
                        });
                }
                if (successfulBuildInARow == 2)
                {
                    choreography.Sequences.Add(new SequencesText2Speech()
                        {
                            BeginTime = 300,
                            Text = " another successful build"
                        });
                }
                else
                {
                    choreography.Sequences.Add(new SequencesPlaySound {BeginTime = 2000, File = "Success"});
                }
            }
            else
            {
                System.Console.Out.WriteLine("Project could not be found on jenkins");
            }
        }

        protected virtual void AddCoreProjectStatus(Choreography choreography, string projectName, bool isSuccess)
        {
            Log.Warn("CommandSuccessfulBuild:AddCoreProjectStatus Sending green pin");
            var coreProjects = GetCoreProjects().ToArray();
            foreach (var coreProject in coreProjects.Where(x => x.Name.ToUpper() == projectName))
            {
                coreProject.Color = isSuccess ? JenkensTextConverter.SuccessColor : JenkensTextConverter.FailColor;
            }
            Log.Info("Core projects stats for " + string.Join(",",coreProjects.Select(x => x.Name).ToArray()));
            Log.Info("Core projects stats for " + string.Join(",", coreProjects.Select(x => x.Color).ToArray()));
            const int beginTime = 10000;
            if (coreProjects.Any(x => x.Color == JenkensTextConverter.FailColor))
            {
                Log.Info("Found atleast one core project that failed");
                choreography.Sequences.AddRange(SwitchOnPin(beginTime, AppSettings.Default.LsRedPin));
            }
            else
            {
                Log.Warn("CommandSuccessfulBuild:AddCoreProjectStatus Sending green pin");
                choreography.Sequences.AddRange(SwitchOnPin(beginTime, AppSettings.Default.LsGreenPin));
            }

        }

        private IEnumerable<Job> GetCoreProjects()
        {
            var allProjects = AllProjects();
            allProjects.Wait();
            var strings = AppSettings.Default.CoreProjects.Split('|');
            return allProjects.Result.Jobs.Where(x => IsIn(strings, x.Name));
        }

        private bool IsIn(IEnumerable<string> strings, string job)
        {
            return strings.Any(job.Contains);
        }

        #endregion
    }
}