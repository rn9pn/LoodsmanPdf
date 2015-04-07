using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loodsman;
using DataProvider;
using System.IO;

namespace LoodsmanPdf
{
    class LoodsmanWorker
    {
        private IPluginCall API; 
        private object returncode = 0;
        private object errmes = 0;

        public LoodsmanWorker(IPluginCall _loodsman)
        {
            API = _loodsman;

            if (API == null)
            {
                throw new Exception("Не удалось подключиться к Kompas3D");
            }
        }

        /// <summary>
        /// Возвращает id-шники всех объектов типа "Документ" входящие в селектируемый объект
        /// </summary>             
        internal List<int> GetAllDocs()
        {
            List<int> allDocs = new List<int>();

            DataSet docs = new DataSet();
            
            if(API.stType == "Документ")
            {
                allDocs.Add(API.IdVersion);
            }
            
            docs.DATA = API.RunMethod("FindObjectsInContext", new object[] { 
                Convert.ToString(API.IdVersion),
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
                    allDocs.Add(docs.get_FieldValue("_ID_VERSION"));

                    docs.Next();
                }
            }

            return allDocs;
        }

        /// <summary>
        /// Выгрузить на диск файл с расширением ".kdw"
        /// </summary>
        /// <param name="_id">Id документа</param>
        /// <returns>Имя файла с путем</returns>
        internal string GetFile(int _id)
        {
            string fName;
            string fLocalName;

            DataSet fileInfo = new DataSet();
            fileInfo.DATA = API.RunMethod("GetInfoAboutVersion", new object[] { "", "", "", _id, 7});
            if(fileInfo.RecordCount > 0)
            {
                fileInfo.First();
                while (!fileInfo.Eof)
                {
                    fName = fileInfo.get_FieldValue("_NAME");
                    fLocalName = fileInfo.get_FieldValue("_LOCALNAME");
                    if(Path.GetExtension(fName) == ".kdw")
                    {
                        return API.RunMethod("GetFileById", new object[] { _id, fName, fLocalName, returncode, errmes });
                    }
                    fileInfo.Next();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Загрузить вторичное представление
        /// </summary>
        /// <param name="_id">Id объекта</param>
        /// <param name="_pdfFileString">Строка из массива байт</param>
        internal void SetPdf(int _id, string _pdfFileString)
        {
            API.RunMethod("GetReport", new object[] { "CreateSecondaryRepresentation", _id, "pdffile=" + _pdfFileString, returncode, errmes });
        }
    }
}
