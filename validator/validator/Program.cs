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

static void main()
{
    String rootPath = "C:\\Users\\marek\\source\\repos\\validator\\validator";

    XmlDocument doc = new XmlDocument();
    doc.Load(String.Concat(rootPath, "\\xmls\\01XadesT.xml"));

    if (!validEnvelope(doc)) Console.WriteLine("Envelope error");
    if (!validAlgorithms(doc)) Console.WriteLine("Algorithm error");
}

main();