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
using System.Xml.Xsl;

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
            if (file == "")
            {
                MessageBox.Show("Nebol nájdený uložený súbor!");
                return;
            }

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", "../../XML_scheme.xsd");
            XmlReader rd = XmlReader.Create(file);
            XDocument doc = XDocument.Load(rd);
            var success = true;
            doc.Validate(schema, (sender, e) =>
            {
                Console.WriteLine(e.Message);
                success = false;
                ValidationEventHandler(sender, e);
            }, true);

            if (success)
            {
                MessageBox.Show("Údaje spĺňajú zadanú štruktúru");
            }

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
            }
        }

        public static void TransformToHtml(string xmlPath, string xslPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath); 
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            xslTransform.Load(xslPath);
            
            using (StringWriter writer = new StringWriter())
            {
                // Perform the transformation
                xslTransform.Transform(xmlDoc, null, writer);

                // Get the transformed HTML as a string
                string transformedHtml = writer.ToString();

                // Print or use the transformed HTML as needed
                Console.WriteLine(transformedHtml);
                File.WriteAllText("../../../output.html", transformedHtml);
            }
        }

        
    }
}
