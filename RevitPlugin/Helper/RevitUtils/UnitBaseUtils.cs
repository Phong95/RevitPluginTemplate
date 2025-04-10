using Autodesk.Revit.DB;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class UnitBaseUtils
    {
        public static double MmToFt(double a)
        {
#if REVIT2021
                return UnitUtils.ConvertToInternalUnits(a,UnitTypeId.Millimeters);
#elif REVIT2022 || REVIT2023 || REVIT2024
            return UnitUtils.ConvertToInternalUnits(a, UnitTypeId.Millimeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
#endif
        }
        public static double FtToMm(double a)
        {
#if REVIT2021
                return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.Millimeters);
#elif REVIT2022 || REVIT2023 || REVIT2024
                return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.Millimeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS);
#endif
        }
        public static double F3toM3(double a)
        {
#if REVIT2021
                return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.CubicMeters);
#elif REVIT2022 || REVIT2023 || REVIT2024
            return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.CubicMeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_CUBIC_FEET, DisplayUnitType.DUT_CUBIC_METERS);
#endif
        }
        public static double F2toM2(double a)
        {
#if REVIT2021
                return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.SquareMeters);
#elif REVIT2022 || REVIT2023 || REVIT2024
            return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.SquareMeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS);
#endif
        }
        public static double RadToDegree(double a)
        {
#if REVIT2021
                return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.Degrees);
#elif REVIT2022 || REVIT2023 || REVIT2024
            return UnitUtils.ConvertFromInternalUnits(a, UnitTypeId.Degrees);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_RADIANS, DisplayUnitType.DUT_DECIMAL_DEGREES);
#endif
        }
    }
}
