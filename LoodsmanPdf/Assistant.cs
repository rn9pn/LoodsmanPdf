using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loodsman;
using DataProvider;

namespace LoodsmanPdf
{
    internal static class Assistant
    {
        private static object returncode = 0;
        private static object errmes = 0;

        /// <summary>
        /// Возвращает id-шники всех объектов типа "Документ"
        /// </summary>
        /// <param name="_idVersion">Контекст поиска, задается через запятую</param>        
        internal static List<int> GetAllDocs(string _idVersion)
        {
            List<int> allDocs = new List<int>();

            DataSet docs = new DataSet();
                        
            docs.DATA = Main.APlugin.RunMethod("FindObjectsInContext2", new object[] { 
                _idVersion,
                "<FIND><LinksStep id=\"1\" stepname=\"Связи1\" direction=\"down\" recursive=\"true\"><LinkList><LinkType name=\"Журнал изменений\"/><LinkType name=\"Документы\"/>"+
                "<LinkType name=\"На основании\"/><LinkType name=\"Разработан по\"/><LinkType name=\"Заявка на\"/><LinkType name=\"Использует\"/><LinkType name=\"Элемент маршрута\"/>"+
                "<LinkType name=\"Касается\"/><LinkType name=\"Состоит из ...\"/><LinkType name=\"Содержит\"/><LinkType name=\"Для изделий\"/><LinkType name=\"Применяется для ...\"/>"+
                "<LinkType name=\"Исполнения\"/><LinkType name=\"Изготавливается из ...\"/><LinkType name=\"Заготовка для\"/><LinkType name=\"Маршрут\"/>"+
                "<LinkType name=\"Создает версию ...\"/><LinkType name=\"Оснастка\"/><LinkType name=\"Проектная документация\"/><LinkType name=\"Потребляет\"/>"+
                "<LinkType name=\"Взаимозаменяемые\"/><LinkType name=\"Фрагменты\"/><LinkType name=\"Документы в архив\"/><LinkType name=\"Приложение\"/><LinkType name=\"Эквивалент\"/>"+
                "<LinkType name=\"Технологическая сборка\"/><LinkType name=\"Для операции\"/><LinkType name=\"Для сборки\"/><LinkType name=\"Технологический состав для\"/>"+
                "</LinkList><FilterStep id=\"2\" stepname=\"Фильтр1\"><TypeList><Type name=\"Документ\"/></TypeList><ResultStep id=\"3\" stepname=\"Результат1\" AllResult=\"union\"/>"+
                "</FilterStep></LinksStep></FIND>",
                0,
                returncode,
                errmes
            });

            if (docs.RecordCount > 0)
            {
                docs.First();
                while (!docs.Eof)
                {
                    if (docs.get_FieldValue("_TYPE") == "Сборочная единица")
                    {
                        allDocs.Add(docs.get_FieldValue("_ID_VERSION"));
                    }
                    docs.Next();
                }
            }

            return allDocs;
        }
    }
}
