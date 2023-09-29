﻿using csca5028.TransactionProcessor;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace components
{
    public class Sale
    {
        private List<SaleItem> items;
        //compute totalprice as the total price of all items in the list
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (SaleItem item in items)
                {
                    total += item.price * item.quantity;
                }
                return decimal.Round(total, 2);
            }
        }

        //compute totalItems as the total number of items in the list
        public int TotalItems
        {
            get
            {
                int total = 0;
                foreach (SaleItem item in items)
                {
                    total += item.quantity;
                }
                return total;
            }
        }

        //public decimal Totalprice { get; set; }

        public enum PaymentType { Cash, CreditCard, Check };

        public PaymentType paymentType;

        //give Sale a unique ID GUID
        public Guid ID;

        public Guid StoreID;

        public bool loyaltyCard;

        public CreditCardResponse CreditCardResponse { get; set; }

        //give Sale a constructor that takes all the fields as parameters
        public Sale(List<SaleItem> items, PaymentType paymentType, Guid storeID, bool loyaltyCard)
        {
            this.items = items;
            this.paymentType = paymentType;
            this.StoreID = storeID;
            this.loyaltyCard = loyaltyCard;
            ID = Guid.NewGuid();
        }
        
    }

    public class SaleItem
    {
        public string name;
        public int quantity;
        public decimal price;
    }
}
