using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using Z1_forms.model;
using System.Xml.Xsl;
using System.Text.Json;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Reflection.Metadata;
using Aspose.Pdf;
using Document = Aspose.Pdf.Document;

namespace sipvs.Controllers
{
    public class HomeController : Controller
    {


        public static string fileId = null;

      
        public IActionResult Index()
        {
            return View();
        }

        public  void GeneratePdf(String path)
        {
            HtmlLoadOptions htmloptions = new HtmlLoadOptions();
            // Load HTML file
            Document doc = new Document(path, htmloptions);
            // Convert HTML file to PDF
            doc.Save("output.pdf");
        }


        [HttpPost]
        public ActionResult Submit(FormData data)
        {
           
            TempData["Kids"] = data.kids;
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
            GeneratePdf("output.html");
            string jsonString = JsonSerializer.Serialize(new
            {
                xml_file = System.IO.File.ReadAllText(Path.Combine("./", "XML_" + fileId + "output_.xml")),
                xsl_file = System.IO.File.ReadAllText(Path.Combine("./", "xsltest.xsl")),
                xsd_file = System.IO.File.ReadAllText(Path.Combine("./", "XML_scheme.xsd")),
                pdf64_file = ConvertPdfToBase64("output.pdf")
            });
            return Ok(jsonString);
        }

     
            public string ConvertPdfToBase64(string pdfFilePath)
            {
                if (System.IO.File.Exists(pdfFilePath))
                {
                    try
                    {
                        byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                        string base64String = Convert.ToBase64String(pdfBytes);

                        return base64String;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                    return null;
                }
            }
        

        //táto funkcia sa síce zavolá ale nedôdu do nej dáta a vytvorí sa iba prázdny súbor
        [HttpPost("xades")]
        public IActionResult saveXades([FromBody] Models.Signature signature)
        {
            if (fileId == null)
            {
                return Ok("Najskôr uložte súbor");
            }
            string data = signature.data;
            string fileName = "xades_output.xml";

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(data);
            xdoc.Save(fileName);

            return Ok(fileName);

        }

        public ActionResult ValidateData(FormData data)
        {
            TempData["Kids"] = data.kids;

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

            try
            {
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
            TempData["Kids"] = data.kids;
            if (fileId == null)
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
                
            }
            ViewData["confirmation"] = "Formulár bol transformovaný";

            
            return View("Views/Home/Index.cshtml");


        }
    }
}