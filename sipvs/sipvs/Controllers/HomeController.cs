using Microsoft.AspNetCore.Mvc;
using sipvs.Models;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using Z1_forms.model;
using System.Xml.Xsl;
using System.IO;

namespace sipvs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(FormData data)
        {
            data.fillOutEmptyData();
            var id = Guid.NewGuid().ToString("N");
            String path = "XML_output_" + id + ".xml";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("taxBonusForm", "http://www.taxBonusForm.com");
                XmlSerializer serializer = new XmlSerializer(typeof(FormData));
                serializer.Serialize(writer, data, ns);
                
            }
            return View("Views/Home/Index.cshtml");
        }

        public ActionResult ValidateData(FormData data)
        {
            var id = Guid.NewGuid().ToString("N");
            String path = "XML_output_" + id + ".xml";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("taxBonusForm", "http://www.taxBonusForm.com");
                XmlSerializer serializer = new XmlSerializer(typeof(FormData));
                serializer.Serialize(writer, data, ns);
            }

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("http://www.taxBonusForm.com", "XML_scheme.xsd");
            XmlReader rd = XmlReader.Create(path);
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
                return Content("Údaje spĺňajú zadanú štruktúru");
                //MessageBox.Show("Údaje spĺňajú zadanú štruktúru");
            }

            return Content("Údaje nespĺňajú zadanú štruktúru");
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                {
                    //MessageBox.Show(e.Message); //throw new Exception(e.Message);
                }
            }
        }

        public ActionResult TransformToHtml(FormData data)
        {
            var id = Guid.NewGuid().ToString("N");
            String path = "XML_output_" + id + ".xml";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("taxBonusForm", "http://www.taxBonusForm.com");
                XmlSerializer serializer = new XmlSerializer(typeof(FormData));
                serializer.Serialize(writer, data, ns);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            xslTransform.Load("xsltest.xsl");

            using (StringWriter writer = new StringWriter())
            {
                // Perform the transformation
                xslTransform.Transform(xmlDoc, null, writer);

                // Get the transformed HTML as a string
                string transformedHtml = writer.ToString();

                // Print or use the transformed HTML as needed
                System.IO.File.AppendAllText("output.html", transformedHtml);
            }
            return Content("Údaje sa transformovali do html");
        }
    }
}