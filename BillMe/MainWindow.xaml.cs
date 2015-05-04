﻿using System;
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
        }

        private void ButtonCreateDevis_OnClick(object sender, RoutedEventArgs e)
        {
            var window2 = new CreateDevisWindow();
            window2.IdDevis.Text = "42";
            window2.Show();
        }

        private void EditButtonDevis(object sender, RoutedEventArgs e)
        {
            var editWindow = new CreateDevisWindow();
            var selectedDevis = DevisGrid.SelectedItem as Bill ?? new Bill();

            editWindow.IdDevis.Text = selectedDevis.Id.ToString();
            editWindow.Mail.Text = selectedDevis.Mail;
            editWindow.Tel.Text = selectedDevis.Telephone;
            editWindow.Date.Text = selectedDevis.Date;
            editWindow.Client.Text = selectedDevis.Client;
            editWindow.Adress.Text = selectedDevis.Address;
            editWindow.CurrentBill = selectedDevis;
            editWindow.IsEdit = true;

            editWindow.Show();
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
        }

        private void EditButtonFacture(object sender, RoutedEventArgs e)
        {
            var editWindow = new CreateDevisWindow{IsFacture = true};
            var selectedDevis = DevisGrid.SelectedItem as Bill ?? new Bill();

            editWindow.IdDevis.Text = selectedDevis.Id.ToString();
            editWindow.Mail.Text = selectedDevis.Mail;
            editWindow.Tel.Text = selectedDevis.Telephone;
            editWindow.Date.Text = selectedDevis.Date;
            editWindow.Client.Text = selectedDevis.Client;
            editWindow.Adress.Text = selectedDevis.Address;
            editWindow.CurrentBill = selectedDevis;
            editWindow.IsEdit = true;

            editWindow.Show();
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
    }
}
