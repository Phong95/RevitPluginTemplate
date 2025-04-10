using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.WarningSwallower
{
    public class WarningSwallower : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor a)
        {
            //Lấy hết tất cả các Warning
            IList<FailureMessageAccessor> failures = a.GetFailureMessages();
            bool isCommit = false;
            foreach (FailureMessageAccessor f in failures)
            {
                //Lấy id loại của warning (warning có rất nhiều loại, mỗi loại có id tương ứng)
                FailureDefinitionId id = f.GetFailureDefinitionId();
                //So sánh, nếu id của warning bằng với loại warning hiện ra khi thay đổi đối tượng trong group
                if (BuiltInFailures.GroupFailures.AtomViolationWhenOnePlaceInstance == id)
                {
                    //Delete warning đó
                    a.DeleteWarning(f);
                }
                if (BuiltInFailures.CutFailures.CannotCutJoinedElement == id)
                {
                    a.ResolveFailure(f);
                    isCommit = true;
                }
                else { a.DeleteWarning(f); }
            }
            if(isCommit)
            {
                return FailureProcessingResult.ProceedWithCommit;
            }
            return FailureProcessingResult.Continue;
        }
    }
}
