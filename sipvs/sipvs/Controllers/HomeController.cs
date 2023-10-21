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
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text;


namespace sipvs.Controllers
{
    public class HomeController : Controller
    {
        
        public static string fileId = null;
        public IActionResult Index()
        {
            return View();
        }
         
        [HttpPost]
        public ActionResult Submit(FormData data)
        {
            if (!ModelState.IsValid)
            {
                ViewData["rejection"] = "Neplatný formulár";
                return View("Views/Home/Index.cshtml");
            }
            data.fillOutEmptyData();

            fileId = Guid.NewGuid().ToString("N");
            String path = "XML_" + fileId + "output_.xml";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("taxbonusform", "http://www.taxbonusform.com");
                
                XmlSerializer serializer = new XmlSerializer(typeof(FormData));
                serializer.Serialize(writer, data, ns);
                
            }
            ViewData["confirmation"] = "Formulár bol uložený";
            return View("Views/Home/Index.cshtml");
        }
        
        [HttpGet("sign")]
        public IActionResult signDocument()
        {
            if (fileId == null)
            {
                return Ok("Najskôr uložte súbor");
            }
            string jsonString = JsonSerializer.Serialize(new {
                xml_file =System.IO.File.ReadAllText(Path.Combine("./","XML_" + fileId + "output_.xml")),
                xsl_file = System.IO.File.ReadAllText(Path.Combine("./","xsltest.xsl")),
                xsd_file = System.IO.File.ReadAllText(Path.Combine("./","XML_scheme.xsd")),
            });
            return Ok(jsonString);
        }
        
        //táto funkcia sa síce zavolá ale nedôdu do nej dáta a vytvorí sa iba prázdny súbor
        [HttpPost("xades")]
        public IActionResult saveXades(string data)
        { 
            
            string fileName = "xades.xml";
            System.IO.File.WriteAllText("foo.xml", data);
            return Ok(fileName);
          
        }

        public ActionResult ValidateData(FormData data)
        {

           
            if (fileId == null)
            {
                ViewData["rejection"] = "Najprv ulozte subor!";
                return View("Views/Home/Index.cshtml");
            }
            String path = "XML_" + fileId + "output_.xml";

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("http://www.taxbonusform.com", "XML_scheme.xsd");

            XmlReader rd = XmlReader.Create(path);
            XDocument doc = XDocument.Load(rd);

            try {
                doc.Validate(schema, (sender, e) =>
                {
                    Console.WriteLine(e.Message);
                    ValidationEventHandler(sender, e);
                }, true);
            }
            catch
            {
                ViewData["rejection"] = "Údaje nespĺňajú zadanú štruktúru";
                return View("Views/Home/Index.cshtml");

            }
            ViewData["confirmation"] = "Údaje sĺňajú zadanú štruktúru";
            return View("Views/Home/Index.cshtml");
        }


        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Warning || type == XmlSeverityType.Error)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public ActionResult TransformToHtml(FormData data)
        {
            if(fileId == null)
            {
                ViewData["rejection"] = "Najprv ulozte subor!";
                return View("Views/Home/Index.cshtml");
            }
            String path = "XML_" + fileId + "output_.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            xslTransform.Load("xsltest.xsl");

            using (StringWriter writer = new StringWriter())
            {
                xslTransform.Transform(xmlDoc, null, writer);
                string transformedHtml = writer.ToString();
                System.IO.File.WriteAllText("output.html", transformedHtml);
            //return Content(transformedHtml);
            }
            ViewData["confirmation"] = "Formulár bol transformovaný";
            return View("Views/Home/Index.cshtml");
        }
    }
}