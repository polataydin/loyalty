using Microsoft.Xrm.Sdk;

namespace Loyalty.Plugins._Repository
{
    public class RepositoryHandler
    {

        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }


        public RepositoryHandler()
        {

        }
        public RepositoryHandler(IOrganizationService Service)
        {
            this.Service = Service;
        }

        public RepositoryHandler(IOrganizationService Service, ITracingService TracingService)
        {
            this.Service = Service;
            this.TracingService = TracingService;

        }
        public void trace(string message)
        {
            if (this.TracingService != null)
            {
                this.TracingService.Trace(message);
            }
        }

    }
}
