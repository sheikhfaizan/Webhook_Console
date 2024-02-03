using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhookConsoleApp.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public bool? DefaultForType { get; set; }
        public string ID { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public object JobTitle { get; set; } = new object();
        public string Phone { get; set; }
        public object MobilePhone { get; set; } = new object();
        public object Fax { get; set; } = new object();
        public string Email { get; set; }
        public object Website { get; set; } = new object();
        public bool? Default { get; set; }
        public object Comment { get; set; } = new object();
        public bool? IncludeInEmail { get; set; }
        public string ID { get; set; }
    }

    public class CustomerList
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string PaymentTerm { get; set; }
        public double? Discount { get; set; }
        public string TaxRule { get; set; }
        public string Carrier { get; set; }
        public object SalesRepresentative { get; set; } = new object();
        public string Location { get; set; }
        public string Comments { get; set; }
        public string AccountReceivable { get; set; }
        public string RevenueAccount { get; set; }
        public string PriceTier { get; set; }
        public string TaxNumber { get; set; }
        public string AdditionalAttribute1 { get; set; }
        public string AdditionalAttribute2 { get; set; }
        public string AdditionalAttribute3 { get; set; }
        public string AdditionalAttribute4 { get; set; }
        public string AdditionalAttribute5 { get; set; }
        public string AdditionalAttribute6 { get; set; }
        public string AdditionalAttribute7 { get; set; }
        public string AdditionalAttribute8 { get; set; }
        public string AdditionalAttribute9 { get; set; }
        public string AdditionalAttribute10 { get; set; }
        public object AttributeSet { get; set; } = new object();
        public string Tags { get; set; }
        public string Status { get; set; }
        public double? CreditLimit { get; set; }
        public bool? IsOnCreditHold { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public List<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    }

    public class ProductPrice
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public double? Price { get; set; }
    }

    public class CustomerListModel
    {
        public int? Total { get; set; }
        public int? Page { get; set; }
        public List<CustomerList> CustomerList { get; set; } = new List<CustomerList>();
    }


}
