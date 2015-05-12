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
    /// Interaction logic for CreateProductWindow.xaml
    /// </summary>
    public partial class CreateProductWindow : Window
    {
        public Boolean IsEdit { get; set; }
        public Boolean IsFacture { get; set; }
        public Product CurrentProduct { get; set; }
        public CreateProductWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prix = Convert.ToInt32(Prix.Text);
                var qte = Convert.ToInt32(Qte.Text);
                var product = new Product
                {
                    Code = IdDevis.Text,
                    Name = Name.Text,
                    Price = prix,
                    Quantity = qte,
                };
                if (this.IsEdit)
                {

                    CurrentProduct.Code = product.Code.ToString();
                    CurrentProduct.Name = product.Name;
                    CurrentProduct.Price = product.Price;
                    CurrentProduct.Quantity = product.Quantity;
                }
                else
                {
                    DbContext.DB.Products.Add(product);
                }
                DbContext.Save();
                this.Close();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Veuillez entrer un prix et ou une quantité sous forme de nombre");
            }
        }
    }
}
