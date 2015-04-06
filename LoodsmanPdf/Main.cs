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
        public static IPluginCall APlugin;
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

            APlugin = _APlugin;

            switch(APlugin.stType)
            {
                case "Сборочная единица":
                    {
                        logger.Info("Выбран тип \"Сборочная единица\"");

                        List<int> allDocs = Assistant.GetAllDocs(Convert.ToString(APlugin.IdVersion));
                    }
                    break;

                case "Документ":
                    {
                        logger.Info("Выбран тип \"Документ\"");
                    }
                    break;

                default:
                    {
                        logger.Info("Выбран тип \"" + APlugin.stType + "\"");

                        MessageBox.Show("Выбирите тип \"Сборочная единица\" или \"Документ\"");
                    }
                    break;
            }

            logger.Info("Выход из программы");
        }
    }
}