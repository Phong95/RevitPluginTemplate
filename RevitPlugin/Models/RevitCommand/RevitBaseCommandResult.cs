using RevitPlugin.Models.RevitCommandErrorCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Models.RevitCommand
{
    public class RevitBaseCommandResult: IRevitBaseCommandResult
    {
        public RevitBaseCommandResponseResult Result { get; set; }
        public string Message { get; set; }
        public RevitCommandErrorCode.RevitCommandErrorCode ErrorCode { get; set; }

    }
}
