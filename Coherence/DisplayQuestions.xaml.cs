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
    /// Interaction logic for DisplayQuestions.xaml
    /// </summary>
    public partial class DisplayQuestions : Page
    {
        public static string connectionString = @"Data Source=DESKTOP-JI48AUG\SQLEXPRESS;Initial Catalog=Coherence;Integrated Security=True";
        int topicId;
        Dictionary<int, string> TempDetails = new Dictionary<int, string>();
        ObservableCollection<QuestionModel> QuestionDetailsResult = new ObservableCollection<QuestionModel>();
        int qid;

        public DisplayQuestions()
        {
            InitializeComponent();
        }

        public DisplayQuestions(int id,string topic)
        {
            InitializeComponent();
            topicId = id;
            txtboxTopic.Text = topic;
            GetQuestionAndTestCaseDetails();
            lstboxDisplayQuestion.ItemsSource = TempDetails.Values;
        }

        //public void GetQidByTopicId()
        //{
            
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
        //        {
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = con;
        //            con.Open();

        //            cmd.CommandText = "select Qid,QuestionStem,HasHint,HasChallange,HasAlternate from Question where TopicId=" + topicId+"";
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                QuestionModel qm = new QuestionModel();
        //                qm.Qid = Convert.ToInt32(dr["Qid"]);
        //                qm.QuestionStem = dr["Qid"].ToString();

        //                qm.IsHint= Convert.ToInt32(dr["HasHint"]);
        //                qm.HasChallange = Convert.ToInt32(dr["HasChallange"]);
        //                qm.HasAlternate = Convert.ToInt32(dr["HasAlternate"]);
        //                QuestionDetailsResult.Add(qm);
        //            }

        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        public void GetQuestionAndTestCaseDetails()
        {
           
            SqlConnection sqlcon = new SqlConnection(connectionString);
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("select Qid,QuestionStem from Question where TopicId='" + topicId + "'", sqlcon);
            SqlDataReader reader1 = cmd.ExecuteReader();
            while (reader1.Read())
            {
                int _qId = reader1.GetInt32(0);
                string _questionStem = reader1.GetString(1);
                TempDetails.Add(_qId, _questionStem);
            }
            reader1.Close();
            foreach (var item in TempDetails)
            {
                cmd.CommandText = "select Count(IsSample) from TestCases where Qid='" + item.Key + "'and IsSample=1";
                int _sample = (int)cmd.ExecuteScalar();
                cmd.CommandText = "select Count(IsSample) from TestCases where Qid='" + item.Key + "'and IsSample=0";
                int _nonSample = (int)cmd.ExecuteScalar();
                //if(_sample>0 && _nonSample>0)
                //{

                //}
                //else if((_sample==0 && _nonSample>0)||(_sample>0&&_nonSample==0))
                //{

                //}
                //else
                //{

                //}
                QuestionDetailsResult.Add(new QuestionModel { Qid = item.Key, QuestionStem = item.Value, SampleTestCaseCount = _sample, TestCaseCount = _nonSample });
            }
        }

        public void GetQuestions()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select Qid, QuestionStem from Question where TopicId = " + topicId + "";
                    //var res = cmd.ExecuteScalar();
                    var res = cmd.ExecuteNonQuery();
                    
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstboxDisplayQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = sender as TextBlock;
            //Dictionary<int , string> temp = a.DataContext as Dictionary<int, string>;
            
            var obj = lstboxDisplayQuestion.SelectedValue;
            
            foreach (var item in TempDetails)
            {
                if (item.Value.Equals(obj))
                {
                    qid = item.Key;
                    var nav = new LandingPage(qid);
                    NavigationService.GetNavigationService(this).Navigate(nav);
                }
            }

        }
    }
}
