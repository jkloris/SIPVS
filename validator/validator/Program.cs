using System.Xml;
static bool validEnvelope(XmlDocument doc)
{

    XmlElement rootElement = doc.DocumentElement;

    // check if the root element has the required attributes
    if (rootElement != null &&
        rootElement.HasAttribute("xmlns:xzep") &&
        rootElement.HasAttribute("xmlns:ds"))
    {
        string xzepValue = rootElement.GetAttribute("xmlns:xzep");
        if (xzepValue == null || xzepValue != "http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0") return false;

        string dsValue = rootElement.GetAttribute("xmlns:ds");
        if (dsValue == null || dsValue != "http://www.w3.org/2000/09/xmldsig#") return false;
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
        string signatureMethodAlgorithm = signatureMethodNode.Attributes["Algorithm"].Value;
        string canonicalizationMethodAlgorithm = canonicalizationMethodNode.Attributes["Algorithm"].Value;

        if (signatureMethodAlgorithm == null || !signatureAlgorithms.Contains(signatureMethodAlgorithm)) return false;
        if (canonicalizationMethodAlgorithm == null || canonicalizationMethodAlgorithm != "http://www.w3.org/TR/2001/REC-xml-c14n-20010315") return false;
    }
    else
    {
        return false;
    }

    return true;
}

static bool validTransform(XmlDocument doc)
{
    // select ds:Transforms and ds:DigestMethod
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode transforms = doc.SelectSingleNode("//ds:Transforms", namespaceManager);
    XmlNode digestMethod = doc.SelectSingleNode("//ds:DigestMethod", namespaceManager);

    // Check if the elements are found
    if (transforms != null && digestMethod != null)
    {

        List<string> signatureAlgorithms = new List<string> {   "http://www.w3.org/2000/09/xmldsig#dsa-sha1",
                                                                "http://www.w3.org/2000/09/xmldsig#rsa-sha1",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384",
                                                                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512"};

        // Get the Algorithm attribute values
        string transformsAlgorithm = transforms.Attributes["Algorithm"].Value;
        string digestMethodAlgorithm = digestMethod.Attributes["Algorithm"].Value;

        if (transformsAlgorithm == null || !transforms.Contains(transformsAlgorithm)) return false;
        if (digestMethodAlgorithm == null || !digestMethod.Contains(digestMethodAlgorithm)) return false;
    }
    
    return false;
}

static bool validSignatureID(XmlDocument doc)
{
    // select ds:Signature
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signature = doc.SelectSingleNode("//ds:Signature", namespaceManager);
    
    // check if signature is valid
    if (signature != null && signature.Attributes["Id"]) {
        return true;
    }

    return false;
}

static bool validSignatureValueID(XmlDocument doc)
{
    // select ds:SignatureValue
    XmlNode signatureValue = doc.SelectSingleNode("//ds:SignatureValue");

    // check if signature value is valid
    if (signatureValue != null && signatureValue.Attributes["Id"])
    {
        return true;
    }

    return false;
}

static bool validSReferences(XmlDocument doc)
{
    //obtain ds:SignedInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signedInfo = doc.SelectSingleNode("//ds:SignedInfo", namespaceManager);

    if (signedInfo != null)
    {
        List<string> types = new List<string> { "http://www.w3.org/2000/09/xmldsig#Object",
                                                "http://www.w3.org/2000/09/xmldsig#SignatureProperties"
                                                "http://uri.etsi.org/01903#SignedProperties",
                                                "http://www.w3.org/2000/09/xmldsig#Manifest" }

        XmlNode keyInfo = signedInfo.SelectSingleNode("//ds:Reference");
        if (keyInfo != null) 
        {
            if (keyInfo.Attributes["Type"] != types[0])
            {
                return false;
            }

            int keyInfoId = keyInfo.Attributes["URI"];
            XmlNode keyInfoNode = doc.SelectSingleNode("//ds:KeyInfo");

            if (keyInfoNode.Attributes["Id"] != keyInfoId)
            {
                return false;
            }
        }

        XmlNode signatureProperties = signedInfo.SelectSingleNode("//ds:Reference");
        if (signatureProperties != null)
        {
            if (signatureProperties.Attributes["Type"] != types[1])
            {
                return false;
            }

            int signaturePropertiesId = signatureProperties.Attributes["URI"];
            XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:SignatureProperties");

            if (signaturePropertiesNode.Attributes["Id"] != signaturePropertiesId)
            {
                return false;
            }
        }

        XmlNode signedProperties = signedInfo.SelectSingleNode("//ds:Reference");
        if (signedProperties != null)
        {
            if (signedProperties.Attributes["Type"] != types[2])
            {
                return false;
            }

            int signedPropertiesId = signedProperties.Attributes["URI"];
            XmlNode signedPropertiesNode = doc.SelectSingleNode("//ds:SignedProperties");

            if (signedPropertiesNode.Attributes["Id"] != signedPropertiesId)
            {
                return false;
            }
        }

    }

    return true;
}

static void main()
{
    String rootPath = "C:\\Users\\marek\\source\\repos\\validator\\validator";

    XmlDocument doc = new XmlDocument();
    doc.Load(String.Concat(rootPath, "\\xmls\\01XadesT.xml"));

    if (!validEnvelope(doc)) Console.WriteLine("Envelope error");
    if (!validAlgorithms(doc)) Console.WriteLine("Algorithm error");
    if (!validTransform(doc)) Console.WriteLine("Signed info error");

    if (!validSignatureID(doc)) Console.Writeint("Signature ID error");
    if (!validSignatureValueID(doc)) Console.Writeint("Signature value ID error");
    if (!validReferences(doc)) Console.Writeint("References error");
}

main();