using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Cron
{
    public class JobScheduler
    {

        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobClass>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggerNotification","GroupNotification")
                .StartNow()
                .WithSimpleSchedule(
                    s => s.WithIntervalInHours(6)
                          .RepeatForever()
                    )
                
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}