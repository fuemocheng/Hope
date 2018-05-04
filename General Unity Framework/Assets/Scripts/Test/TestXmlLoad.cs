using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// 在程序中访问进而操作XML文件一般有两种模型，分别是使用DOM（文档对象模型）和流模型。
/// 使用DOM的好处在于它允许编辑和更新XML文档，可以随机访问文档中的数据，可以使用XPath查询，但是，DOM的缺点在于它需要一次性的加载整个文档到内存中，对于大型的文档，这会造成资源问题。
/// 流模型很好的解决了这个问题，因为它对XML文件的访问采用的是流的概念，也就是说，任何时候在内存中只有当前节点，但它也有它的不足，它是只读的，仅向前的，不能在文档中执行向后导航操作。
///   1: 使用 XmlDocument
///   2: 使用 XmlTextReader
///   3: 使用 Linq to Xml
/// </summary>
public class TestXmlLoad : MonoBehaviour
{

    void Start()
    {
        //TestXmlDocument();
        //TestXmlTextReader();
        //TestXmlTextWriter();
        TestXElement();
    }

    void Update()
    {

    }

    #region XmlDocument
    /// <summary>
    /// DOM（文档对象模型）：XmlDocument是读取整个XML的，所以如果XML内容过多，则会消费很多内存。所以XML内容过大时，不推荐使用XmlDocument。
    /// </summary> 
    private void TestXmlDocument()
    {
        TestXmlDocumentRead();
        //TestXmlDocumentAdd();
        //TestXmlDocumentRemove();
        TestXmlDocumentModify();
    }

    private void TestXmlDocumentRead()
    {
        XmlDocument xmlDoc = new XmlDocument();
        //直接读会 注释也会成为一个子节点
        //doc.Load(Application.streamingAssetsPath + "/xml/Test.xml");

        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;     //忽略文档里面的注释
        XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/xml/Test.xml", readerSettings);
        xmlDoc.Load(reader);
        reader.Close();

        //得到根节点bookstore
        XmlNode xn = xmlDoc.SelectSingleNode("bookstore");
        XmlNodeList xnl = xn.ChildNodes;
        foreach (XmlNode xmlNode in xnl)
        {
            // 将节点转换为元素，便于得到节点的属性值
            XmlElement xe = (XmlElement)xmlNode;
            string attr0 = xe.GetAttribute("Type").ToString();
            string attr1 = xe.GetAttribute("ISBN").ToString();
            Debug.Log("Type:" + attr0 + " " + "ISBN:" + attr1);

            XmlNodeList xnlChild = xmlNode.ChildNodes;
            string item0 = xnlChild.Item(0).InnerText;
            string item1 = xnlChild.Item(1).InnerText;
            float item2 = (float)Convert.ToDouble(xnlChild.Item(2).InnerText);
            Debug.Log(item0 + "-" + item1 + "-" + item2.ToString());

            Debug.Log(xmlNode.SelectSingleNode("title").Name + ": " + xmlNode.SelectSingleNode("title").Value);
            Debug.Log(xmlNode.SelectSingleNode("author").LocalName + ": " + xmlNode.SelectSingleNode("author").InnerXml);
            Debug.Log(xmlNode.LastChild.Name + ": " + xmlNode.LastChild.InnerText);
        }
    }

    private void TestXmlDocumentAdd()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;
        XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/xml/Test.xml", readerSettings);
        xmlDoc.Load(reader);
        reader.Close();

        XmlNode root = xmlDoc.SelectSingleNode("bookstore");

        //创建一个节点，并设置属性
        XmlElement element = xmlDoc.CreateElement("book");
        XmlAttribute attribute = xmlDoc.CreateAttribute("type");
        attribute.InnerText = "必修课";
        element.SetAttributeNode(attribute);
        element.SetAttribute("type2", "null");

        XmlElement child = xmlDoc.CreateElement("author");
        child.InnerText = "famous";
        element.AppendChild(child);

