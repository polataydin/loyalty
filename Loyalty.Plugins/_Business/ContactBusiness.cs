using Loyalty.Plugins._Models.Contact;
using Loyalty.Plugins._Models.CustomerCard;
using Loyalty.Plugins._Repository;
using Loyalty.Plugins.Common;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins._Business
{
    public class ContactBusiness : BusinessHandler
    {
        public ContactBusiness(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }
        public void updateContactDefaultCardFields(Guid contactId, Guid customerCardId)
        {
            CustomerCardRepository customerCardRepository = new CustomerCardRepository(this.Service, this.TracingService);
            EnvironmentVariableRepository environmentVariableRepository = new EnvironmentVariableRepository(this.Service, this.TracingService);
            var variable = environmentVariableRepository.getEnvironmentVariable("plt_defaultcardtype");

            this.trace(string.Format("variable {0}", variable));
            Entity e = new Entity("contact");
            e["plt_cardtypeid"] = new EntityReference("plt_cardtypes", new Guid(variable));
            e["plt_currentcardid"] = new EntityReference("plt_customercard", customerCardId);

            e["plt_cardnumber"] = customerCardRepository.getActiveCustomerCard(contactId).GetAttributeValue<string>("plt_cardnumber");
            e["plt_cardnumbermasked"] = customerCardRepository.getActiveCustomerCard(contactId).GetAttributeValue<string>("plt_cardnumber").Mask(0, 12, '*');

            e.Id = contactId;
            Service.Update(e);
        }
        public void updateContactDefaultCardFields(Guid contactId, Guid CardType, Guid customerCardId)
        {
            Entity e = new Entity("contact");
            e["plt_cardtypeid"] = new EntityReference("plt_cardtypes", CardType);
            e["plt_currentcardid"] = new EntityReference("plt_customercard", customerCardId);
            e.Id = contactId;
            Service.Update(e);
        }
        public void updateCustomerCard(UpdateCustomerCardRequest updateCustomerCardRequest)
        {
            CustomerCardBusiness customerCardBusiness = new CustomerCardBusiness(Service, TracingService);
            ContactRepository contactRepository = new ContactRepository(Service, TracingService);
            CardTypeRepository cardTypeRepository = new CardTypeRepository(Service, TracingService);
            CustomerCardRepository customerCardRepository = new CustomerCardRepository(Service, TracingService);

            var activeCard = customerCardRepository.getActiveCustomerCard(updateCustomerCardRequest.contactId);
            var contact = contactRepository.getContactById(updateCustomerCardRequest.contactId);
            var totalPoints = contact.GetAttributeValue<decimal>("plt_totalamount");

            var cardType = cardTypeRepository.getCardTypeByPoint(totalPoints);

            trace(string.Format("activeCard {0}", activeCard.Id));
            trace(string.Format(cardType == null ? "card null" : "card not null :" + cardType.Id));
            trace(string.Format("totalPoints {0}", totalPoints));
            trace(string.Format("contact.plt_cardtypeid {0}", contact.GetAttributeValue<EntityReference>("plt_cardtypeid").Id));
            trace(string.Format("cardType.id {0}", cardType.Id));

            if (cardType.Id != contact.GetAttributeValue<EntityReference>("plt_cardtypeid").Id)
            {
                trace("card changing start");
                var _id = customerCardBusiness.createCustomerCard(new CreateCustomerCardRequest
                {
                    contactId = contact.Id,
                    cardNumber = activeCard.GetAttributeValue<string>("plt_cardnumber"),
                    cardTypeId = cardType.Id
                });
                trace("card changing end");
                trace("deactivation start");
                customerCardBusiness.deactivateExistingCard(activeCard.Id);
                trace("deactivation end");

                this.updateContactDefaultCardFields(contact.Id, cardType.Id, _id);
            }
        }
    }
}
