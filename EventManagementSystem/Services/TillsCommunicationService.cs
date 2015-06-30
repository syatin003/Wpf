using EventManagementSystem.Models.TillDomainObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Services
{

    public class TillsCommunicationService
    {

        #region Methods

        private static void SendXmlData(string command, string tillName, string ipAddress)
        {
            try
            {
                 Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect using a timeout (5 seconds)
                IAsyncResult result = socket.BeginConnect(ipAddress, 5559, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(200, true);
                if (!success)
                {
                    // NOTE, MUST CLOSE THE SOCKET
                    socket.Close();
                    bool? dialogResult = null;
                    string confirmText = "Unable to sync item as Till " + "'" + tillName + "' " + "is not responding.\nPlease check and retry.";

                    RadWindow.Confirm(new DialogParameters()
                    {
                        Owner = Application.Current.MainWindow,
                        Content = confirmText,
                        Header = "Warning!",
                        OkButtonContent = "Retry",
                        Closed = (sender, args) => { dialogResult = args.DialogResult; }
                    });

                    if (dialogResult != null && dialogResult.Value)
                    {
                        SendXmlData(command, tillName, ipAddress);
                    }
                }
                else
                {
                    string requestedUrl = "http://" + ipAddress + ":5559/data.xml";
                    WebRequest request = WebRequest.Create(requestedUrl);
                    // Set the Method property of the request to POST.
                    request.Method = "POST";
                    Encoding encoder = new UTF8Encoding(true, true);
                    byte[] liveBytes = System.Text.Encoding.UTF8.GetBytes(command);
                    request.ContentType = "application/x-www-form-urlencoded";

                    // Set the 'ContentLength' property of the WebRequest.
                    request.ContentLength = liveBytes.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(liveBytes, 0, liveBytes.Length);

                    // Close the Stream object.
                    newStream.Close();

                    // Assign the response object of 'WebRequest' to a 'WebResponse' variable.
                    HttpWebResponse response;
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBox.Show("Some Error Occured.Please check it!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }

        public static void SetTillProduct(TillProductModel tillProductModel, List<String> tillNames, List<String> ipAddresses)
        {
            string generatedXml = CreateProductXml("1", tillProductModel);
            for (int i = 0; i < ipAddresses.Count; i++)
            {
                SendXmlData(generatedXml, tillNames[i], ipAddresses[i]);
            }
        }

        public static void SetProductGroup(ProductGroupModel productGroupModel, String tillName, string ipAddress)
        {
            string generatedXml = CreateGroupAndDepartmentXml("2", Convert.ToString(productGroupModel.Record), productGroupModel.Name);
            SendXmlData(generatedXml, tillName, ipAddress);
        }

        public static void SetProductDepartment(ProductDepartmentModel productDepartmentModel, String tillName, string ipAddress)
        {
            string generatedXml = CreateGroupAndDepartmentXml("55", Convert.ToString(productDepartmentModel.Record), productDepartmentModel.Name);
            SendXmlData(generatedXml, tillName, ipAddress);
        }

        private static string CreateGroupAndDepartmentXml(string fileNumber, string recordID, string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("SENDPROGRAMDATA");
            XmlAttribute rootNodeAttribute = doc.CreateAttribute("file");
            rootNode.Attributes.Append(rootNodeAttribute);
            rootNodeAttribute.Value = fileNumber;
            doc.AppendChild(rootNode);

            XmlNode dataNode = doc.CreateElement("DATA");
            rootNode.AppendChild(dataNode);

            XmlNode recordNode = doc.CreateElement("RECORD");
            recordNode.AppendChild(doc.CreateTextNode(recordID));
            dataNode.AppendChild(recordNode);

            XmlNode nameNode = doc.CreateElement("NAME");
            nameNode.AppendChild(doc.CreateTextNode(name));
            dataNode.AppendChild(nameNode);

            return (doc.InnerXml) + "\n";
        }

        private static string CreateProductXml(string fileNumber, TillProductModel tillProductModel)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("SENDPROGRAMDATA");
            XmlAttribute rootNodeAttribute = doc.CreateAttribute("file");
            rootNode.Attributes.Append(rootNodeAttribute);
            rootNodeAttribute.Value = fileNumber;
            doc.AppendChild(rootNode);

            XmlNode dataNode = doc.CreateElement("DATA");
            rootNode.AppendChild(dataNode);

            XmlNode recordNode = doc.CreateElement("RECORD");
            recordNode.AppendChild(doc.CreateTextNode(Convert.ToString(tillProductModel.Record)));
            dataNode.AppendChild(recordNode);

            XmlNode nameNode = doc.CreateElement("NAME");
            nameNode.AppendChild(doc.CreateTextNode(tillProductModel.Name));
            dataNode.AppendChild(nameNode);

            XmlNode priceNode = doc.CreateElement("PRICE1L1");
            priceNode.AppendChild(doc.CreateTextNode(Convert.ToString(tillProductModel.Price1L1)));
            dataNode.AppendChild(priceNode);

            XmlNode quantityNode = doc.CreateElement("QTY1");
            quantityNode.AppendChild(doc.CreateTextNode("1.00"));
            dataNode.AppendChild(quantityNode);

            XmlNode groupNode = doc.CreateElement("GROUP");
            groupNode.AppendChild(doc.CreateTextNode(Convert.ToString(tillProductModel.GroupRecord)));
            dataNode.AppendChild(groupNode);

            XmlNode deptNode = doc.CreateElement("DEPT");
            deptNode.AppendChild(doc.CreateTextNode(Convert.ToString(tillProductModel.DepartmentRecord)));
            dataNode.AppendChild(deptNode);

            XmlNode taxRateNode = doc.CreateElement("TAXRATE");
            taxRateNode.AppendChild(doc.CreateTextNode(Convert.ToString(tillProductModel.ProductRateRecord)));
            dataNode.AppendChild(taxRateNode);

            return (doc.InnerXml) + "\n";
        }

        #endregion Methods
    }
}
