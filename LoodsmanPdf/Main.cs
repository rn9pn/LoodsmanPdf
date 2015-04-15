using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Loodsman;
using DataProvider;
using NLog;

namespace LoodsmanPdf
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("9F970E60-F4A3-4015-8C1F-6AE0F679858A")]
    [ComVisible(true)]
    public interface ILoodsmanPdf
    {
        void Command1(IPluginCall APlugin);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("DB6DC3EB-4F28-4596-A11D-1977A8E598C3")]
    [ProgId("Loodsman.LoodsmanPdf")]
    [ComVisible(true)]
    public class Main : ILoodsmanPdf
    {              
        public static Logger logger;        
        private object returncode = 0;
        private object errmes = 0;

        public Main()
        {
            logger = LogManager.GetCurrentClassLogger();
            string pathNlogConfig = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Nlog.config";
            if (File.Exists(pathNlogConfig))
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(pathNlogConfig, true);
            }
            else
            {
                MessageBox.Show("Не найден файл \"Nlog.config\" по следующему пути: " + pathNlogConfig);
                return;
            }
        }
       
        public void Command1(IPluginCall _APlugin)
        {
            logger.Info("Создать вторичное представление");

            //if(Protection() == true)
            //{                
            //    return;
            //}

            LoodsmanWorker loodsman = new LoodsmanWorker(_APlugin);

            KompasWorker kompas = new KompasWorker(false);

            List<int> allDocs = loodsman.GetAllDocs();            

            foreach (int id in allDocs)
            {
                try
                {
                    logger.Info("Отработка плагина для объекта с id = " + Convert.ToString(id));

                    string filePath = loodsman.GetFile(id);

                    logger.Info("Путь до исходного файла: " + filePath);
                    
                    string pdfFilePath = kompas.ConvertFile(filePath);

                    logger.Info("Путь до сконвертированного файла: " + pdfFilePath);
                    
                    byte[] binaryArray = File.ReadAllBytes(pdfFilePath);
                    
                    string pdfFileString = BinaryToString(binaryArray);
                                        
                    CRC32.BuildTable();

                    string crcSumm = ((uint)CRC32.Crc(binaryArray)).ToString(); 
                    
                    loodsman.SetPdf(id, crcSumm, pdfFileString);

                    logger.Info("Вторичное представление для объекта с id = " + Convert.ToString(id) + " успешно загружено в Лоцман");

                    loodsman.RefreshSelectedObject();
                }
                catch(Exception ex)
                {
                    logger.Error("Ошибка при формировании вторичного представления для объекта с id = " + Convert.ToString(id) + " , сообщение об ошибке: " + ex.Message);
                }
            }

            kompas.Quit();

            logger.Info("Выход из программы");
        }

        /// <summary>
        /// Конвертирование файла в строку, состоящую из массива байт
        /// </summary>
        /// <param name="pdfFilePath">Путь до файла</param>        
        public static string BinaryToString(byte[] _binaryArray)
        {
            string hex = BitConverter.ToString(_binaryArray);

            return "0x" + hex.Replace("-", "");            
        }

        private static bool Protection()
        {
            if (DateTime.Now >= Convert.ToDateTime("20.04.2015"))
            {
                MessageBox.Show("Время работы пробной версии вышло.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }    
}