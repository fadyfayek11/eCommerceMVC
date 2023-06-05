using eCommerce.Integrations.Service;
using eCommerce.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eCommerce.Web.Cron
{
    public class JobClass : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run( () => ProductsService.Instance.SyncProducts());

        }
    }
}