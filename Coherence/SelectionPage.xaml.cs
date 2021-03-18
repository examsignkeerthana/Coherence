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

namespace Coherence
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionPage : Page
    {
        SortedList<string, List<string>> courseDetails = new SortedList<string, List<string>>();
        public static string connectionString = @"Data Source=DESKTOP-JI48AUG\SQLEXPRESS;Initial Catalog=Coherence;Integrated Security=True";
        string topic;

        public SelectionPage()
        {
            InitializeComponent();
            GetCourses();
            cmboboxCourse.ItemsSource = courseDetails.Keys.ToList();

        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var obj = new DisplayQuestions(GetTopicIdByTopicAndCourseName(), cmboboxTopic.SelectedItem.ToString());
            NavigationService.GetNavigationService(this).Navigate(obj);

            //var obj = new LandingPage(cmboboxCourse.SelectedItem.ToString(), cmboboxTopic.SelectedItem.ToString());
            //NavigationService.GetNavigationService(this).Navigate(obj);

        }

        private void GetCourses()
        {
            List<string> crs = new List<string>();
            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select distinct(CourseName) from CourseTopics";
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        crs.Add(dr.GetString(0));
                    }

                    con.Close();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    for (int i = 0; i < crs.Count; i++)
                    {
                        List<string> topic = new List<string>();
                        cmd.CommandText = "select distinct(Topic) from CourseTopics where CourseName='" + crs[i] + "'";
                        SqlDataReader sqlDr = cmd.ExecuteReader();
                        while (sqlDr.Read())
                        {
                            topic.Add(sqlDr.GetString(0));
                        }
                        courseDetails.Add(crs[i], topic);
                    }

                    con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void cmboboxCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //topic = cmboboxCourse.SelectedItem.ToString();
            cmboboxTopic.ItemsSource = courseDetails[cmboboxCourse.SelectedItem.ToString()].ToList();
            
        }

        private int GetTopicIdByTopicAndCourseName()
        {
            int id = 0;
            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "select TopicId from CourseTopics where Topic='" + topic + "'";
                    var res = cmd.ExecuteScalar();
                    id = int.Parse(res.ToString());

                    con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return id;
        }

        private void cmboboxTopic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            topic = cmboboxTopic.SelectedItem.ToString();
            btnNext.IsEnabled = true;
        }
    }
}
