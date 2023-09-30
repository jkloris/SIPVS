using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using Z1_forms.model;

namespace Z1_forms
{
    public class FormManager
    {
        //source: https://www.youtube.com/watch?v=H6n33q-2hWc
        public static String SaveData(FormData data)
        {
            var id = Guid.NewGuid().ToString("N");
            String path = "../../../XML_output_" + id + ".xml";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FormData));
                serializer.Serialize(writer, data);
            }
            MessageBox.Show("Údaje boli uložené");

            return path;
        }

        //source: https://www.c-sharpcorner.com/article/how-to-validate-xml-using-xsd-in-c-sharp/
        public static void ValidateData(String file)
        {
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", "../../XML_scheme.xsd");
            XmlReader rd = XmlReader.Create(file);
            XDocument doc = XDocument.Load(rd);
            doc.Validate(schema, ValidationEventHandler);
        }

        //source: https://www.c-sharpcorner.com/article/how-to-validate-xml-using-xsd-in-c-sharp/
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                {
                    MessageBox.Show(e.Message); //throw new Exception(e.Message);
                }
                else MessageBox.Show("Údaje spĺňajú zadanú štruktúru");
            }
            else MessageBox.Show("Údaje spĺňajú zadanú štruktúru");
        }
    }
}
