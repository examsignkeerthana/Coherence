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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Coherence
{
    /// <summary>
    /// Interaction logic for Alternate.xaml
    /// </summary>
    public partial class Alternate : Page
    {
        public Alternate()
        {
            InitializeComponent();
        }

        private bool HasHint(int qid)
        {
            bool ans = false;
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select HasHint from Question where Qid=" + qid + "";
                    var res = cmd.ExecuteScalar();
                    ans = bool.Parse(res.ToString());

                    //if (ans)
                    //{ flag = true; }
                    //int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return ans;
        }
    }


}
