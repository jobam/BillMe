using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillMe.Models
{
    public class Db : INotifyPropertyChanged
    {
        private List<Bill> devis;
        private List<Bill> factures;
        private List<Product> products;
        private Enterprise enterprise;
        public List<Bill> Devis
        {
            get { return this.devis; }
            set
            {
                this.devis = value;
                NotifyPropertyChanged("Devis");
            }
        }

        public List<Bill> Factures
        {
            get { return this.factures; }
            set
            {
                this.factures = value;
                NotifyPropertyChanged("Factures");
            }
        }

        public List<Product> Products
        {
            get { return this.products; }
            set
            {
                this.products = value;
                NotifyPropertyChanged("Products");
            }
            
        }

        public Enterprise Enterprise
        {
            get { return this.enterprise; }
            set
            {
                this.enterprise = value;
                NotifyPropertyChanged("Enterprise");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(String info)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
    }
}
