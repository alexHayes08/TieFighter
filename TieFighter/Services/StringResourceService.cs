using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace TieFighter.Services
{
    public class StringResourceService : IStringResourceService
    {
        #region Constructors

        #endregion

        #region Fields

        private string ConfigFileLocation = null;
        private string fileName = null;
        private string pathToFile = null;
        private ConcurrentDictionary<string, string> stringDictionary;
        public string ConfigPlaceholder = ""; // TODO: Replace this
        public const string RootElementName = "StringResources";
        public const string StringResourceElementName = "StringResource";
        public const string NameElementName = "Name";
        public const string IdElementName = "Id";
        public const string StringResourceSchemaPath = "C:\\Users\\alexc\\Source\\Repos\\TieFighter\\TieFighter\\wwwroot\\StringResources.xsd"; // FIXME

        #endregion

        #region Properties

        public bool IsInitialized
        {
            get
            {
                return stringDictionary != null;
            }
        }

        public string this[string id]
        {
            get
            {
                if (stringDictionary.ContainsKey(id))
                {
                    return stringDictionary[id];
                }
                else
                {
                    return id;
                }
            }
            set
            {
                stringDictionary[id] = value;
            }
        }

        #endregion

        #region Functions

        public bool TrySetConfigFileLocation(string fullPathToFile)
        {
            if (File.Exists(fullPathToFile))
            {
                try
                {
                    bool foundRootElement = false;
                    var xmlDocument = new XmlDocument();
                    XmlReader reader = null;
                    xmlDocument.PreserveWhitespace = true;
                    xmlDocument.Load(fullPathToFile);

                    var xmlReaderSettings = new XmlReaderSettings();

                    var schema = new XmlSchemaSet();
                    schema.Add("http://www.w3.org/2001/XMLSchema", StringResourceSchemaPath);

                    xmlReaderSettings.Schemas.Add(schema);
                    xmlReaderSettings.ValidationEventHandler += ValidationEventHandler;

                    xmlReaderSettings.ValidationFlags =
                        xmlReaderSettings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;

                    reader = XmlReader.Create(fullPathToFile, xmlReaderSettings);
                    xmlDocument.Load(reader);

                    // Load the xml document into the string dictionary
                    stringDictionary = new ConcurrentDictionary<string, string>();

                    var rootNode = xmlDocument.SelectSingleNode("/StringResources");
                    var stringResources = rootNode.ChildNodes;

                    foreach (var stringResource in stringResources)
                    {
                        var element = stringResource as XmlElement;
                        if (element != null)
                        {
                            var id = element.SelectSingleNode("Name").InnerText;
                            var name = element.SelectSingleNode("Id").InnerText;
                            stringDictionary[id] = name;
                        }
                    }

                    fileName = Path.GetFileName(fullPathToFile);
                    pathToFile = Path.GetFullPath(fullPathToFile);

                    return foundRootElement;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region EventHandlers

        private void ValidationEventHandler (object sender, ValidationEventArgs e)
        {

        }

        #endregion

        #endregion
    }
}
