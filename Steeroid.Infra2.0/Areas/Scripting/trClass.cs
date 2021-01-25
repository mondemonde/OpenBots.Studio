
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Steeroid.Infra2._0.Areas.Scripting
{
    public partial class tr
    {

        private trTD[] tdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("td")]
        public trTD[] td
        {
            get
            {
                return this.tdField;
            }
            set
            {
                this.tdField = value;
            }
        }

        [XmlIgnore]
        public string CommandName { 
            get
            {
                try
                {
                  return td[0].Text.FirstOrDefault();

                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
        }

        [XmlIgnore]
        public string Value
        {
            get
            {
                try
                {
                    return td[1].Text.FirstOrDefault();

                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    td[1].Text[0] = value;

                    if(td[1].datalist!=null)
                    td[1].datalist.option = value;
                    
                }
                catch (Exception)
                {


                }
            }

        }

        [XmlIgnore]
        public string Key
        {
            get
            {
                try
                {
                    return td[2].Text.FirstOrDefault();

                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
           set
            {
                try
                {
                    td[2].Text[0] = value;

                }
                catch (Exception)
                {

                    
                }
            }

        }




    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class trTD
    {

        private trTDDatalist datalistField;

        private string[] textField;

        /// <remarks/>
        public trTDDatalist datalist
        {
            get
            {
                return this.datalistField;
            }
            set
            {
                this.datalistField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }



    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class trTDDatalist
    {

        private string optionField;

        /// <remarks/>
        public string option
        {
            get
            {
                return this.optionField;
            }
            set
            {
                this.optionField = value;
            }
        }
    }


}
