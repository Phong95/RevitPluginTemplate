using RevitPlugin2025.Models.RevitCommandErrorCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.RevitCommand
{
    public interface IRevitBaseCommandResult
    {
        RevitBaseCommandResponseResult Result { get; set; }
        string Message { get; set; }
        RevitCommandErrorCode.RevitCommandErrorCode ErrorCode { get; set; }
    }
}
