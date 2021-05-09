using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EFInterview.Controllers
{
    public class GetDbDataController
    {

        public DataTable Customers { get; set; }

        public GetDbDataController()
        {
            DBContext dbc = new DBContext();
            this.Customers = dbc.ToDataTable<Customer>(dbc.Customers.ToList<Customer>());
        }

        public void Update(int id)
        {
            try
            {
                DataRow foundItem = Customers.Select("CustomerNumber = " + id).First();
                using (var context = new DBContext())
                {
                    // Retrieve entity by id
                    var entity = context.Customers.FirstOrDefault(item => item.CustomerNumber == id);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        // Make changes on entity
                        entity.Phone = foundItem.ItemArray[4].ToString();

                        // Save changes in database
                        context.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                //...
            }
        }


        public List<Customer> ManageCustomersAtRisk(int percent)
        {
            try
            {
                List<Customer> res = new List<Customer>();
                using (var dbc = new DBContext())
                {
                    Dictionary<int, double> ordersSumPerCustomer = new Dictionary<int, double>();

                    var orderDetails = dbc.Orderdetails;
                    var ordersByUser = dbc.Orders.AsEnumerable<Order>().GroupBy(ord => ord.CustomerNumber);

                    foreach (var ordsPerCust in ordersByUser)
                    {
                        ordersSumPerCustomer[ordsPerCust.Key] = 0;
                        foreach (var order in ordsPerCust)
                        {
                            var ordDetails = orderDetails.Where(item => item.OrderNumber == order.OrderNumber);
                            foreach (var ordsdet in ordDetails)
                            {
                                ordersSumPerCustomer[ordsPerCust.Key] += ordsdet.QuantityOrdered * (double)ordsdet.PriceEach;
                            }
                        }
                    }

                    var paymentsByUser = dbc.Payments.AsEnumerable<Payment>().GroupBy(
                    pay => pay.CustomerNumber,
                    pay => pay.Amount,
                    (customerNum, amount) => new
                    {
                        Key = customerNum,
                        Sum = amount.Sum()
                    });

                    foreach (var pay in paymentsByUser)
                    {
                        if ((double)pay.Sum < ordersSumPerCustomer[pay.Key] * (100 - percent) / 100)
                            res.Add(dbc.Customers.FirstOrDefault(cust => cust.CustomerNumber == pay.Key));
                    }
                }
                return res;
            }
            catch(Exception ex)
            {
                //...
                return null;
            }
        }

        #region forWPF

        //public DataTable GetCustomers()
        //{
        //    DBContext dbc = new DBContext();
        //    DataTable dt = dbc.ToDataTable<Customer>(dbc.Customers.ToList<Customer>());
        //    return dt;
        //}

        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            //#region ICommand Members  

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

            }

        }
        #endregion
    }
}
