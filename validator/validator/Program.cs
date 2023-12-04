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
using System.Security.Cryptography;
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

    XmlNode signatureMethodNode = doc.SelectSingleNode("//ds:Signature/ds:SignedInfo/ds:SignatureMethod", namespaceManager);
    XmlNode canonicalizationMethodNode = doc.SelectSingleNode("//ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod", namespaceManager);

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

    XmlNodeList transforms = doc.SelectNodes("//ds:Signature/ds:SignedInfo/ds:Reference/ds:Transforms/ds:Transform", namespaceManager);
    XmlNodeList digestMethods = doc.SelectNodes("//ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestMethod", namespaceManager);

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

    XmlNode signatureValue = doc.SelectSingleNode("//ds:Signature/ds:SignatureValue", namespaceManager);

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
                XmlNode keyInfoNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo", namespaceManager);

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

                if (keyInfoNodeId.Value != keyInfoId.Substring(1))
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
                XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:Signature/ds:Object/ds:SignatureProperties", namespaceManager);

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

                if (signaturePropertiesNodeId.Value != signaturePropertiesId.Substring(1))
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
                XmlNode signedPropertiesNode = reference.SelectSingleNode("//xades:SignedProperties", xadesNamespaceManager);

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

                if (signedPropertiesNodeId.Value != signedPropertiesId.Substring(1))
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
                XmlNodeList manifestNodes = doc.SelectNodes("//ds:Signature/ds:Object/ds:Manifest", namespaceManager);

                if (manifestNodes == null)
                {
                    Console.WriteLine("XML súbor neobsahuje element ds:Manifest");
                    return false;
                }
                Boolean foundManifest = false;

                foreach (XmlNode manifestNode in manifestNodes)
                {
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

                    if (manifestNodeId.Value == manifestId.Substring(1))
                    {
                        foundManifest = true;
                        break;
                    }
                }

                if (!foundManifest)
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

    XmlNode keyInfoNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo", namespaceManager);

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

    XmlNode dataNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo/ds:X509Data", namespaceManager);
    if (dataNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509Data");
        return false;
    }

    XmlNode certificateNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager);
    if (certificateNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509Certificate");
        return false;
    }

    XmlNode issuerSerialNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509IssuerSerial", namespaceManager);
    if (issuerSerialNode == null)
    {
        Console.WriteLine("Element ds:KeyInfo neobsahuje element ds:X509IssuerSerial");
        return false;
    }

    XmlNode subjectNameNode = doc.SelectSingleNode("//ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509SubjectName", namespaceManager);
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

    XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:Signature/ds:Object/ds:SignatureProperties", namespaceManager);

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

    XmlNodeList signaturePropertyNodes = doc.SelectNodes("//ds:Signature/ds:Object/ds:SignatureProperties/ds:SignatureProperty", namespaceManager);
    if (signaturePropertyNodes == null || signaturePropertyNodes.Count != 2)
    {
        Console.WriteLine("Element ds:SignatureProperties neobsahuje práve 2 elementy ds:SignatureProperty");
        return false;
    }

    foreach (XmlNode signaturePropertyNode in signaturePropertyNodes)
    {
        XmlNamespaceManager xzepNamespaceManager = new XmlNamespaceManager(doc.NameTable);
        xzepNamespaceManager.AddNamespace("xzep", "http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0");
        XmlNode signatureVersion = doc.SelectSingleNode("//xzep:SignatureVersion", xzepNamespaceManager);
        if (signatureVersion == null)
        {
            Console.WriteLine("Element ds:SignatureProperty neobsahuje element xzep:SignatureVersion");
            return false;
        }

        XmlNode productInfos = doc.SelectSingleNode("//xzep:ProductInfos", xzepNamespaceManager);
        if (productInfos == null)
        {
            Console.WriteLine("Element ds:SignatureProperty neobsahuje element xzep:ProductInfos");
            return false;
        }

        XmlNode signaturePropertyTarget = signaturePropertyNode.Attributes["Target"];
        String signatureId = doc.SelectSingleNode("//ds:Signature", namespaceManager).Attributes["Id"].Value;
        if (signaturePropertyTarget == null || signaturePropertyTarget.Value.Substring(1) != signatureId)
        {
            Console.WriteLine("Atribút Target elementu ds:SignatureProperty nie je zhodný s atribútom Id elementu ds:Signature");
            return false;
        }
    }

    return true;
}

