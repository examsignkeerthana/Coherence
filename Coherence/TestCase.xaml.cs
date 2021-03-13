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
using System.IO;
using System.Text;

namespace Coherence
{
    /// <summary>
    /// Interaction logic for TestCase.xaml
    /// </summary>
    public partial class TestCase : Page
    {
        public static string connectionString = @"Data Source=DESKTOP-JI48AUG\SQLEXPRESS;Initial Catalog=Coherence;Integrated Security=True";
        List<string> inputType = new List<string>();
        //ObservableCollection<string> par = new ObservableCollection<string>();
        ObservableCollection<string> mylist = new ObservableCollection<string>();
        SortedList<int, string> result = new SortedList<int, string>();
        List<string> Data = new List<string>();
        int quesId;
        List<string> type = new List<string> { "Integer","Double","String","Integer[]","Double[]","String[]"};

        ObservableCollection<TestCaseModel> tcm = new ObservableCollection<TestCaseModel>();
        ObservableCollection<Parameter> par = new ObservableCollection<Parameter>();

        public TestCase()
        {
            InitializeComponent();
           
        }

        public TestCase(int id)
        {
            InitializeComponent();
            quesId = id;
           // cmboboxType.ItemsSource = type;
        }

        
        private void btnAddParam_Click(object sender, RoutedEventArgs e)
        {
            par.Clear();
            int count = int.Parse(txtboxParameter.Text);

            for (int i = 0; i < count; i++)
            {
                par.Add(new Parameter 
                {ParameterName="",
                Type="",
                LowerBound="",
                UpperBound=""
                });

            }

            lstboxParameter.ItemsSource = par;


        }

        private void btnRemovePAram_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Parameter data = b.DataContext as Parameter;
            int index = par.IndexOf(data);
            
            par.RemoveAt(index);
        }

        private void btnAddTestCase_Click(object sender, RoutedEventArgs e)
        {
            tcm.Clear();
            int count = Convert.ToInt32(txtboxTestCase.Text);

            for (int i = 1; i <= count; i++)
            {
                tcm.Add(new TestCaseModel {Name="TestCase "+i, Input = "", Output = "" });
            }
            lstboxTestCase.ItemsSource = tcm;
        }
        private void btnDeleteSampleInput_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TestCaseModel data = b.DataContext as TestCaseModel;
            int index = tcm.IndexOf(data);

