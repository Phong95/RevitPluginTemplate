using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Models.Language.RevitCommandErrorCode
{
    public class RevitCommandErrorCode
    {
        #region Select Elements
        public string SELECT_FLOOR_BEFORE_RUN { get; set; }
        public string SELECT_REBAR_BEFORE_RUN { get; set; }
        public string SELECT_FRAME_BEFORE_RUN { get; set; }
        public string SELECT_CAD_DRAWING_BEFORE_RUN { get; set; }
        public string SELECT_ELEMENTS_BEFORE_RUN { get; set; }
        public string SELECT_VIEW_BEFORE_RUN { get; set; }
        public string SELECT_STRUCTURAL_COLUMN_BEFORE_RUN { get; set; }
        #endregion

        #region Work Condition
        public string ONLY_WORK_IN_2D { get; set; }
        public string ONLY_WORK_IN_3D { get; set; }
        public string ONLY_WORK_WITH_STRAIGHT_FRAME { get; set; }
        public string ONLY_WORK_WITH_STRAIGHT_COLUMN { get; set; }
        #endregion

        #region Incorrect Input
        public string INVALID_DATA { get; set; }
        public string INVALID_REF_LOCATION { get; set; }
        public string INVALID_LOCATION { get; set; }
        public string INVALID_PARAMETER { get; set; }
        public string INVALID_ROOM { get; set; }
        public string INVALID_FLOOR_TYPE { get; set; }
        public string INVALID_WALL_HEIGHT { get; set; }
        public string INVALID_LINE_STYLE { get; set; }
        public string INVALID_RULE{ get; set; }
        public string INVALID_CATEGORY { get; set; }
        #endregion

        #region Dont have
        public string DONT_HAVE_ANY_ELEMENTS { get; set; }
        public string DONT_HAVE_ANY_SCHEDULE { get; set; }
        public string DONT_HAVE_ANY_TITLE_BLOCK { get; set; }
        public string DONT_HAVE_ANY_WARNING { get; set; }
        public string DONT_HAVE_ANY_VIEW_SHEET { get; set; }
        #endregion

        #region License
        public string INVALID_LICENSE { get; set; }
        public string EXPIRED_LICENSE { get; set; }
        #endregion
    }
}
