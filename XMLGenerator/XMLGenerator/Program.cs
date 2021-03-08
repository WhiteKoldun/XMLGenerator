using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using XMLGenerator;
using SearchOption = System.IO.SearchOption;

namespace XMLGenerator
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Enter a path:");
            string path = "D:\\XMLGenerator";
            //string path = Console.ReadLine();
            Console.WriteLine("Choooooose action:");
            Console.WriteLine("1 - est kod FOTON, i on est v NSI, i est SPED");
            Console.WriteLine("2 - est kod FOTON, i on est v NSI, NO SPEDA NETU");
            Console.WriteLine("3 - stavim kod LOH");
            Console.WriteLine("4 - stavim kod FOTON, a ego net v NSI");
            Console.WriteLine("5 - no-thi-ng");
            int action = 4;
            //int action = Console.ReadLine();
            await General_cycle(path, action);
            var awaite = Console.ReadLine();
        }

        private static async Task General_cycle(string path, int action)
        {
            string inp = "Input";
            string outp = "Output";
            string pathin = Path.Combine(path, inp);
            string pathout = Path.Combine(path, outp);
            var listOfFiles = await GetListOfXMLFiles(pathin);
            foreach (var listoffile in listOfFiles)
            {
                var pathfile = Path.Combine(pathin, listoffile);
                var bytes = File.ReadAllBytes(pathfile);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(pathfile);
                DeleteExistAuthPerson(xmldoc,listoffile);
                switch (action)
                {
                    case 1:
                        Foton_exist_sped_exist(xmldoc);
                        break;
                    case 2:
                        Foton_exist_sped_not(xmldoc);
                        break;
                    case 3:
                        Insert_not_foton_or_sped(xmldoc);
                        break;
                    case 4:
                        Insert_invalid_sped_person(xmldoc);
                        break;
                    case 5:
                        break;
                }
                xmldoc.Save(listoffile.Replace("\\Input\\", "\\Output\\"));
            }
        }

        private static void Foton_exist_sped_exist(XmlDocument xmldoc)
        {
            AddPersonToXML(xmldoc, "FOTON", "1488");
        }

        private static void Foton_exist_sped_not(XmlDocument xmldoc)
        {
            AddPersonToXML(xmldoc, "FOTON", "1488");
        }

        private static void Insert_not_foton_or_sped(XmlDocument xmldoc)
        {
            AddPersonToXML(xmldoc, "LALKA", "1488");
        }
        private static void Insert_invalid_sped_person(XmlDocument xmldoc)
        {
            // указываем, в какую ноду надо добавить созданные ноды
            AddPersonToXML(xmldoc, "FOTON", "1488");
        }

        private static void DeleteExistAuthPerson(XmlDocument xmldoc, string listoffile)
        {
            XmlNode parentNode = xmldoc[xmldoc.DocumentElement.Name];
                string parentNodeStr = parentNode.Name;
                string xpath = parentNodeStr + "/autorised_person";
                XmlNode remNode = xmldoc.SelectSingleNode("//autorised_person");
                if (remNode == null)
                {
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine(listoffile);
                Console.WriteLine("не получилось убрать дите");
                Console.WriteLine("--------------------------------------------------------");
                }
                else
                {
                remNode.ParentNode.RemoveChild(remNode);
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine(listoffile);
                Console.WriteLine("дитё убрано");
                Console.WriteLine("--------------------------------------------------------");
            }
        }

        

        private static void AddPersonToXML(XmlDocument xmldoc, string organizationStr, string idStr)
        {
            XmlNode parentNode = xmldoc[xmldoc.DocumentElement.Name];
            XmlNode autorised_personNode = xmldoc.CreateNode(XmlNodeType.Element, "autorised_person", "");
            XmlNode autorised_person_dtlsNode = xmldoc.CreateNode(XmlNodeType.Element, "autorised_person_dtls", "");
            XmlNode autorised_person_typeNode = xmldoc.CreateNode(XmlNodeType.Element, "authorised_person_type", "");
            autorised_person_typeNode.InnerText = "12";
            XmlNode bases_of_powersNode = xmldoc.CreateNode(XmlNodeType.Element, "bases_of_powers", "");
            bases_of_powersNode.InnerText = "базе пуверс тicт";
            XmlElement anchorElement = parentNode["account_holder"];
            XmlNode indivdiual_or_entityNode = xmldoc.CreateNode(XmlNodeType.Element, "indivdiual_or_entity", "");
            indivdiual_or_entityNode.InnerText = "LEGL";
            parentNode.InsertAfter(autorised_personNode, anchorElement);
            XmlNode parentNode2 = parentNode.SelectSingleNode("autorised_person");
            parentNode2.AppendChild(autorised_person_dtlsNode);
            parentNode2.AppendChild(autorised_person_typeNode);
            parentNode2.AppendChild(bases_of_powersNode);
            XmlNode parentNode3 = parentNode2.SelectSingleNode("autorised_person_dtls");
            // создаем ноды, которые надо добавить
            XmlNode newNodeId1 = xmldoc.CreateNode(XmlNodeType.Element, "id", "");
            newNodeId1.InnerText = idStr;
            XmlNode newNodeOrganization1 = xmldoc.CreateNode(XmlNodeType.Element, "organization", "");
            newNodeOrganization1.InnerText = organizationStr;
            /*
            XmlNode newNodeId2 = xmldoc.CreateNode(XmlNodeType.Element, "id", "");
            newNodeId2.InnerText = "228";
            XmlNode newNodeOrganization2 = xmldoc.CreateNode(XmlNodeType.Element, "organization", "");
            newNodeOrganization2.InnerText = "SPED";
            //*/
            //создаем два варианта party_id
            XmlNode party_idNode1 = xmldoc.CreateNode(XmlNodeType.Element, "party_id", "");
            XmlNode party_idNode2 = xmldoc.CreateNode(XmlNodeType.Element, "party_id", "");
            party_idNode1.AppendChild(newNodeId1);
            party_idNode1.AppendChild(newNodeOrganization1);
            /*
            party_idNode2.AppendChild(newNodeId2);
            party_idNode2.AppendChild(newNodeOrganization2);
            //*/
            parentNode3.AppendChild(party_idNode1);
            // parentNode3.AppendChild(party_idNode2);
        }
        public static async Task<string[]> GetListOfXMLFiles(string path)
        {
            var listOfFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(s => (Path.GetExtension(s).ToLower() == ".xml")).ToArray();
            return listOfFiles;
        }
    }
}

