using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class CTRegister: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _companyName { get; set; }

        [DataMember]
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if(_companyName!=value)
                {
                    _companyName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CompanyName"));
                }
            }
        }

        private string _companyNo { get; set; }

        [DataMember]
        public string CompanyNo
        {
            get { return _companyNo; }
            set
            {
                if (_companyNo != value)
                {
                    _companyNo = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CompanyNo"));
                }
            }
        }

        private string _companyPhoto { get; set; }

        [DataMember]
        public string CompanyPhoto
        {
            get { return _companyPhoto; }
            set
            {
                if (_companyPhoto != value)
                {
                    _companyPhoto = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CompanyPhoto"));
                }
            }
        }

        private string _username { get; set; }

        [DataMember]
        public string username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("username"));
                }
            }
        }

        private string _password { get; set; }

        [DataMember]
        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("password"));
                }
            }
        }

        private string _comfirmpassword { get; set; }

        [DataMember]
        public string comfirmpassword
        {
            get { return _comfirmpassword; }
            set
            {
                if (_comfirmpassword != value)
                {
                    _comfirmpassword = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("comfirmpassword"));
                }
            }
        }

        private string _mobilephone { get; set; }

        [DataMember]
        public string mobilephone
        {
            get { return _mobilephone; }
            set
            {
                if (_mobilephone != value)
                {
                    _mobilephone = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("mobilephone"));
                }
            }
        }

        private string _email { get; set; }

        [DataMember]
        public string email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("email"));
                }
            }
        }

        private string _name { get; set; }

        [DataMember]
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
                }
            }
        }

        private int _gender { get; set; }

        [DataMember]
        public int gender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("gender"));
                }
            }
        }
    }
}
