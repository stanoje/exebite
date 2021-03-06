﻿using System;
using Topshelf;
using Unity;

namespace Exebite.JobScheduler
{
    public static class Program
    {
        public static void Main()
        {
            using (var container = new UnityContainer())
            {
                Unity.UnityConfig.RegisterTypes(container);
                var rc = HostFactory.Run(x =>
                    {
                        x.Service<JobSchedulerService>(s =>
                        {
                            s.ConstructUsing(() => new JobSchedulerService());
                            s.WhenStarted(tc => tc.Start());
                            s.WhenStopped(tc => tc.Stop());
                        });

                        x.SetDescription("Job Scheduler Service");
                        x.SetDisplayName("Job Scheduler Service");
                        x.SetServiceName("JobSchedulerService");
                    });

                var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
                Environment.ExitCode = exitCode;
            }
        }
    }
}