        root.AppendChild(element);
        xmlDoc.Save(Application.streamingAssetsPath + "/xml/Test.xml");
    }

    private void TestXmlDocumentRemove()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;
        XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/xml/Test.xml", readerSettings);
        xmlDoc.Load(reader);
        reader.Close();

        //DocumentElement 获取xml文档对象的根XmlElement.
        XmlElement xeRoot = xmlDoc.DocumentElement;
        string strPath = string.Format("/bookstore/book[@ISBN=\"{0}\"]", "7-111-19149-1");
        //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
        XmlElement selectXe = (XmlElement)xeRoot.SelectSingleNode(strPath);
        selectXe.ParentNode.RemoveChild(selectXe);

        //删除保存到文件
        //xmlDoc.Save(Application.streamingAssetsPath + "/xml/Test.xml");
    }

    private void TestXmlDocumentModify()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;
        XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/xml/Test.xml", readerSettings);
        xmlDoc.Load(reader);
        reader.Close();

        //DocumentElement 获取xml文档对象的根XmlElement.
        XmlElement xeRoot = xmlDoc.DocumentElement;
        string strPath = string.Format("/bookstore/book[@ISBN=\"{0}\"]", "7-111-19149-1");

        //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
        XmlElement selectXe = (XmlElement)xeRoot.SelectSingleNode(strPath);
        selectXe.SetAttribute("Type", "必修");
        selectXe.GetElementsByTagName("title").Item(0).InnerText = "操作系统";
        selectXe.GetElementsByTagName("author").Item(0).InnerText = "MMM";
        selectXe.GetElementsByTagName("price").Item(0).InnerText = "30.00";

        //修改保存到文件
        xmlDoc.Save(Application.streamingAssetsPath + "/xml/Test.xml");
    }

    #endregion

    #region XmlTextReader & XmlTextWriter

    private void TestXmlTextReader()
    {
        XmlTextReader reader = new XmlTextReader(Application.streamingAssetsPath + "/xml/Test.xml");
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "book")
                {
                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        string attr = reader.Name + ":" + reader.Value;
                        Debug.Log(attr);
                    }

                    string att0 = reader.GetAttribute(0);
                    string att1 = reader.GetAttribute(1);
                    Debug.Log("book attr0:" + att0 + " attr1" + att1);
                }

                if (reader.Name == "title")
                {
                    string value = reader.ReadElementString().Trim();
                    Debug.Log("title: " + value);
                }
                if (reader.Name == "author")
                {
                    string value = reader.ReadElementString().Trim();
                    Debug.Log("author:" + value);
                }
                if (reader.Name == "price")
                {
                    float value = reader.ReadElementContentAsFloat();
                    Debug.Log("price: " + value);
                }
            }
            if (reader.NodeType == XmlNodeType.EndElement)
            {
                Debug.Log("EndElement");
            }
        }
    }

    private void TestXmlTextWriter()
    {
        XmlTextWriter xmlTextWriter = new XmlTextWriter(Application.streamingAssetsPath + "/xml/Test2.xml", null);
        //使用 Formatting 属性指定希望将 XML 设定为何种格式。 这样，子元素就可以通过使用 Indentation 和 IndentChar 属性来缩进。
        xmlTextWriter.Formatting = Formatting.Indented;
        xmlTextWriter.WriteStartDocument(false);
        xmlTextWriter.WriteStartElement("bookstore");

        xmlTextWriter.WriteComment("记录书本信息");

        xmlTextWriter.WriteStartElement("book");
        xmlTextWriter.WriteAttributeString("Type", "");
        xmlTextWriter.WriteAttributeString("ISBN", "7-111-19149-1");

        xmlTextWriter.WriteElementString("title", "操作系统");
        xmlTextWriter.WriteElementString("author", "KKK");
        xmlTextWriter.WriteElementString("price", "36.00");

        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();

        xmlTextWriter.Flush();
        xmlTextWriter.Close();
    }

    #endregion

    #region Linq to XML

    private void TestXElement()
    {
        TestXElementRead();
        //TestXElementInsert();
        //TestXElementDelete();
        //TestXElementDeleteAll();
        //TestXElementModify();
    }

    private void ShowInfoByElement(IEnumerable<XElement> elements)
    {
        foreach (var ele in elements)
        {
            string title = ele.Element("title").Value;
            string author = ele.Element("author").Value;
            string price = ele.Element("price").Value;

            string type = ele.Attribute("Type").Value;
            string ISBN = ele.Attribute("ISBN").Value;

            Debug.Log("title:" + title + " author:" + author + " price:" + price + " type:" + type + "ISBN:" + ISBN);
        }
    }

    private void TestXElementRead()
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/xml/Test.xml");
        while (www.isDone == false)
        {
            //未加载完成
        }
        if (www == null)
        {
            Debug.LogError(Application.streamingAssetsPath + "/xml/Test.xml" + " is null");
            return;
        }

        //XElement xe = XElement.Load(Application.streamingAssetsPath + "/xml/Test.xml");

        //这里文件必须是UTF-8无BOM编码格式
        XElement xe = XElement.Parse(www.text);
        IEnumerable<XElement> elements = from ele in xe.Elements("book")
                                         select ele;
        ShowInfoByElement(elements);
    }

    /// <summary>
    /// 直接找到元素为book的这个结点,然后遍历读取所有的结果
    /// </summary>
    private void TestXElementInsert()
    {
        XElement xe = XElement.Load(Application.streamingAssetsPath + "/xml/Test.xml");
        XElement record = new XElement(
            new XElement("book",
            new XAttribute("Type", "NewType"),
            new XAttribute("ISBN", "7-111-19149-1"),
            new XElement("title", "title"),
            new XElement("author", "author"),
            new XElement("price", "20.00")));

        xe.Add(record);
        xe.Save(Application.streamingAssetsPath + "/xml/Test.xml");
        Debug.Log("Insert Success");
    }

    /// <summary>
    /// 首先得到选中的那一行,通过Type来找到这个元素，然后用Remove方法直接删除
    /// </summary>
    private void TestXElementDelete()
    {
        XElement xe = XElement.Load(Application.streamingAssetsPath + "/xml/Test.xml");

        IEnumerable<XElement> elements = from ele in xe.Elements("book")
                                         where ele.Attribute("Type").Value == "NewType"
                                         select ele;

        if (elements.Count() > 0)
        {
            elements.First().Remove();
        }

        xe.Save(Application.streamingAssetsPath + "/xml/Test.xml");
        Debug.Log("Delete Success");
    }

    /// <summary>
    /// 选出所有的数据，然后用Remove方法
    /// </summary>
    private void TestXElementDeleteAll()
    {
        XElement xe = XElement.Load(Application.streamingAssetsPath + "/xml/Test2.xml");
        IEnumerable<XElement> elements = from ele in xe.Elements("book")
                                         select ele;

        if (elements.Count() > 0)
        {
            elements.Remove();
        }

        xe.Save(Application.streamingAssetsPath + "/xml/Test2.xml");
        Debug.Log("Delete All Success");
    }

    /// <summary>
    /// 得到所要修改的某一个结点，然后用SetAttributeValue来修改属性，用ReplaceNodes来修改结点元素。
    /// </summary>
    private void TestXElementModify()
    {
        XElement xe = XElement.Load(Application.streamingAssetsPath + "/xml/Test2.xml");

        IEnumerable<XElement> elements = from ele in xe.Elements("book")
                                         where ele.Attribute("Type").Value == "NewType"
                                         select ele;

        if (elements.Count() > 0)
        {
            XElement first = elements.First();
            //设置新属性
            first.SetAttributeValue("Type", "Type0");
            //替换新节点
            first.ReplaceNodes(
                new XElement("title", "title3"),
                new XElement("author", "author3"),
                new XElement("price", "20.00")
                );
        }
        xe.Save(Application.streamingAssetsPath + "/xml/Test2.xml");
        Debug.Log("Modify Success");
    }

    #endregion
}
