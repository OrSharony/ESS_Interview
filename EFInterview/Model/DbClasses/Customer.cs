using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace EFInterview
{
    public partial class Customer:INotifyPropertyChanged
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
        }

        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactFirstName { get; set; }
        public string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int? SalesRepEmployeeNumber { get; set; }
        public decimal? CreditLimit { get; set; }

        public virtual Employee SalesRepEmployeeNumberNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
