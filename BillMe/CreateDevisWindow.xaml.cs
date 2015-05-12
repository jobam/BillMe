using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BillMe.Models;
using System.Web.Script.Serialization;

namespace BillMe
{
    /// <summary>
    /// Interaction logic for CreateDevisWindow.xaml
    /// </summary>
    public partial class CreateDevisWindow : Window
    {
        public Boolean IsEdit { get; set; }
        public Boolean IsFacture { get; set; }
        public Bill CurrentBill { get; set; }
        public CreateDevisWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(IdDevis.Text);
                var bill = new Bill
                {
                    Id = id,
                    Date = Date.DisplayDate.ToShortDateString(),
                    Client = Client.Text,
                    Telephone = Tel.Text,
                    Mail = Mail.Text,
                    Address = Adress.Text
                };
                if (this.IsEdit)
                {
                    Bill currentBill;
                    if (!this.IsFacture)
                        currentBill = DbContext.DB.Devis.ToList<Bill>().Find(x => x.Equals(this.CurrentBill));
                    else
                        currentBill = DbContext.DB.Factures.ToList<Bill>().Find(x => x.Equals(this.CurrentBill));
                    currentBill.Id = bill.Id;
                    currentBill.Date = bill.Date;
                    currentBill.Client = bill.Client;
                    currentBill.Telephone = bill.Telephone;
                    currentBill.Mail = bill.Mail;
                    currentBill.Address = bill.Address;
                }
                else
                {
                    if (this.IsFacture)
                        DbContext.DB.Factures.Add(bill);
                    else
                        DbContext.DB.Devis.Add(bill);
                }
                DbContext.Save();
                this.Close();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Veuillez entrer un Numéro de Devis / Facture sous forme de nombre");
            }
        }
    }
}
