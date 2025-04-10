using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugin2025.Extensions;
using RevitPlugin2025.Models.ModelessDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RevitPlugin2025.Tools
{
    public partial class RequestHandler : IExternalEventHandler
    {
        // The value of the latest request made by the modeless form 
        private Request m_request = new Request();

        /// <summary>
        /// A public property to access the current request value
        /// </summary>
        public Request Request
        {
            get { return m_request; }
        }

        public void Execute(UIApplication app)
        {

            switch (Request.Take())
            {
                case RequestId.NONE:
                    {
                        return;  // no request at this time -> we can leave immediately
                    }
             
                case RequestId.RETRIEVE_VIEWS:
                    {
                        this.RetrieveViews(app);
                        break;
                    }               


                default:
                    {
                        // some kind of a warning here should
                        // notify us about an unexpected request 
                        break;
                    }
            }

        }

        public string GetName()
        {
            return "R2014 External Event Sample";
        }

        public void RetrieveViews(UIApplication app)
        {

        }

    }
}
