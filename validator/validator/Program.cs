using System.Security.Cryptography.X509Certificates;
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
        XmlNode signatureMethodAlgorithmNode = signatureMethodNode.Attributes["Algorithm"];
        if (signatureMethodAlgorithmNode != null)
        {
            String signatureMethodAlgorithm = signatureMethodAlgorithmNode.Value;
            if (signatureMethodAlgorithm == null || !signatureAlgorithms.Contains(signatureMethodAlgorithm))
                return false;

        }
        else
            return false;

        XmlNode canonicalizationMethodAlgorithmNode = canonicalizationMethodNode.Attributes["Algorithm"];
        if (canonicalizationMethodAlgorithmNode != null)
        {
            String canonicalizationMethodAlgorithm = canonicalizationMethodAlgorithmNode.Value;

            if (canonicalizationMethodAlgorithm == null || canonicalizationMethodAlgorithm != "http://www.w3.org/TR/2001/REC-xml-c14n-20010315")
                return false;
        }
        else
            return false;
    }
    else
        return false;

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
                    return false;
            }
            else
                return false;
        }

        foreach (XmlNode digestMethod in digestMethods)
        {
            XmlAttribute digestMethodAlgorithmNode = digestMethod.Attributes["Algorithm"];
            if (digestMethodAlgorithmNode != null)
            {
                String digestMethodAlgorithm = digestMethodAlgorithmNode.Value;
                if (digestMethodAlgorithm == null || !digestAlgorithms.Contains(digestMethodAlgorithm))
                    return false;
            }
            else
                return false;
        }
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
    if (signature != null && signature.Attributes["Id"] != null) {
        return true;
    }

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
    {
        return true;
    }

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
                return false;

            // check reference to KeyInfor
            if (referenceType.Value == types[0])
            {
                String keyInfoId = reference.Attributes["URI"].Value;
                XmlNode keyInfoNode = doc.SelectSingleNode("//ds:KeyInfo", namespaceManager);

                if (keyInfoNode == null)
                    return false;

                XmlNode keyInfoNodeId = keyInfoNode.Attributes["Id"];
                if (keyInfoNodeId == null)
                    return false;

                if (keyInfoNodeId.Value != keyInfoId)
                    return false;

                types[0] = "";
            }

            //check reference to SignatureProperties
            else if (referenceType.Value == types[1])
            {
                String signaturePropertiesId = reference.Attributes["URI"].Value;
                XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:SignatureProperties", namespaceManager);

                if (signaturePropertiesNode == null)
                    return false;

                XmlNode signaturePropertiesNodeId = signaturePropertiesNode.Attributes["Id"];
                if (signaturePropertiesNodeId == null)
                    return false;

                if (signaturePropertiesNodeId.Value != signaturePropertiesId)
                    return false;

                types[1] = "";
            }

            //check reference to SignedProperties
            else if (referenceType.Value == types[2])
            {
                String signedPropertiesId = reference.Attributes["URI"].Value;

                XmlNamespaceManager xadesNamespaceManager = new XmlNamespaceManager(doc.NameTable);
                xadesNamespaceManager.AddNamespace("xades", "http://www.w3.org/2000/09/xmldsig#");
                XmlNode signedPropertiesNode = reference.SelectSingleNode("//xades:SignedProperties", xadesNamespaceManager);

                if (signedPropertiesNode == null)
                    return false;

                XmlNode signedPropertiesNodeId = signedPropertiesNode.Attributes["Id"];
                if (signedPropertiesNodeId == null)
                    return false;

                if (signedPropertiesNodeId.Value != signedPropertiesId)
                    return false;

                types[2] = "";
            }

            //check reference to manifest
            else if (referenceType.Value == types[3])
            {
                String manifestId = reference.Attributes["URI"].Value;
                XmlNode manifestNode = doc.SelectSingleNode("//ds:Manifest", namespaceManager);

                if (manifestNode == null)
                    return false;

                XmlNode manifestNodeId = manifestNode.Attributes["Id"];
                if (manifestNodeId == null)
                    return false;

                if (manifestNodeId.Value != manifestId)
                    return false;
            }

            else
                return false;
        }

    }

    if (types[0] != "" || types[1] != "" || types[2] != "")
        return false;

    return true;
}

static bool validKeyInfo(XmlDocument doc)
{
    // select KeyInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode keyInfoNode = doc.SelectSingleNode("//ds:KeyInfo", namespaceManager);

    if (keyInfoNode == null)
        return false;

    if (keyInfoNode.Attributes["Id"] == null)
        return false;

    XmlNode dataNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data", namespaceManager);
    if (dataNode == null)
        return false;

    XmlNode certificateNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager);
    if (certificateNode == null)
        return false;

    XmlNode issuerSerialNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509IssuerSerial", namespaceManager);
    if (issuerSerialNode == null)
        return false;

    XmlNode subjectNameNode = doc.SelectSingleNode("//ds:KeyInfo/ds:X509Data/ds:X509SubjectName", namespaceManager);
    if (subjectNameNode == null)
        return false;
    
    return true;
}

