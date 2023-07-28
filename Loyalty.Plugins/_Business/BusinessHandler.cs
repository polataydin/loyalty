using Microsoft.Xrm.Sdk;

namespace Loyalty.Plugins._Business
{
    public class BusinessHandler
    {

        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }
 

        public BusinessHandler()
        {

        }
        public BusinessHandler(IOrganizationService Service)
        {
            this.Service = Service;
        }

        public BusinessHandler(IOrganizationService Service, ITracingService TracingService)
        {
            this.Service = Service;
            this.TracingService = TracingService;
    
        }

        public void trace(string message)
        {
            if(this.TracingService != null)
            {
                this.TracingService.Trace(message);
            }
        }       
    }
}
