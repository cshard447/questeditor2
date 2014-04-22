﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StalkerOnlineQuesterEditor
{
    //! Класс, содержащий данные о территориях, которые нужно посетить по квестам
    public class CZoneConstants
    {
        //! Словарь ID территории (mark в xml файле) - Имя территории (по-русски, для геймдевов)
        Dictionary<string, CZoneDescription> zones;

        //! Конструктор, создает словарь на основе xml файлов areas и AllAreas
        public CZoneConstants()
        {
            zones = new Dictionary<string, CZoneDescription>();
            XDocument doc = XDocument.Load("source/Areas.xml");
            foreach (XElement item in doc.Root.Elements())            
            {
                string id = item.Element("mark").Value.ToString().Trim();
                string name = item.Element("description").Value.ToString().Trim();
                zones.Add(id, new CZoneDescription(name));
            }

            // добавление неназванных зон из AllAreas.xml - создается парсером по всем chunk
            XDocument allAreas = XDocument.Load("source/AllAreas.xml");
            foreach(XElement item in allAreas.Root.Elements())
            {
                string id = item.Element("mark").Value.ToString().Trim();
                if (!zones.ContainsKey(id))
                    zones.Add(id, new CZoneDescription(id));
            }

        }
        //! Возвращает словарь всех территорий
        public Dictionary<string, CZoneDescription> getAllZones()
        {
            return zones;
        }
        //! Возвращает описание территории по-русски по ее ключу mark
        public CZoneDescription getDescriptionOnKey(string key)
        {
            //System.Console.WriteLine("key:" + key);
            return zones[key.Trim()];
        }
        //! Возвращает ключ по описанию территории
        public string getKeyOnDescription(string description)
        {
            foreach (string key in zones.Keys)
                if (zones[key].getName().Equals(description))
                    return key;
            return "";
        }
    }

    //! Класс описания территории для посещения. Имеет только поле имя. Что за пиздец???
    public class CZoneDescription
    {
        string Name;

        public CZoneDescription(string name)
        {
            this.Name = name;
        }

        public string getName()
        {
            return this.Name;
        }
    }
}