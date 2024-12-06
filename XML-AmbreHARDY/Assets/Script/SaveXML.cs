using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using TMPro;
using System.Globalization;

public class SaveXML : MonoBehaviour
{
    private static string _name = "Ambre";
    private static float _time;
    private static float _pushNumber;

    public static string[] saveFileName = { "File1", "File2" };
    public static int currentSaveFile = 0;

    [SerializeField] private List<TMP_Text> _listText;

    private void Start()
    {
        LoadGame();
        ActualiseText();
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    public void ActualiseText()
    {
        _listText[0].text = $"{_name}";
        _listText[1].text = $"{_time}";
        _listText[2].text = $"{_pushNumber}";
    }

    public void OnSave()
    {
        _pushNumber += 1;
        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
        {
            NewLineOnAttributes = true,
            Indent = true,
        };

        Debug.Log(Application.persistentDataPath);
        XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/" + saveFileName[currentSaveFile] + ".xml", xmlWriterSettings);

        writer.WriteStartDocument();
        writer.WriteStartElement("Data");

        WriteXMLString(writer, "Data", saveFileName[0]);
        WriteXMLString(writer, "Name", _name);
        WriteXMLValue(writer, "Time", _time);
        WriteXMLValue(writer, "PushNumber", _pushNumber);

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
    }

    static void WriteXMLString(XmlWriter _writer, string _key, string _value)
    {
        _writer.WriteStartElement(_key);
        _writer.WriteString(_value);
        _writer.WriteEndElement();

    }

    static void WriteXMLValue(XmlWriter _writer, string _key, float _value)
    {
        _writer.WriteStartElement(_key);
        _writer.WriteValue(_value);
        _writer.WriteEndElement();

    }

    public void LoadGame()
    {
        XmlDocument saveFile = new XmlDocument();
        if (!System.IO.File.Exists(Application.persistentDataPath+ "/" + saveFileName[currentSaveFile] + ".xml"))
        {
            return;
        }
        saveFile.LoadXml(System.IO.File.ReadAllText(Application.persistentDataPath + "/" + saveFileName[currentSaveFile] + ".xml"));

        string key;
        string value;
        foreach (XmlNode node in saveFile.ChildNodes[1])
        {
            key = node.Name;
            value = node.InnerText;

            switch (key)
            {
                case "Name":
                    _name = value;
                    break;
                
                case "Time":
                    _time = float.Parse(value, CultureInfo.InvariantCulture);
                    break;

                case "PushNumber":
                    _pushNumber = float.Parse(value);
                    break;
            }
        }
    }
}
