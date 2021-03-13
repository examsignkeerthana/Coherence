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
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Coherence
{
    /// <summary>
    /// Interaction logic for Challange.xaml
    /// </summary>
    public partial class Challange : Page
    {
        public static string connectionString = @"Data Source=DESKTOP-JI48AUG\SQLEXPRESS;Initial Catalog=Coherence;Integrated Security=True";

        ObservableCollection<string> challangelst = new ObservableCollection<string>();

        int quesId = 0;

        public Challange()
        {
            InitializeComponent();
        }

        public Challange(int qid)
        {
            InitializeComponent();
            quesId = qid;

            txtblckDisplayQues.Text = GetQuestionByQid(qid);

            if (HasHint(qid))
            {
                txtblckDisplayHint.Text = GetHintByQid(qid);
            }
            else
            {
                txtblckDisplayHint.Visibility = Visibility.Collapsed;
            }



        }

        private void btnChallangeRemove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string s = btn.DataContext as string;
            int index = challangelst.IndexOf(s);

            challangelst.RemoveAt(index);
        }

        private void btnAddChallange_Click(object sender, RoutedEventArgs e)
        {
            challangelst.Add(txtboxChallangeStem.Text);
            lstboxChallange.ItemsSource = challangelst;
            txtboxChallangeStem.Text = "";
            txtboxChallangeStem.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            QuestionModel questionModel = new QuestionModel();
            questionModel.Qid = quesId;

            for (int i = 0; i < challangelst.Count; i++)
            {
                InsertChallange(questionModel.Qid, challangelst[i]);
            }
            UpdateChallange(questionModel.Qid);
        }

        private string GetQuestionByQid(int qid)
        {
            string s = "";

            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select QuestionStem from Question where Qid=" + qid + "";
                    var res = cmd.ExecuteScalar();
                    s = res.ToString();
                    //int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return s;
        }

        

        private bool HasHint(int qid)
        {
            bool ans = false ;
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

        private string GetHintByQid(int qid)
        {
            string s = "";

            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select Hint from Question where Qid=" + qid + " and HasHint=" + 1 + "";
                    var res = cmd.ExecuteScalar();
                    s = res.ToString();
                    //int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return s;
        }

        private void InsertChallange(int Qid, string stem)
        {
            //int id = GetQuestionIdByTopicAndQuestion(courseName, topic, Ques);

            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "insert into Challange(Qid, ChallangeStem) values(" + Qid + ", '" + stem + "')";
                    //var res = cmd.ExecuteScalar();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UpdateChallange(int Qid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "update Question set HasChallange=" + 1 + " where Qid=" + Qid + "";
                    //var res = cmd.ExecuteScalar();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