static bool validManifests(XmlDocument doc)
{
    // select manifests
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList manifests = doc.SelectNodes("//ds:Signature/ds:Object/ds:Manifest", namespaceManager);
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

        XmlNodeList referenceNodes = manifest.SelectNodes("ds:Reference", namespaceManager);

        if (referenceNodes == null)
        {
            Console.WriteLine("Element ds:Manifest neobsahuje element ds:Reference");
            return false;
        }

        if (referenceNodes.Count != 1)
        {
            Console.WriteLine("Element ds:Manifest obsahuje viac ako jeden element ds:Reference");
            return false;
        }
        XmlNode referenceNode = referenceNodes[0];

        XmlNode referenceType = referenceNode.Attributes["Type"];
        if (referenceType == null || !types.Contains(referenceType.Value))
        {
            Console.WriteLine("Element ds:Reference neobsahuje v atribúte Type podporovanú hodnotu");
            return false;
        }

        List<string> transformAlgorithms = new List<string> {   "http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                                                                "http://www.w3.org/2000/09/xmldsig#base64"};

        XmlNode transformNode = referenceNode.SelectSingleNode("ds:Transforms/ds:Transform", namespaceManager);
        if (transformNode == null)
        {
            Console.WriteLine("Element ds:Reference neobsahuje element ds:Transform");
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
            Console.WriteLine("Element ds:Reference neobsahuje element ds:DigestMethod");
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

static bool validManifestReferences(XmlDocument doc)
{
    // select references
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList manifests = doc.SelectNodes("//ds:Signature/ds:Object/ds:Manifest", namespaceManager);

    foreach (XmlNode manifest in manifests)
    {
        XmlNode reference = manifest.SelectSingleNode("/ds:Reference");
    }

    return true;
}


//len kopia s upravami, nefunguje
static Org.BouncyCastle.X509.X509Certificate getTimeStampSignatureCertificate( XmlDocument doc)
{
    Org.BouncyCastle.X509.X509Certificate signerCert = null;

    try
    {
        // Parse TimeStampToken
        TimeStampToken token = getTimestampToken(doc);

        Org.BouncyCastle.X509.Store.IX509Store x509Certs = token.GetCertificates("Collection");
        ArrayList certs = new ArrayList(x509Certs.GetMatches(null));

        // nájdenie podpisového certifikátu tokenu v kolekcii
        foreach (Org.BouncyCastle.X509.X509Certificate cert in certs)
        {
            string cerIssuerName = cert.IssuerDN.ToString(true, new Hashtable());
            string signerIssuerName = token.SignerID.Issuer.ToString(true, new Hashtable());

            // kontrola issuer name a seriového čísla
            if (cerIssuerName == signerIssuerName && cert.SerialNumber.Equals(token.SignerID.SerialNumber))
            {
                signerCert = cert;
                break;
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions and set error message
        //this.errorMessage = "GetTimeStampSignatureCertificate: " + ex.Message;
        return null;
    }

    return signerCert;
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


//kanonikalizácia ds:SignedInfo a overenie hodnoty ds:SignatureValue pomocou pripojeného podpisového certifikátu v ds:KeyInfo
static bool coreValidationSignedInfo(XmlDocument doc)
{
    
    var namespaceId = new XmlNamespaceManager(doc.NameTable);
    namespaceId.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
    namespaceId.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");

    
    XmlNode checkData = doc.SelectSingleNode(@"//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceId);
    if (checkData == null)
        return true;

    byte[] certificateData = Convert.FromBase64String(doc.SelectSingleNode(@"//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceId).InnerText);
    byte[] signature = Convert.FromBase64String(doc.SelectSingleNode(@"//ds:SignatureValue", namespaceId).InnerText);
    XmlNode signedInfoNnn = doc.SelectSingleNode(@"//ds:SignedInfo", namespaceId);
    string digestAlg = doc.SelectSingleNode(@"//ds:SignedInfo/ds:SignatureMethod", namespaceId).Attributes.GetNamedItem("Algorithm").Value;
    
    //kanonikalizátor
    XmlDsigC14NTransform transformation = new XmlDsigC14NTransform(false);
    XmlDocument pom = new XmlDocument();
    pom.LoadXml(signedInfoNnn.OuterXml);
    //kanonikalizácia ds:SignedInfo
    transformation.LoadInput(pom);
    byte[] data = ((MemoryStream)transformation.GetOutput()).ToArray();
    
    try
    {
        SubjectPublicKeyInfo ski = X509CertificateStructure.GetInstance(Org.BouncyCastle.Asn1.Asn1Object.FromByteArray(certificateData)).SubjectPublicKeyInfo;
       AsymmetricKeyParameter pk = Org.BouncyCastle.Security.PublicKeyFactory.CreateKey(ski);

        string algStr = ""; //signature alg

        //find digest
        switch (digestAlg)
        {
            case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
                algStr = "sha1";
                break;
            case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
                algStr = "sha256";
                break;
            case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384":
                algStr = "sha384";
                break;
            case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512":
                algStr = "sha512";
                break;
        }

        //find encryption
        switch (ski.AlgorithmID.ObjectID.Id)
        {
            case "1.2.840.10040.4.1": //dsa
                algStr += "withdsa";
                break;
            case "1.2.840.113549.1.1.1": //rsa
                algStr += "withrsa";
                break;
            default:
                Console.WriteLine("Verejný kľúč uložený v ds:X509Certificate nemá podporovaný formát.");
                return false;
        }
        
        //overenie hodnoty
        ISigner verif = Org.BouncyCastle.Security.SignerUtilities.GetSigner(algStr);
        verif.Init(false, pk);
        verif.BlockUpdate(data, 0, data.Length);
        bool res = verif.VerifySignature(signature);

        if (!res)
        {
            Console.WriteLine("Core validation zlyhala. Nepodarilo sa nájsť zhodu medzi ds:SignedInfo a ds:SignatureValue pomocou pripojeného podpisového certifikátu v ds:KeyInfo");
        }

        return res;

    }
    catch (Exception ex)
    {
        Console.WriteLine("Nepodarilo sa získať pripojený podpisový certifikát.");
        return false;
    }
}

static bool coreValidationDigestValue(XmlDocument doc) 
{
    
    var namespaceId = new XmlNamespaceManager(doc.NameTable);
    namespaceId.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
    namespaceId.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");
    
    //dereferencovanie URI, kanonikalizácia referencovaných ds:Manifest elementov a overenie hodnôt odtlačkov ds:DigestValue,
    XmlNode signedInfoN = doc.SelectSingleNode(@"//ds:SignedInfo", namespaceId);
    XmlNodeList referenceElements = signedInfoN.SelectNodes(@"//ds:Reference", namespaceId);
    
    foreach (XmlNode reference in referenceElements)
    {
        String referenceURI = reference.Attributes.GetNamedItem("URI").Value;
        referenceURI = referenceURI.Substring(1);
        XmlNode digestMethod = reference.SelectSingleNode("ds:DigestMethod", namespaceId);
        String digestMethodAlgorithm = digestMethod.Attributes.GetNamedItem("Algorithm").Value;
        string dsDigestValue = reference.SelectSingleNode("ds:DigestValue", namespaceId).InnerText;
        
        if (referenceURI.StartsWith("ManifestObject"))
        {
            //dereferencovanie URI
            string manifestXML = doc.SelectSingleNode("//ds:Manifest[@Id='" + referenceURI + "']", namespaceId).OuterXml;
            MemoryStream sManifest = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(manifestXML));
            
            //kankonikalizácia referencovaných ds:Manifest elementov
            XmlDsigC14NTransform transformation = new XmlDsigC14NTransform();
            transformation.LoadInput(sManifest);
            
            HashAlgorithm hash = null;

            switch (digestMethodAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#sha512":
                    hash = new SHA512Managed();
                    break;
                case "http://www.w3.org/2000/09/xmldsig#sha1":
                    hash = new SHA1Managed();
                    break;
                case "http://www.w3.org/2001/04/xmlenc#sha256":
                    hash = new SHA256Managed();
                    break;
                case "http://www.w3.org/2001/04/xmldsig-more#sha384":
                    hash = new SHA384Managed();
                    break;
                
            }

            if (hash == null)
            {
                Console.WriteLine("Element ds:DigestMethod neobsahuje podporovaný algoritmus.");
                return false;
            }

            byte[] digest = transformation.GetDigestedOutput(hash);
            string result = Convert.ToBase64String(digest);

            //overenie hodnôt odtlačkov ds:DigestValue
            if (!result.Equals(dsDigestValue))
            {
                Console.WriteLine("Core validation zlyhala. Hodnota ds:DigestValue a hash hodnota elementu ds:Manifest sú rozdielne.");
                return false;
            }
            
        }
    }
    
    return true;
}

static X509Crl getSignCert(string url)
{
    try
    {
        using (System.Net.WebClient client = new System.Net.WebClient())
        {
            // Stiahnite CRL zo zadaného URL
            byte[] crlBytes = client.DownloadData(url);
            if (crlBytes != null)
            {
                X509CrlParser crlParser = new Org.BouncyCastle.X509.X509CrlParser();
                X509Crl crl = crlParser.ReadCrl(new MemoryStream(crlBytes));
                return crl;
            }
            else
            {
                Console.WriteLine("Nepodarilo sa stiahnuť CRL.");
                return null;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
}

static bool validSignCert(XmlDocument doc)
{
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
    namespaceManager.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");

    TimeStampToken token = getTimestampToken(doc);
    // Console.WriteLine(doc.SelectSingleNode(@"//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager).InnerText);

    // get signature cert
    byte[] signatureCertificate = Convert.FromBase64String(doc.SelectSingleNode(@"//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager).InnerText);
    X509CertificateParser x509parser = new X509CertificateParser();
    Org.BouncyCastle.X509.X509Certificate x509cert = x509parser.ReadCertificate(signatureCertificate);

    try
    {
        Org.BouncyCastle.X509.X509Certificate signerCert = getTimeStampSignatureCertificate(doc);

        //check x509 certtificate and GenTime
        int compare1 = DateTime.Compare(x509cert.NotAfter, token.TimeStampInfo.TstInfo.GenTime.ToDateTime());
        int compare2 = DateTime.Compare(token.TimeStampInfo.TstInfo.GenTime.ToDateTime(), x509cert.NotBefore);


        if (compare1 < 0 || compare2 < 0)
        {
            Console.WriteLine("Neplatný certifikát v čase podpisovania.");
            return false;
        }

        X509Crl crl = getSignCert("http://test.ditec.sk/DTCCACrl/DTCCACrl.crl");
        if (crl.IsRevoked(x509cert))
        {
            Console.WriteLine("Neplatný podpis z dôvodu predčasného zrušenia certifikátu.");
            return false;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("error");
        return false;
    }
    return true;
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
            Console.WriteLine("\nValidácia súboru " + filePath);

            if (!validEnvelope(doc))
                continue;

            if (!validAlgorithms(doc))
                continue;

            if (!validTransform(doc))
                continue;
            
            if (!coreValidationDigestValue(doc))
                continue;
            
            if (!coreValidationSignedInfo(doc))
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

            //if (!validManifestReferences(doc))
            //    continue;

            if (!validSignCert(doc))
                continue;
        }
    }
}

main();