            tcm.RemoveAt(index);
        }

        private void GenreateFile(int Qid)
        {
            foreach (TestCaseModel item in tcm)
            {
                string input = "Input.txt";
                string output = "Output.txt";
                string tempfolderpath = System.IO.Path.GetTempPath();

                string _inputPath = System.IO.Path.Combine(tempfolderpath, input);
                string _outputPath = System.IO.Path.Combine(tempfolderpath, output);

                StreamWriter sw = new StreamWriter(_inputPath, true, Encoding.ASCII);
                sw.Write(item.Input);
                sw.Close();

                StreamWriter writer = new StreamWriter(_outputPath, true, Encoding.ASCII);
                writer.Write(item.Output);
                writer.Close();

                InsertTestCases(Qid, _inputPath, _outputPath);
                File.Delete(_inputPath);
                File.Delete(_outputPath);

            }
        }

        private void InsertTestCases(int Qid, string inputpath, string outputpath)
        {
            // int id = GetQuestionIdByTopicAndQuestion(courseName, topic, Ques);
            //System.IO.FileStream Ipfs = new System.IO.FileStream(inputpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //BinaryReader br = new BinaryReader(Ipfs);
            //byte[] input = br.ReadBytes((Int32)Ipfs.Length);

            //System.IO.FileStream Opfs = new System.IO.FileStream(outputpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //BinaryReader reader = new BinaryReader(Opfs);
            //byte[] output = reader.ReadBytes((Int32)Opfs.Length);

            byte[] input = FileToByteArray(inputpath);
            byte[] output = FileToByteArray(outputpath);



            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.database))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "insert into TestCases(Qid,Input,ExpectedOutput)values(" + Qid + ",'" + input + "','" + output + "')";
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
       
        public List<string> GenerateInt(int LowerRange, int HighestRange, int TotalTestCase, string ParameterName)
        {
            Random r = new Random();
            int sample = 0;
            int listCount = 0;
            Data.Add(ParameterName);
            while (listCount < TotalTestCase)
            {
                sample = r.Next(LowerRange, HighestRange);
                if (!Data.Contains(sample.ToString()))
                {
                    Data.Add(sample.ToString());
                    listCount++;
                }
            }
            return Data;
        }
        public List<string> GenerateDouble(int LowerRange, int HighestRange, int TotalTestCase, string ParameterName)
        {
            Random r = new Random();
            Data.Add(ParameterName);
            double sample = 0;
            int listCount = 0;
            while (listCount < TotalTestCase)
            {
                sample = r.NextDouble() * (HighestRange - LowerRange) + LowerRange;
                sample = Math.Round(sample, 2);
                if (!Data.Contains(sample.ToString()))
                {
                    listCount++;
                    Data.Add(sample.ToString());
                }
            }
            return Data;
        }
        public List<string> GenerateIntArray(int LowerRange, int HighestRange, int TotalTestCase, string ParameterName, int ArraySize)
        {
            Random r = new Random();
            Data.Add(ParameterName);
            int sample = 0;
            int listCount = 0;

            List<string> arraylist = new List<string>();
            while (listCount < TotalTestCase)
            {
                arraylist.Clear();
                while (arraylist.Count <= ArraySize)
                {
                    sample = r.Next(LowerRange, HighestRange);
                    if (!arraylist.Contains(sample.ToString()))
                    {
                        arraylist.Add(sample.ToString());
                    }
                }
                string[] NumberArray = new string[ArraySize];
                string s = "";
                for (int i = 0; i < NumberArray.Length; i++)
                {
                    s = s + arraylist.ElementAt(i) + " ";
                }
                if (!Data.Contains(s) && s != " ")
                {
                    Data.Add(s);
                    listCount++;
                }
            }
            return Data;
        }
        public List<string> GenerateDoubleArray(int LowerRange, int HighestRange, int TotalTestCase, string ParameterName, int ArraySize)
        {
            Random r = new Random();
            Data.Add(ParameterName);
            double sample = 0;
            int listCount = 0;

            List<string> arraylist = new List<string>();
            while (listCount < TotalTestCase)
            {
                arraylist.Clear();
                while (arraylist.Count <= ArraySize)
                {
                    sample = r.NextDouble() * (HighestRange - LowerRange) + LowerRange;
                    sample = Math.Round(sample, 2);
                    if (!arraylist.Contains(sample.ToString()))
                    {
                        arraylist.Add(sample.ToString());
                    }
                }
                string[] NumberArray = new string[ArraySize];
                string s = "";
                for (int i = 0; i < NumberArray.Length; i++)
                {
                    s = s + arraylist.ElementAt(i) + " ";
                }
                if (!Data.Contains(s) && s != " ")
                {
                    Data.Add(s);
                    listCount++;
                }
            }
            return Data;
        }
        public List<string> GenerateString(int length, int TotalTestcase)
        {
            Random r = new Random();
            //string Alphabets = "abcdefghijklmnopqrstuvwyz";
            StringBuilder str_build = new StringBuilder();
            while (TotalTestcase > 0)
            {
                int Slenth = length;
               
                char letter;
                str_build.Clear();
                while (Slenth > 0)
                {
                    double FloatValue = r.NextDouble();
                    int shift = Convert.ToInt32(Math.Floor(25 * FloatValue));
                    letter = Convert.ToChar(shift + 65);
                    str_build.Append(letter);
                    Slenth--;
                }
                if (!Data.Contains(str_build.ToString()))
                {
                    Data.Add(str_build.ToString());
                    TotalTestcase--;
                }
            }
            return Data;
        }

        private void btnGenerateTC_Click(object sender, RoutedEventArgs e)
        {
            int _noOfParameters = par.Count;
            for(int i=1;i<=_noOfParameters;i++)
            {

            }

        }

        private string ByteArrayToFile(string path, byte[] content)
        {
            //Save the Byte Array as File.
            //string filePath = bPath + "\\" + fName;
            File.WriteAllBytes(path, content);

            MessageBox.Show("File Generated");

            return path;
        }

        public byte[] FileToByteArray(string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);
            byte[] fileContent = br.ReadBytes((Int32)fs.Length);

            return fileContent;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            GenreateFile(quesId);

            MessageBox.Show("Inserted Successfully");

            //foreach (TestCaseModel item in tcm)
            //{
            //    Byte[] inp = new UTF8Encoding(true).GetBytes(item.Input);
            //    byte[] out=new UTF8Encoding(true).GetBytes()
            //    //InsertTestCases(quesId,)
            //}
        }

        
        private void txtboxInput_SelectionChanged(object sender, RoutedEventArgs e)
        {
    
        }

        private void lstboxTestCase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cmboboxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _type = cmboboxType.SelectedValue;
           
        }
    }
}
