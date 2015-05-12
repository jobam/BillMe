using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillMe.Models
{
    public class Db : INotifyPropertyChanged
    {
        private ObservableCollection<Bill> devis;
        private ObservableCollection<Bill> factures;
        private ObservableCollection<Product> products;
        private Enterprise enterprise;
        public ObservableCollection<Bill> Devis
        {
            get { return this.devis; }
            set
            {
                this.devis = value;
                NotifyPropertyChanged("Devis");
            }
        }

        public ObservableCollection<Bill> Factures
        {
            get { return this.factures; }
            set
            {
                this.factures = value;
                NotifyPropertyChanged("Factures");
            }
        }

        public ObservableCollection<Product> Products
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