static bool validSignatureProperties(XmlDocument doc)
{
    // select SignatureProperties
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNode signaturePropertiesNode = doc.SelectSingleNode("//ds:SignatureProperties", namespaceManager);

    if (signaturePropertiesNode == null)
        return false;

    if (signaturePropertiesNode.Attributes["Id"] == null)
        return false;

    XmlNodeList signaturePropertyNodes = doc.SelectNodes("//ds:SignatureProperties/ds:SignatureProperty", namespaceManager);
    if (signaturePropertyNodes == null || signaturePropertyNodes.Count != 2)
        return false;

    XmlNode signatureVersion = doc.SelectSingleNode("//ds:SignatureProperties/ds:SignatureProperty/xzep:SignatureVersion", namespaceManager);
    if (signatureVersion == null)
        return false;

    String signatureId = doc.SelectSingleNode("//ds:Signature", namespaceManager).Attributes["Id"].Value;
    XmlNode signatureVersionTarget = signatureVersion.Attributes["Target"];
    if (signatureVersionTarget == null || signatureVersionTarget.Value != signatureId)
        return false;

    XmlNode productInfos = doc.SelectSingleNode("//ds:SignatureProperties/ds:SignatureProperty/xzep:ProductInfos", namespaceManager);
    if (productInfos == null)
        return false;

    XmlNode productInfosTarget = productInfos.Attributes["Target"];
    if (productInfosTarget == null || productInfosTarget.Value != signatureId)
        return false;

    return true;
}

static bool validManifests(XmlDocument doc)
{
    // select KeyInfo
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
    namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

    XmlNodeList manifests = doc.SelectNodes("//ds:Manifest", namespaceManager);
    if (manifests == null)
        return false;

    foreach (XmlNode manifest in manifests)
    {
        XmlNode manifestId = manifest.Attributes["Id"];
        if (manifestId == null)
            return false;

        List<string> types = new List<string> { "http://www.w3.org/2000/09/xmldsig#Object",
                                                "http://www.w3.org/2000/09/xmldsig#SignatureProperties",
                                                "http://uri.etsi.org/01903#SignedProperties",
                                                "http://www.w3.org/2000/09/xmldsig#Manifest" };

        XmlNode referenceNode = manifest.SelectSingleNode("ds:Reference", namespaceManager);
        if (referenceNode == null)
            return false;

        XmlNode referenceType = referenceNode.Attributes["Type"];
        if (referenceType == null || !types.Contains(referenceType.Value))
            return false;

        List<string> transformAlgorithms = new List<string> {   "http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                                                                "http://www.w3.org/2000/09/xmldsig#base64"};

        XmlNode transformNode = referenceNode.SelectSingleNode("//ds:Transform", namespaceManager);
        if (transformNode == null)
            return false;

        XmlNode transformAlgorithm = transformNode.Attributes["Algorithm"];
        if (transformAlgorithm == null || !transformAlgorithms.Contains(transformAlgorithm.Value))
            return false;

        List<string> digestAlgorithms = new List<string> {   "http://www.w3.org/2000/09/xmldsig#sha1",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha224",
                                                             "http://www.w3.org/2001/04/xmlenc#sha256",
                                                             "http://www.w3.org/2001/04/xmldsig-more#sha384",
                                                             "http://www.w3.org/2001/04/xmlenc#sha512"};

        XmlNode digestNode = referenceNode.SelectSingleNode("ds:DigestMethod", namespaceManager);
        if (digestNode == null)
            return false;

        XmlNode digestAlgorithm = digestNode.Attributes["Algorithm"];
        if (digestAlgorithm == null || !digestAlgorithms.Contains(digestAlgorithm.Value))
            return false;     

    }

    return true;
}

static void main()
{
    String rootPath = "../../../../Priklady";

    XmlDocument doc = new XmlDocument();
    doc.Load(String.Concat(rootPath, "/09XadesT.xml"));

    if (!validEnvelope(doc))
    {
        Console.WriteLine("Envelope error");
        return;
    }

    if (!validAlgorithms(doc))
    {
        Console.WriteLine("Algorithm error");
        return;
    }

    if (!validTransform(doc))
    {
        Console.WriteLine("Signed info error");
        return;
    }

    if (!validSignatureID(doc))
    {
        Console.WriteLine("Signature ID error");
        return;
    }

    if (!validSignatureValueID(doc))
    {
        Console.WriteLine("Signature value ID error");
        return;
    }

    //if (!validReferences(doc))
    //{
    //    Console.WriteLine("References error");
    //    return;
    //}

    if (!validKeyInfo(doc))
    {
        Console.WriteLine("Key info error");
        return;
    }

    //if (!validSignatureProperties(doc))
    //{
    //    Console.WriteLine("Signature properties error");
    //    return;
    //}

    if (!validManifests(doc))
    {
        Console.WriteLine("Manifest error");
        return;
    }
}

main();