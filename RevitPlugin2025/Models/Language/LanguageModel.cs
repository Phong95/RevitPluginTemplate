
using RevitPlugin2025.Models.Language.Panel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.Language
{
    public class LanguageModel
    {
        private static LanguageModel instance = null;
        public static LanguageModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LanguageModel();
                    var resourceName = "RevitPlugin20252025.Resources.en.json";
                    var assembly = Assembly.GetExecutingAssembly();
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        instance = JsonSerializer.Deserialize<LanguageModel>(json);

                    }
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public RibbonPanelModel RibbonPanel { get; set; }
        public RevitPlugin2025.Models.Language.Tool_Form.ToolFormModel ToolForm { get; set; }
        public RevitPlugin2025.Models.Language.RevitCommandErrorCode.RevitCommandErrorCode RevitCommandErrorCode { get; set; }

    }
}
