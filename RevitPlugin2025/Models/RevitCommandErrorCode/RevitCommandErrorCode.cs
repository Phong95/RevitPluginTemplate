using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.RevitCommandErrorCode
{
    public enum RevitCommandErrorCode
    {
        #region Select Elements 0 - 50
        SELECT_FLOOR_BEFORE_RUN = 0,
        SELECT_REBAR_BEFORE_RUN = 1,
        SELECT_FRAME_BEFORE_RUN = 2,
        SELECT_CAD_DRAWING_BEFORE_RUN = 3,
        SELECT_ELEMENTS_BEFORE_RUN = 4,
        SELECT_VIEW_BEFORE_RUN = 5,
        SELECT_STRUCTURAL_COLUMN_BEFORE_RUN = 6,

        #endregion

        #region Work Condition 51 - 100
        ONLY_WORK_IN_2D = 51,
        ONLY_WORK_IN_3D = 52,
        ONLY_WORK_WITH_STRAIGHT_FRAME = 53,
        ONLY_WORK_WITH_STRAIGHT_COLUMN = 54,
        #endregion

        #region Incorrect Input 101-150
        INVALID_DATA = 101,
        INVALID_REF_LOCATION = 102,
        INVALID_LOCATION = 103,
        INVALID_PARAMETER = 104,
        INVALID_ROOM = 105,
        INVALID_FLOOR_TYPE = 106,
        INVALID_WALL_HEIGHT = 107,
        INVALID_LINE_STYLE = 108,
        INVALID_RULE = 109,
        INVALID_CATEGORY = 110,
        #endregion

        #region Dont have 151-200
        DONT_HAVE_ANY_ELEMENTS = 151,
        DONT_HAVE_ANY_SCHEDULE = 152,
        DONT_HAVE_ANY_TITLE_BLOCK = 153,
        DONT_HAVE_ANY_WARNING = 154,
        DONT_HAVE_ANY_VIEW_SHEET = 155,
        #endregion

        #region License 200-250
        INVALID_LICENSE=201,
        EXPIRED_LICENSE=202
        #endregion
    }
}
