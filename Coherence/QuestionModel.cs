using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Coherence
{
    class QuestionModel
    {
        public int Qid { get; set; }

        public int TopicId { get; set; }

        public string QuestionStem { get; set; }

        public int IsHint { get; set; }

        public string Hint { get; set; }

        public int HasChallange { get; set; }

        public int ChallangeId { get; set; }

        public int HasAlternate { get; set; }

        public string AlternateId { get; set; }

        public int SampleTestCaseCount { get; set; }

        public int TestCaseCount { get; set; }
    }
    class TestCaseModel : INotifyPropertyChanged
    {
        string _name;
        public string Name
        {
            get 
            {
                return _name;
            }
            set 
            {
                _name = value;
                OnPropertyRaised("Name");
            }
        }

        string _input;
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
                OnPropertyRaised("Input");
            }
        }
        string _output;
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                OnPropertyRaised("Output");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }

    class Parameter : INotifyPropertyChanged
    {
        string _parameterName;

        public string ParameterName
        {
            get 
            {
                return _parameterName;
            }
            set
            {
                _parameterName = value;
                OnPropertyRaised(ParameterName);
            }
        }

        string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                string[] s = value.Split();

                _type = s[s.Length-1];
               
                OnPropertyRaised(Type);
               
            }
        }

        bool _selectedType;
        public bool SelectedType
        {
            get
            {
               // return this._type == "Ingerer[]" || this._type == "Double[]" || this._type == "String[]";
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                OnPropertyRaised(SelectedType.ToString());
            }
        }


        string _size;
        public string Size
        {
            get
            {
                return _size;
            }
            set
            {
               
                    _size = value;
               
               
                OnPropertyRaised(Size);
            }
        }

        string _lowerBound;
        public string LowerBound
        {
            get 
            {
                return _lowerBound;
            }
            set
            {
                _lowerBound = value;
                OnPropertyRaised(value);
            }
        }

        string _upperBound;
        public string UpperBound
        {
            get
            {
                return _upperBound;
            }
            set
            {
                _upperBound = value;
                OnPropertyRaised(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
    enum Status
    {
        Available = 1,
        NotAvailable = 0,
    }
}
