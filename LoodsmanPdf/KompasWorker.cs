using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Kompas6API5;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;
using Pdf2d_LIBRARY;
using System.IO;

namespace LoodsmanPdf
{
    class KompasWorker
    {
        private KompasObject API5;
        private _Application API7;
             
        public KompasWorker(bool _visible)
        {
            try
            {
                API5 = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
            }
            catch
            {
                API5 = (KompasObject)Activator.CreateInstance(Type.GetTypeFromProgID("KOMPAS.Application.5"));
            }

            if (API5 != null)
            {
                API5.Visible = _visible;

                API5.ActivateControllerAPI();

                API7 = (_Application)API5.ksGetApplication7();
            }
            else
            {
                throw new Exception("Не удалось подключиться к Kompas3D");
            }
        }        

        /// <summary>
        /// Сконвертировать файл в pdf-формат
        /// </summary>
        /// <param name="_path">Полный путь до файла</param>
        public string ConvertFile(string _path)
        {                        
            IConverter pdfConverter = (IConverter)API7.get_Converter(@Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Pdf2d(x64).dll");
            //IPdf2dParam pdfParam = (IPdf2dParam)pdfConverter.ConverterParameters(0);
            //pdfParam.ColorType = 1;                
            if(pdfConverter.Convert(_path, _path + ".pdf", 1, false) == 1)
            {                
                return _path + ".pdf";
            }

            return string.Empty;
        }

        /// <summary>
        /// Выйти из Компас3D
        /// </summary>
        public void Quit()
        {
            API5.Quit();
        }
    }
}
