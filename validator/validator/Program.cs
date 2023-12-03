using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Tsp;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.Zlib;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.XPath;
using System.Security.Cryptography.X509Certificates;

static bool validEnvelope(XmlDocument doc)
{

    XmlElement rootElement = doc.DocumentElement;

    // check if the root element has the required attributes
    if (rootElement != null &&
        rootElement.HasAttribute("xmlns:xzep") &&
        rootElement.HasAttribute("xmlns:ds"))
    {
        string xzepValue = rootElement.GetAttribute("xmlns:xzep");
        if (xzepValue == null || xzepValue != "http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0")
        {
            Console.WriteLine("Koreňový element nemá v atribúte xmlns:xzep hodnotu \"http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0\"");
            return false;
        }

        string dsValue = rootElement.GetAttribute("xmlns:ds");
        if (dsValue == null || dsValue != "http://www.w3.org/2000/09/xmldsig#")
        {
            Console.WriteLine("Koreňový element nemá v atribúte xmlns:ds hodnotu \"http://www.w3.org/2000/09/xmldsig#\"");
            return false;
        }
    }
    return true;
}

static bool validAlgorithms(XmlDocument doc)
{
    // select SignatureMethod and CanonicalizationMethod elements
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signatureMethodNode = doc.SelectSingleNode("//ds:SignatureMethod", namespaceManager);
    XmlNode canonicalizationMethodNode = doc.SelectSingleNode("//ds:CanonicalizationMethod", namespaceManager);

    // Check if the elements are found
    if (signatureMethodNode != null && canonicalizationMethodNode != null)
    {

        List<string> signatureAlgorithms = new List<string> {   "http://www.w3.org/2000/09/xmldsig#dsa-sha1",
                                                                "http://www.w3.org/2000/09/xmldsig#rsa-sha1",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512"};

        // Get the Algorithm attribute values
        XmlNode signatureMethodAlgorithmNode = signatureMethodNode.Attributes["Algorithm"];
        if (signatureMethodAlgorithmNode != null)
        {
            String signatureMethodAlgorithm = signatureMethodAlgorithmNode.Value;
            if (signatureMethodAlgorithm == null || !signatureAlgorithms.Contains(signatureMethodAlgorithm))
            {
                Console.WriteLine("Element ds:SignatureMethod nemá v atribúte Algorithm podporovanú hodnotu");
                return false;
            }

        }
        else
        {
            Console.WriteLine("Element ds:SignatureMethod neobsahuje atribút Algorithm");
            return false;
        }

        XmlNode canonicalizationMethodAlgorithmNode = canonicalizationMethodNode.Attributes["Algorithm"];
        if (canonicalizationMethodAlgorithmNode != null)
        {
            String canonicalizationMethodAlgorithm = canonicalizationMethodAlgorithmNode.Value;

            if (canonicalizationMethodAlgorithm == null || canonicalizationMethodAlgorithm != "http://www.w3.org/TR/2001/REC-xml-c14n-20010315")
            {
                Console.WriteLine("Element ds:CanonicalizationMethod nemá v atribúte Algorithm podporovanú hodnotu");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Element ds:CanonicalizationMethod neobsahuje atribút Algorithm");
            return false;
        }
    }
    else
    {
        Console.WriteLine("XML súbor neobsahuje element ds:SignatureMethod alebo ds:CanonicalizationMethod");
        return false;
    }

    return true;
}

static bool validTransform(XmlDocument doc)
{
    // select ds:Transforms and ds:DigestMethod
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList transforms = doc.SelectNodes("//ds:Transform", namespaceManager);
    XmlNodeList digestMethods = doc.SelectNodes("//ds:DigestMethod", namespaceManager);

    // Check if the elements are found
    if (transforms != null && digestMethods != null)
    {
        List<string> digestAlgorithms = new List<string> {   "http://www.w3.org/2000/09/xmldsig#sha1",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha224",
                                                             "http://www.w3.org/2001/04/xmlenc#sha256",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha384",
                                                             "http://www.w3.org/2001/04/xmlenc#sha512"};


        List<string> transformAlgorithms = new List<string> {   "http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                                                                "http://www.w3.org/2000/09/xmldsig#base64"};

        // Get the Algorithm attribute values
        foreach (XmlNode transform in transforms)
        {
            XmlAttribute transformsAlgorithmNode = transform.Attributes["Algorithm"];
            if (transformsAlgorithmNode != null)
            {
                String transformsAlgorithm = transformsAlgorithmNode.Value;
                if (transformsAlgorithm == null || !transformAlgorithms.Contains(transformsAlgorithm))
                {
                    Console.WriteLine("Element ds:Transform nemá v atribúte Algorithm podporovanú hodnotu");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Element ds:Transform neobsahuje atribút Algorithm");
                return false;
            }
        }

        foreach (XmlNode digestMethod in digestMethods)
        {
            XmlAttribute digestMethodAlgorithmNode = digestMethod.Attributes["Algorithm"];
            if (digestMethodAlgorithmNode != null)
            {
                String digestMethodAlgorithm = digestMethodAlgorithmNode.Value;
                if (digestMethodAlgorithm == null || !digestAlgorithms.Contains(digestMethodAlgorithm))
                {
                    Console.WriteLine("Element ds:DigestMethod nemá v atribúte Algorithm podporovanú hodnotu");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Element ds:DigestMethod neobsahuje atribút Algorithm");
                return false;
            }
        }
    }
    else
    {
        Console.WriteLine("XML súbor neobsahuje element ds:Transform alebo ds:DigestMethod");
        return false;
    }

    return true;
}

static bool validSignatureID(XmlDocument doc)
{
    // select ds:Signature
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signature = doc.SelectSingleNode("//ds:Signature", namespaceManager);
    
    // check if signature is valid
    if (signature != null && signature.Attributes["Id"] != null)
        return true;

    Console.WriteLine("Element ds:Signature neobsahuje atribút Id");
    return false;
}

static bool validSignatureValueID(XmlDocument doc)
{
    // select ds:SignatureValue
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signatureValue = doc.SelectSingleNode("//ds:SignatureValue", namespaceManager);

    // check if signature value is valid
    if (signatureValue != null && signatureValue.Attributes["Id"] != null)
        return true;

    Console.WriteLine("Element ds:SignatureValue neobsahuje atribút Id");
    return false;
}

static bool validReferences(XmlDocument doc)
{
    List<string> types = new List<string> { "http://www.w3.org/2000/09/xmldsig#Object",
                                            "http://www.w3.org/2000/09/xmldsig#SignatureProperties",
                                            "http://uri.etsi.org/01903#SignedProperties",
                                            "http://www.w3.org/2000/09/xmldsig#Manifest" };

    //obtain ds:SignedInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList references = doc.SelectNodes("//ds:SignedInfo/ds:Reference", namespaceManager);

    if (references != null)
    {
        foreach (XmlNode reference in references)
        {
            XmlNode referenceType = reference.Attributes["Type"];
            if (referenceType == null)
            {
                Console.WriteLine("Element ds:SignedInfo neobsahuje atribút Type");
                return false;
            }

            // check reference to KeyInfor
            if (referenceType.Value == types[0])
            {
                String keyInfoId = reference.Attributes["URI"].Value;
                XmlNode keyInfoNode = doc.SelectSingleNode("//ds:KeyInfo", namespaceManager);

                if (keyInfoNode == null)
                {
                    Console.WriteLine("XML súbor neobsahuje element ds:KeyInfo");
                    return false;
                }

                XmlNode keyInfoNodeId = keyInfoNode.Attributes["Id"];
                if (keyInfoNodeId == null)
                {
                    Console.WriteLine("Element ds:KeyInfo neobsahuje atribút Id");
                    return false;
                }

                if (keyInfoNodeId.Value != keyInfoId)
                {
                    Console.WriteLine("Atribút Id elementu ds:KeyInfo sa nezhoduje s atribútom URI elementu ds:SignedInfo");
                    return false;
                }

                types[0] = "";
            }

            //check reference to SignatureProperties
            else if (referenceType.Value == types[1])
            {
                String signaturePropertiesId = reference.Attributes["URI"].Value;
                XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:SignatureProperties", namespaceManager);

                if (signaturePropertiesNode == null)
                {
                    Console.WriteLine("XML súbor neobsahuje element ds:SignatureProperties");
                    return false;
                }

                XmlNode signaturePropertiesNodeId = signaturePropertiesNode.Attributes["Id"];
                if (signaturePropertiesNodeId == null)
                {
                    Console.WriteLine("Element ds:SignatureProperties neobsahuje atribút Id");
                    return false;
                }

                if (signaturePropertiesNodeId.Value != signaturePropertiesId)
                {
                    Console.WriteLine("Atribút Id elementu ds:signatureProperties sa nezhoduje s atribútom URI elementu ds:SignedInfo");
                    return false;
                }

                types[1] = "";
            }

            //check reference to SignedProperties
            else if (referenceType.Value == types[2])
            {
                String signedPropertiesId = reference.Attributes["URI"].Value;

                XmlNamespaceManager xadesNamespaceManager = new XmlNamespaceManager(doc.NameTable);
                xadesNamespaceManager.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");
                XmlNode signedPropertiesNode = doc.SelectSingleNode("//xades:SignedProperties", xadesNamespaceManager);

                if (signedPropertiesNode == null)
                {
                    Console.WriteLine("XML súbor neobsahuje element xades:SignedProperties");
                    return false;
                }

                XmlNode signedPropertiesNodeId = signedPropertiesNode.Attributes["Id"];
                if (signedPropertiesNodeId == null)
                {
                    Console.WriteLine("Element xades:SignedProperties neobsahuje atribút Id");
                    return false;
                }

                if (signedPropertiesNodeId.Value != signedPropertiesId)
                {
                    Console.WriteLine("Atribút Id elementu xades:SignedProperties sa nezhoduje s atribútom URI elementu ds:SignedInfo");
                    return false;
                }

                types[2] = "";
            }

            //check reference to manifest
            else if (referenceType.Value == types[3])
            {
                String manifestId = reference.Attributes["URI"].Value;
                XmlNode manifestNode = doc.SelectSingleNode("//ds:Manifest", namespaceManager);

                if (manifestNode == null)
                {
                    Console.WriteLine("XML súbor neobsahuje element ds:Manifest");
                    return false;
                }

                XmlNode manifestNodeId = manifestNode.Attributes["Id"];
                if (manifestNodeId == null)
                {
                    Console.WriteLine("Element ds:Manifest neobsahuje atribút Id");
                    return false;
                }

                if (manifestNodeId.Value != manifestId)
                {
                    Console.WriteLine("Atribút Id elementu ds:Manifest sa nezhoduje s atribútom URI elementu ds:SignedInfo");
                    return false;
                }
            }

            else
            {
                Console.WriteLine("Element ds:SignedInfo má v atribúte Type nepodporovanú hodnotu"); 
                return false;
            }
        }

    }

    if (types[0] != "" || types[1] != "" || types[2] != "")
    {
        Console.WriteLine("Element ds:SignedInfo neobsahuje všetky povinné referencie");
        return false;
    }

    return true;
}

static bool validKeyInfo(XmlDocument doc)
{
    // select KeyInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode keyInfoNode = doc.SelectSingleNode("//ds:KeyInfo", namespaceManager);

    if (keyInfoNode == null)
    {
        Console.WriteLine("XML súbor neobsahuje element ds:KeyInfo");
        return false;
    }

    if (keyInfoNode.Attributes["Id"] == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje atribút Id");
        return false;
    }

    XmlNode dataNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data", namespaceManager);
    if (dataNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509Data");
        return false;
    }

    XmlNode certificateNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager);
    if (certificateNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509Certificate");
        return false;
    }

    XmlNode issuerSerialNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509IssuerSerial", namespaceManager);
    if (issuerSerialNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509IssuerSerial");
        return false;
    }

    XmlNode subjectNameNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509SubjectName", namespaceManager);
    if (subjectNameNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509SubjectName");
        return false;
    }

    return true;
}

static bool validSignatureProperties(XmlDocument doc)
{
    // select SignatureProperties
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:SignatureProperties", namespaceManager);

    if (signaturePropertiesNode == null)
    {
        Console.WriteLine("XML súbor neobsahuje element ds:SignatureProperties");
        return false;
    }

    if (signaturePropertiesNode.Attributes["Id"] == null)
    {
        Console.WriteLine("Element ds:SignatureProperties neobsahuje atribút Id");
        return false;
    }

    XmlNodeList signaturePropertyNodes = doc.SelectNodes("//ds:SignatureProperties/ds:SignatureProperty", namespaceManager);
    if (signaturePropertyNodes == null || signaturePropertyNodes.Count != 2)
    {
        Console.WriteLine("Element ds:SignatureProperties neobsahuje práve 2 elementy ds:SignatureProperty");
        return false;
    }

    XmlNamespaceManager xzepNamespaceManager = new XmlNamespaceManager(doc.NameTable);
    xzepNamespaceManager.AddNamespace("zxep", "http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0");
    XmlNode signatureVersion = doc.SelectSingleNode("//ds:SignatureProperties/ds:SignatureProperty/xzep:SignatureVersion", xzepNamespaceManager);
    if (signatureVersion == null)
    {
        Console.WriteLine("Element ds:SignatureProperty neobsahuje element xzep:SignatureVersion");
        return false;
    }

    String signatureId = doc.SelectSingleNode("//ds:Signature", namespaceManager).Attributes["Id"].Value;
    XmlNode signatureVersionTarget = signatureVersion.Attributes["Target"];
    if (signatureVersionTarget == null || signatureVersionTarget.Value != signatureId)
    {
        Console.WriteLine("Atribút Target elementu xzep:SignatureVersion nie je zhodný s atribútom Id elementu ds:Signature");
        return false;
    }

    XmlNode productInfos = doc.SelectSingleNode("//ds:SignatureProperties/ds:SignatureProperty/xzep:ProductInfos", xzepNamespaceManager);
    if (productInfos == null)
    {
        Console.WriteLine("Element ds:SignatureProperty neobsahuje element xzep:ProductInfos");
        return false;
    }

    XmlNode productInfosTarget = productInfos.Attributes["Target"];
    if (productInfosTarget == null || productInfosTarget.Value != signatureId)
    {
        Console.WriteLine("Atribút Target elementu xzep:ProductInfos nie je zhodný s atribútom Id elementu ds:Signature");
        return false;
    }

    return true;
}

static bool validManifests(XmlDocument doc)
{
    // select KeyInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList manifests = doc.SelectNodes("//ds:Manifest", namespaceManager);
    if (manifests == null)
    {
        Console.WriteLine("XML súbor neobsahuje element ds:Manifest");
        return false;
    }

    foreach (XmlNode manifest in manifests)
    {
        XmlNode manifestId = manifest.Attributes["Id"];
        if (manifestId == null)
        {
            Console.WriteLine("Element ds:Manifest neobsahuje atribút Id");
            return false;
        }

        List<string> types = new List<string> { "http://www.w3.org/2000/09/xmldsig#Object",
                                                "http://www.w3.org/2000/09/xmldsig#SignatureProperties",
                                                "http://uri.etsi.org/01903#SignedProperties",
                                                "http://www.w3.org/2000/09/xmldsig#Manifest" };

        XmlNode referenceNode = manifest.SelectSingleNode("ds:Reference", namespaceManager);
        if (referenceNode == null)
        {
            Console.WriteLine("Element ds:Manifest neobsahuje element ds:Reference");
            return false;
        }

        XmlNode referenceType = referenceNode.Attributes["Type"];
        if (referenceType == null || !types.Contains(referenceType.Value))
        {
            Console.WriteLine("Element ds:Reference neobsahuje v atribúte Type podporovanú hodnotu");
            return false;
        }

        List<string> transformAlgorithms = new List<string> {   "http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                                                                "http://www.w3.org/2000/09/xmldsig#base64"};

        XmlNode transformNode = referenceNode.SelectSingleNode("//ds:Transform", namespaceManager);
        if (transformNode == null)
        {
            Console.WriteLine("XML súbor neobsahuje element ds:Transform");
            return false;
        }

        XmlNode transformAlgorithm = transformNode.Attributes["Algorithm"];
        if (transformAlgorithm == null || !transformAlgorithms.Contains(transformAlgorithm.Value))
        {
            Console.WriteLine("Element ds:Transform neobsahuje v atribúte Algorithm podporovanú hodnotu");
            return false;
        }

        List<string> digestAlgorithms = new List<string> {   "http://www.w3.org/2000/09/xmldsig#sha1",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha224",
                                                             "http://www.w3.org/2001/04/xmlenc#sha256",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha384",
                                                             "http://www.w3.org/2001/04/xmlenc#sha512"};

        XmlNode digestNode = referenceNode.SelectSingleNode("ds:DigestMethod", namespaceManager);
        if (digestNode == null)
        {
            Console.WriteLine("XML súbor neobsahuje element ds:DigestMethod");
            return false;
        }

        XmlNode digestAlgorithm = digestNode.Attributes["Algorithm"];
        if (digestAlgorithm == null || !digestAlgorithms.Contains(digestAlgorithm.Value))
        {
            Console.WriteLine("Element ds:DigestMethod neobsahuje v atribúte Algorithm podporovanú hodnotu");
            return false;
        }

    }

    return true;
}




//len kopia s upravami, nefunguje
static byte[] GetTimeStampSignatureCertificate( XmlDocument doc)
{
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("xades", "http://www.w3.org/2000/09/xmldsig#");

    string encapsulatedTimeStamp = "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";
//doc.SelectSingleNode("//xades:EncapsulatedTimeStamp", namespaceManager)?.InnerText;
    byte[] tsResponse = Convert.FromBase64String(encapsulatedTimeStamp);
    try
    {
        // Parse TimeStampResponse
        TimeStampResponse tsResp = new TimeStampResponse(tsResponse);

        // Parse TimeStampToken
        TimeStampToken token = new TimeStampToken(new CmsSignedData(tsResp.TimeStampToken.GetEncoded()));

        // Initialize variables
        Org.BouncyCastle.X509.X509Certificate signerCert = null;
        Org.BouncyCastle.X509.Store.IX509Store x509Certs = tsResp.TimeStampToken.GetCertificates("Collection");
        ArrayList certs = new ArrayList(x509Certs.GetMatches(null));

        // Find the signing certificate in the collection
        foreach (Org.BouncyCastle.X509.X509Certificate cert in certs)
        {
            string cerIssuerName = cert.IssuerDN.ToString(true, new Hashtable());
            string signerIssuerName = token.SignerID.Issuer.ToString(true, new Hashtable());

            // Check issuer name and serial number
            if (cerIssuerName == signerIssuerName &&
                cert.SerialNumber.Equals(token.SignerID.SerialNumber))
            {
                signerCert = cert;
                break;
            }
        }

        // Return the encoded signing certificate
        return signerCert?.GetEncoded();
    }
    catch (Exception ex)
    {
        // Handle exceptions and set error message
        //this.errorMessage = "GetTimeStampSignatureCertificate: " + ex.Message;
        return null;
    }
}

//zacal som ale nedokoncil
static bool ValidCore(XmlDocument doc)
{
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode sInfo = doc.SelectSingleNode("//ds:Signature/ds:SignedInfo", namespaceManager);
    XmlNode sValue = doc.SelectSingleNode("//ds:Signature/ds:SignatureValue", namespaceManager);
    if(sInfo == null || sValue == null)
    {
        return false;
    }
    return true;
}

//funguje ..asi
static TimeStampToken getTimestampToken(XmlDocument doc)
{

        TimeStampToken ts_token = null;

    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");


    //nsMap.put("xades", "http://uri.etsi.org/01903/v1.3.2#");
    var timestamp = doc.SelectSingleNode("//xades:EncapsulatedTimeStamp", namespaceManager);
    if(timestamp == null)
    {
        return null;
    }

    try
    {
        byte[] timestampBytes = Base64.Decode(timestamp.InnerText);
        CmsSignedData cmsSignedData = new CmsSignedData(timestampBytes);
        ts_token = new TimeStampToken(cmsSignedData);
    }
    catch
    {
        return null;
    }
    return ts_token;
}

//crl pre podpisovy certifikat:
//http://test.ditec.sk/DTCCACrl/DTCCACrl.crl

//crl pre certifikat casovej peciatky:
//http://test.ditec.sk/TSAServer/crl/dtctsa.crl
//neviem
/*static X509Crl GetCrl()
{

    var crlData = GetCrlData("http://test.ditec.sk/DTCCACrl/DTCCACrl.crl").ToArray();

    if (crlData == null)
    {
        return null;
    }
    try
    {
        CertificateFactory certFactory = CertificateFactory.getInstance("X.509", "BC");


        var crl = (X509CRL)certFactory.generateCRL(crlData);
    }
    catch
    {
        return null;
    }


    return crl;
}
//neviem
static X509Crl GetCrl2()
{
    var crlData = GetCrlData("http://test.monex.sk/DTCCACrl/DTCCACrl.crl");

    if (crlData == null)
    {
        return null;
    }

    X509Certificate2Collection collection = new X509Certificate2Collection();
    collection.Import(crlData);

    X509Crl crl = new X509Crl(collection[0]);

    return crl;
}*/

//podla mna to spravi co ma, ale nepaci sa mu type vystupu
static MemoryStream GetCrlData(string url)
{
    Uri urlHandler = null;
    try
    {
        urlHandler = new Uri(url);
    }
    catch (UriFormatException e)
    {
        Console.WriteLine($"Failed to create URL from {url}: {e.Message}");
        return null;
    }

    MemoryStream baos = new MemoryStream();
    using (WebClient webClient = new WebClient())
    {
        try
        {
            byte[] byteChunk = webClient.DownloadData(urlHandler);
            baos.Write(byteChunk, 0, byteChunk.Length);
        }
        catch (WebException e)
        {
            Console.WriteLine($"Failed while reading bytes from {urlHandler.AbsoluteUri}: {e.Message}");
            return null;
        }
    }

    return baos;
}




static void main()
{
    String rootPath = "../../../../Priklady";

    if (Directory.Exists(rootPath))
    {
        // Get all files in the directory
        string[] files = Directory.GetFiles(rootPath);

        // Iterate through the files
        foreach (string filePath in files)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            Console.WriteLine("Validácia súboru " + filePath);


            if (!validEnvelope(doc))
                continue;

            if (!validAlgorithms(doc))
                continue;

            if (!validTransform(doc))
                continue;

            if (!validSignatureID(doc))
                continue;

            if (!validSignatureValueID(doc))
                continue;

            if (!validReferences(doc))
                continue;

            if (!validKeyInfo(doc))
                continue;

            if (!validSignatureProperties(doc))
                continue;

            if (!validManifests(doc))
                continue;
        }
    }
}

main();