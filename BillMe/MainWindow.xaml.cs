using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BillMe.Models;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;
using System.ComponentModel;

namespace BillMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DbContext.SavingPath = @"C:\tests\db.json";
            DbContext.LoadingPath = @"C:\tests\db.json";
            if (!DbContext.Load())
            {
                DbContext.Initialize();
            }
            InitializeComponent();
            InitializeEnterprise();
        }

        private void InitializeEnterprise()
        {
            EnterpriseName.Text = DbContext.DB.Enterprise.Name;
            EnterpriseAddress.Text = DbContext.DB.Enterprise.Address;
            EnterprisePhone.Text = DbContext.DB.Enterprise.Telephone;
            EnterpriseMail.Text = DbContext.DB.Enterprise.Mail;
            EnterpriseWebsite.Text = DbContext.DB.Enterprise.Website;
        }


        //==========================================================================

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedIndex == 1)
            {
                try
                {
                    DataContext = DbContext.DB.Devis;
                    DevisGrid.Items.Refresh();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (TabControl.SelectedIndex == 2)
            {
                try
                {
                    DataContext = DbContext.DB.Factures;
                    FactureGrid.Items.Refresh();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (TabControl.SelectedIndex == 3)
            {
                try
                {
                    DataContext = DbContext.DB.Products;
                    FactureGrid.Items.Refresh();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private void ButtonCreateDevis_OnClick(object sender, RoutedEventArgs e)
        {
            var window2 = new CreateDevisWindow();
            window2.IdDevis.Text = "42";
            window2.Show();
            window2.Closing += refreshItems;
        }

        private void EditButtonDevis(object sender, RoutedEventArgs e)
        {
            var selectedDevis = DevisGrid.SelectedItem as Bill ?? new Bill();
            var editWindow = new CreateDevisWindow() {
                CurrentBill = selectedDevis
            };
            editWindow.CurrentBill = editWindow.CurrentBill ?? new Bill();
            editWindow.DataContext = editWindow.CurrentBill.Products ?? new List<Product>();

            editWindow.IdDevis.Text = selectedDevis.Id.ToString();
            editWindow.Mail.Text = selectedDevis.Mail;
            editWindow.Tel.Text = selectedDevis.Telephone;
            editWindow.Date.Text = selectedDevis.Date;
            editWindow.Client.Text = selectedDevis.Client;
            editWindow.Adress.Text = selectedDevis.Address;
            editWindow.IsEdit = true;

            editWindow.Show();
            editWindow.Closing += refreshItems;
        }


        private void RemoveButtonDevis(object sender, RoutedEventArgs e)
        {
            var selectedDevis = DevisGrid.SelectedItem as Bill;
            if (selectedDevis == null)
            {
                MessageBox.Show("Sélectionnez un Devis à supprimer");
            }
            else
            {
                if (MessageBox.Show("Confirmez-vous la Suppression ?", "Confirmation", MessageBoxButton.OKCancel) ==
                    MessageBoxResult.OK)
                {
                    DbContext.DB.Devis.Remove(selectedDevis);
                    DbContext.Save();
                }
            }
        }

        private void PrintButtonDevis(object sender, RoutedEventArgs e)
        {
            var selectedDevis = DevisGrid.SelectedItem as Bill;
            if (selectedDevis == null)
                return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            var path = fbd.SelectedPath + @"\Devis_" + selectedDevis.Id + ".pdf";
            if (PdfWriter.WriteDevis(selectedDevis, path, DbContext.DB.Enterprise))
                MessageBox.Show("Devis imprimmé !");
        }

        //===========================================================================

        private void ButtonCreateFacture_OnClick(object sender, RoutedEventArgs e)
        {
            var window2 = new CreateDevisWindow{IsFacture = true};
            window2.IdDevis.Text = "42";
            window2.Show();
            window2.Closing += refreshItems;
        }

        private void EditButtonFacture(object sender, RoutedEventArgs e)
        {
            var selectedDevis = DevisGrid.SelectedItem as Bill ?? new Bill();
            var editWindow = new CreateDevisWindow{IsFacture = true,
            CurrentBill = selectedDevis};

            editWindow.IdDevis.Text = selectedDevis.Id.ToString();
            editWindow.Mail.Text = selectedDevis.Mail;
            editWindow.Tel.Text = selectedDevis.Telephone;
            editWindow.Date.Text = selectedDevis.Date;
            editWindow.Client.Text = selectedDevis.Client;
            editWindow.Adress.Text = selectedDevis.Address;
            editWindow.IsEdit = true;

            editWindow.Show();
            editWindow.Closing += refreshItems;
        }


        private void RemoveButtonFacture(object sender, RoutedEventArgs e)
        {
            var selectedFacture = FactureGrid.SelectedItem as Bill;
            if (selectedFacture == null)
            {
                MessageBox.Show("Sélectionnez une Facture à supprimer");
            }
            else
            {
                if (MessageBox.Show("Confirmez-vous la Suppression ?", "Confirmation", MessageBoxButton.OKCancel) ==
                    MessageBoxResult.OK)
                {
                    DbContext.DB.Factures.Remove(selectedFacture);
                    DbContext.Save();
                }
            }
        }

        private void PrintButtonFacture(object sender, RoutedEventArgs e)
        {
            var selectedFacture = FactureGrid.SelectedItem as Bill;
            if (selectedFacture == null)
                return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            var path = fbd.SelectedPath + @"\Facture_" + selectedFacture.Id + ".pdf";
            if (PdfWriter.WriteFacture(selectedFacture, path, DbContext.DB.Enterprise))
                MessageBox.Show("Facture imprimmée !");
        }

        //===========================================================================
        private void EnterpriseButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var datas = new Enterprise
                {
                    Name = EnterpriseName.Text,
                    Mail = EnterpriseMail.Text,
                    Telephone = EnterprisePhone.Text,
                    Address = EnterpriseAddress.Text,
                    Website = EnterpriseWebsite.Text
                };
                var jSon = new JavaScriptSerializer().Serialize(datas);
                Console.WriteLine(jSon);
                DbContext.DB.Enterprise = datas;
                DbContext.Save();
                MessageBox.Show("Les informations ont bien été mises à jour !");
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur, les informations n'ont pas été mises à jour");
            }
        }

        //=====================================================================

        private void ButtonCreateProduit_OnClick(object sender, RoutedEventArgs e)
        {
            var productWindow = new CreateProductWindow();
            productWindow.Show();
            productWindow.Closing += refreshItems;
        }

        public void refreshItems(object sender, CancelEventArgs e)
        {
            if (TabControl.SelectedIndex == 1)
            {
                DevisGrid.Items.Refresh();
            }
            if (TabControl.SelectedIndex == 2)
            {
                FactureGrid.Items.Refresh();
            }
            if (TabControl.SelectedIndex == 3)
            {
                ProduitsGrid.Items.Refresh();
            }

        }
        private void RemoveButtonProduit(object sender, RoutedEventArgs e)
        {
            var selectedProduit = ProduitsGrid.SelectedItem as Product;
            if (selectedProduit == null)
            {
                MessageBox.Show("Sélectionnez un Produit à supprimer");
            }
            else
            {
                if (MessageBox.Show("Confirmez-vous la Suppression ?", "Confirmation", MessageBoxButton.OKCancel) ==
                    MessageBoxResult.OK)
                {
                    DbContext.DB.Products.Remove(selectedProduit);
                    DbContext.Save();
                }
            }
        }

        private void EditButtonProduit(object sender, RoutedEventArgs e)
        {
            var editWindow = new CreateProductWindow();
            var selectedProduct = DevisGrid.SelectedItem as Product ?? new Product();

            editWindow.CurrentProduct = selectedProduct;

            editWindow.IdDevis.Text = selectedProduct.Code;
            editWindow.Name.Text = selectedProduct.Name;
            editWindow.Prix.Text = selectedProduct.Price.ToString();
            editWindow.Qte.Text = selectedProduct.Quantity.ToString();
            editWindow.IsEdit = true;

            editWindow.Show();
            editWindow.Closing += refreshItems;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            var bill = DevisGrid.SelectedItem as Bill;
            if (bill != null)
            {
                var copy = new Bill
                {
                    Address = bill.Address,
                    Client = bill.Client,
                    Date = bill.Date,
                    Telephone = bill.Telephone,
                    Products = bill.Products,
                    Mail = bill.Mail
                };
                DbContext.DB.Factures.Add(copy);
                MessageBox.Show("done !");
            }
        }
    }
}
