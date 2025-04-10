using Autodesk.Revit.UI;
using Autodesk.Windows;
using RevitPlugin2025.Helper.AssemblyLoader;
using RevitPlugin2025.Helper.RevitUtils;
using RevitPlugin2025.Models.Language;
using RevitPlugin2025.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RevitPlugin2025
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class App : IExternalApplication
    {
        public static string AddInPath = typeof(App).Assembly.Location;
        public static RequestHandler requestHandler;
        public static ExternalEvent externalEvent;

        public Result OnStartup(UIControlledApplication application)
        {
            var loader = new AssemblyLoader();
            #region Load Language
            if (Properties.Settings.Default.language == "en")
            {
                var resourceName = "RevitPlugin2025.Resources.en.json";
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    LanguageModel.Instance = JsonSerializer.Deserialize<LanguageModel>(json);

                }
            }
            else
            {
                var resourceName = "RevitPlugin2025.Resources.vi.json";
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    LanguageModel.Instance = JsonSerializer.Deserialize<LanguageModel>(json);
                }
            }

            #endregion

            #region Addin Tab
            string tabName = "Hello world";
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception)
            {

            }
            #endregion

            #region panel 00:


            var Panel_Arc00 = application.CreateRibbonPanel(tabName, LanguageModel.Instance.RibbonPanel.HelloWorldPanel.Name);
            RibbonUtils.AddSinglePushButton(Panel_Arc00
                , LanguageModel.Instance.RibbonPanel.HelloWorldPanel.HelloWorldName
                , AddInPath
                , "RevitPlugin2025.Tools.HelloWorld.HelloWorld"
                , Properties.Resources.google
                , LanguageModel.Instance.RibbonPanel.HelloWorldPanel.HelloWorldInstruction);

            #endregion

            ExternalEventInitial();
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }

        #region enable/disable button
        public static void DisableAllTab()
        {

        }
        public static void EnableAllTab()
        {

        }
        #endregion

        #region Register Event Handler
        private void ExternalEventInitial()
        {
            requestHandler = new RequestHandler();
            externalEvent = ExternalEvent.Create(requestHandler);
        }
        #endregion




    }
}
