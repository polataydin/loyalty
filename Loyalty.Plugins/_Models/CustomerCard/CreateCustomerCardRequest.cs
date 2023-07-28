using System;

namespace Loyalty.Plugins._Models.CustomerCard
{
    public class CreateCustomerCardRequest
    {
        public Guid contactId { get; set; }
        public Guid cardTypeId { get; set; }
        public string cardNumber { get; set; }
    }
}
