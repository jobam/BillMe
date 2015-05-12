using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BillMe.Models
{
    public static class DbContext
    {
        public static Db DB { get; set; }
        public static String LoadingPath { get; set; }
        public static String SavingPath { get; set; }

        public static Boolean Load()
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                using (var reader = new StreamReader(LoadingPath))
                {
                    DB = serializer.Deserialize<Db>(reader.ReadToEnd());
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static Boolean Save()
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                using (var writer = new StreamWriter(SavingPath))
                {
                    writer.Write(serializer.Serialize(DB));
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static void Initialize()
        {
            DB = new Db();
            DB.Devis = new ObservableCollection<Bill>();
            DB.Enterprise = new Enterprise();
            DB.Factures = new ObservableCollection<Bill>();
            DB.Products = new ObservableCollection<Product>();
        }
    }
}
