using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugin2025.Extensions;
using RevitPlugin2025.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitPlugin2025.Models.RevitCommand
{
    [RevitTransaction(TransactionMode.Manual)]
    public abstract class RevitBaseCommand : IRevitCommand, IExternalCommand
    {
        //public TransactionMode Mode = TransactionMode.Manual;
        protected virtual UIDocument uiDoc { get; set; }
        protected virtual Document doc { get; set; }
        protected virtual UIApplication uiApp { get; set; }
        protected virtual TransactionGroup g { get; set; }
        protected virtual Transaction t { get; set; }



        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uiDoc = commandData.Application.ActiveUIDocument;
            doc = uiDoc.Document;
            uiApp = commandData.Application;
            g = new TransactionGroup(doc);
            t = new Transaction(doc);
            FailureHandlingOptions failureHandlingOptions = t.GetFailureHandlingOptions();
            RevitPlugin2025.Models.WarningSwallower.WarningSwallower failureHandler = new RevitPlugin2025.Models.WarningSwallower.WarningSwallower();
            failureHandlingOptions.SetFailuresPreprocessor(failureHandler);
            failureHandlingOptions.SetClearAfterRollback(true);
            t.SetFailureHandlingOptions(failureHandlingOptions);
            try
            {
                IRevitBaseCommandResult preExecuteResult = PreExecute();
                if (preExecuteResult.Result == RevitBaseCommandResponseResult.SUCCESS)
                {
                    IRevitBaseCommandResult executeResult = Execute();
                    if (executeResult.Result == RevitBaseCommandResponseResult.SUCCESS)
                    {
                        IRevitBaseCommandResult postExecuteResult = PostExecute();
                        if (postExecuteResult.Result == RevitBaseCommandResponseResult.FAILURE)
                        {
                            postExecuteResult.ProgressResult();
                        }
                    }
                    else
                    {
                        executeResult.ProgressResult();
                    }
                }
                else
                {
                    preExecuteResult.ProgressResult();
                }
                if (t.HasStarted() && !t.HasEnded())
                {
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string errorDescription = this.GetType().Name + " - " + ex.ClassName() + " - " + ex.LineNumber() + ": " + ex.Message;
                MessageBox.Show(errorDescription);
                //StackTrace st = new StackTrace(true);
                //for (int i = 0; i < st.FrameCount; i++)
                //{
                //    StackFrame sf = st.GetFrame(i);
                //    errorDescription = $"Method: {sf.GetMethod()}; File: ${sf.GetFileName()}; Line Number: ${sf.GetFileLineNumber()}";
                //    //RevitErrorPostModel error = new RevitErrorPostModel(errorDescription, App.RevitVersionName, App.RevitVersionNumber, App.RevitVersionBuild);
                //    //App.RevitErrors.Add(error);
                //}


            }
            finally
            {
                if (t.HasStarted() && !t.HasEnded())
                {
                    t.Commit();
                }
            }
            return Result.Succeeded;
        }
        public virtual IRevitBaseCommandResult PreExecute()
        {
            return this.OkResult();
        }
        public virtual IRevitBaseCommandResult Execute()
        {
            return this.OkResult();
        }
        public virtual IRevitBaseCommandResult PostExecute()
        {
            return this.OkResult();
        }

    }
}
