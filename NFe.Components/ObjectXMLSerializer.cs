namespace NFe.Components
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;	//For serialization of an object to an XML file.
    using System.IO;				//For reading/writing data to an XML file.
    using System.ComponentModel;	//For error messsages.
    using System.Text;

    /// <summary>
    /// Custom class used as a wrapper to the XML serialization of an object to/from an XML file.
    /// See method calls 'Load' and 'Save' for usage.
    /// 
    /// References: XML Serialization at http://samples.gotdotnet.com/:
    /// http://samples.gotdotnet.com/QuickStart/howto/default.aspx?url=/quickstart/howto/doc/xmlserialization/rwobjfromxml.aspx
    /// </summary>
    public class ObjectXMLSerializer
    {
        /// <summary>
        /// Load an object from an XML file.
        /// <newpara></newpara>
        /// <example>
        /// The following example loads serialized data into a 'Test' class object, from the XML file 'Objects as XML.xml':
        /// <newpara></newpara>
        /// <code>
        /// Test objTest = new Test(); //Must use new to create the object - cannot set to null.
        /// ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
        /// objObjectXMLSerializer.Load(objTest, "Objects as XML.xml");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="objObjectToLoad">Object to be loaded.</param>
        /// <param name="strXMLFilePathName">File Path name of the XML file containing object(s) serialized to XML.</param>
        /// <returns>Returns an Object loaded from the XML file. If the Object could not be loaded returns null.</returns>
        public object Load(Type typObjectType, string strXMLFilePathName)
        {
            if (!System.IO.File.Exists(strXMLFilePathName))
                throw new Exception("Arquivo '" + strXMLFilePathName + "' não encontrado para deserialização");

            using (TextReader txrTextReader = new StreamReader(strXMLFilePathName, Encoding.UTF8))
            {
                try
                {
                    XmlSerializer xserSerializer = new XmlSerializer(typObjectType);

                    return xserSerializer.Deserialize(txrTextReader);
                }
                catch (Win32Exception exException)
                {
                    throw exException;
                }
            }
        }

        /// <summary>
        /// Save an object to an XML file.
        /// <newpara></newpara>
        /// <example>
        /// The following example saves a 'Test' class object to the XML file 'Objects as XML.xml':
        /// <newpara></newpara>
        /// <code>
        /// Test objTest = new Test();  //Must use new to create the object - cannot set to null.
        /// ObjectXMLSerializer objObjectXMLSerializer = new ObjectXMLSerializer();
        /// bool blnSuccess = objObjectXMLSerializer.Save(objTest, "Objects as XML.xml");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="objObjectToSave">Object to be saved.</param>
        /// <param name="strXMLFilePathName">File Path name of the XML file to contain the object serialized to XML.</param>
        /// <returns>Returns success of the object save.</returns>
        public void Save(Object objObjectToSave, string strXMLFilePathName)
        {
            TextWriter txwTextWriter = null;

            try
            {
                Type typObjectType = objObjectToSave.GetType();

                //Create serializer object using the type name of the Object to serialize.
                XmlSerializer xserSerializer = new XmlSerializer(typObjectType);

                //Create a TextWriter object to write the object-converted-XML to file.
                txwTextWriter = new StreamWriter(strXMLFilePathName, false, Encoding.UTF8);

                xserSerializer.Serialize(txwTextWriter, objObjectToSave);
            }
            catch (Win32Exception exException)
            {
                throw exException;
                //MessageBox.Show(exException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Make sure to close the file even if an exception is raised...
                if (txwTextWriter != null)
                    txwTextWriter.Close();
            }
        }
    }
}
