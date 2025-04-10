using RevitPlugin2025.Models.Language;
using RevitPlugin2025.Models.RevitCommand;
using RevitPlugin2025.Models.RevitCommandErrorCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitPlugin2025.Extensions
{
    public static class RevitBaseCommandExtensions
    {
        public static RevitBaseCommandResult OkResult(this RevitBaseCommand command)
        {
            RevitBaseCommandResult result = new RevitBaseCommandResult();
            result.Result = RevitBaseCommandResponseResult.SUCCESS;
            return result;
        }        
        
        public static RevitBaseCommandResult OkResult(this RevitBaseCommand command,string message)
        {
            RevitBaseCommandResult result = new RevitBaseCommandResult();
            result.Result = RevitBaseCommandResponseResult.SUCCESS;
            result.Message = message;
            return result;
        }

        public static RevitBaseCommandResult ErrorResult(this RevitBaseCommand command)
        {
            RevitBaseCommandResult result = new RevitBaseCommandResult();
            result.Result = RevitBaseCommandResponseResult.FAILURE;
            return result;
        }
        public static RevitBaseCommandResult ErrorResult(this RevitBaseCommand command,string message)
        {
            RevitBaseCommandResult result = new RevitBaseCommandResult();
            result.Result = RevitBaseCommandResponseResult.FAILURE;
            result.Message = message;
            return result;
        }   
        
        public static RevitBaseCommandResult ErrorResult(this RevitBaseCommand command, RevitCommandErrorCode code)
        {
            RevitBaseCommandResult result = new RevitBaseCommandResult();
            result.Result = RevitBaseCommandResponseResult.FAILURE;
            result.ErrorCode = code;
            return result;
        }


        #region Result Extensions
        public static void ProgressResult (this IRevitBaseCommandResult result)
        {
            if(result.Result != RevitBaseCommandResponseResult.SUCCESS && result.Message.IsNotNull())
            {
                MessageBox.Show(result.Message);
            }
            else if(result.Result != RevitBaseCommandResponseResult.SUCCESS && result.ErrorCode.IsNotNull())
            {
                MessageBox.Show(LanguageModel.Instance.RevitCommandErrorCode.GetPropValue<string>(result.ErrorCode.ToString()));
            }

        }
        #endregion
    }
}
