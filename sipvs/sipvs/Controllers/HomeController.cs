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
using System.Net;
using Org.BouncyCastle;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
                id = fileId,
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

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
            namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            XmlElement xzepElement;
            if (xdoc.DocumentElement != null)
            {
                xzepElement = xdoc.DocumentElement;
                if (xzepElement != null)
                {
                    xzepElement.SetAttribute("xmlns:ds", "http://www.w3.org/2000/09/xmldsig#");
                }
            }
            
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
        
        
        [HttpGet("timeStamp")]
        public IActionResult TimeStamp()
        {
            
            XmlDocument xades = new XmlDocument();
            string filename = "./xadest_output.xml";
            xades.Load("./" + "xades_output.xml");

            var namespaceId = new XmlNamespaceManager(xades.NameTable);
            namespaceId.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            string data = xades.SelectSingleNode("//ds:SignatureValue", namespaceId).InnerXml;
                
            byte[] signature = System.IO.File.ReadAllBytes("./" + "xades_output.xml");
            Org.BouncyCastle.Crypto.IDigest digest = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            digest.BlockUpdate(signature, 0, signature.Length);
            byte[] signatureDigest = new byte[digest.GetDigestSize()];
            int outOff = 0;
            digest.DoFinal(signatureDigest, outOff);

            TimeStampRequestGenerator tsRequestGenerator = new TimeStampRequestGenerator(); // certificate generator
            tsRequestGenerator.SetCertReq(true);
            TimeStampRequest tsRequest = tsRequestGenerator.Generate(TspAlgorithms.Sha256, signatureDigest); // vygenerujeme request

            
            byte[] responseBytes = GetTimestamp(tsRequest.GetEncoded(), "https://test.ditec.sk/TSAServer/tsa.aspx");

            TimeStampResponse tsResponse = new TimeStampResponse(responseBytes);
            
            
            XmlNodeList elemList = xades.GetElementsByTagName("xades:QualifyingProperties");
            XmlElement UnsignedElem = xades.CreateElement("xades" , "UnsignedProperties", "http://uri.etsi.org/01903/v1.3.2#");
            XmlElement UnsignedSignElem = xades.CreateElement("xades" , "UnsignedSignatureProperties", "http://uri.etsi.org/01903/v1.3.2#");
            XmlElement SigTimeElem = xades.CreateElement("xades" , "SignatureTimeStamp", "http://uri.etsi.org/01903/v1.3.2#");
            XmlElement EncapsulatedTimestamp = xades.CreateElement("xades", "EncapsulatedTimeStamp", "http://uri.etsi.org/01903/v1.3.2#");
            SigTimeElem.SetAttribute("Id", "IdSignatureTimeStamp");
            EncapsulatedTimestamp.InnerText = Convert.ToBase64String(tsResponse.TimeStampToken.GetEncoded());

            UnsignedElem.AppendChild(UnsignedSignElem);
            UnsignedSignElem.AppendChild(SigTimeElem);
            SigTimeElem.AppendChild(EncapsulatedTimestamp);

            elemList[0].InsertAfter(UnsignedElem, elemList[0].LastChild);
            //Console.WriteLine(Convert.ToBase64String(tsResponse.TimeStampToken.GetEncoded()));
            xades.Save(filename);
            //Console.Write("text" + elemList[0]);  

            //return Ok(tsResponse.TimeStampToken.GetCertificates("Collection"));
            return Ok(filename);
        }
       
       public byte[] GetTimestamp(byte[] tsRequest, string tsUrl)
        {
            const string TS_QUERY_MIME_TYPE = "application/timestamp-query";
            const string TS_REPLY_MIME_TYPE = "application/timestamp-reply";

            string errorMessage = "OK";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(tsUrl);
                
                req.ServerCertificateValidationCallback +=
                    (sender, cert, chain, error) =>
                    {
                        return true;
                    };


                req.Method = "POST";
                req.ContentType = TS_QUERY_MIME_TYPE;
                req.ContentLength = tsRequest.Length;

                Stream requestStream = req.GetRequestStream();
                requestStream.Write(tsRequest, 0, tsRequest.Length);
                requestStream.Close();

                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    //verify response header
                    if (res.ContentType.ToLower() != TS_REPLY_MIME_TYPE.ToLower())
                    {
                        throw new Exception("incorrect response mimetype. " + res.ContentType);
                    }

                    using (Stream stm = new BufferedStream(res.GetResponseStream()))
                    {
                        stm.Flush();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stm.CopyTo(ms);
                            //Console.WriteLine(ms);
                            return ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return null;
            }
        }
        

    }
}