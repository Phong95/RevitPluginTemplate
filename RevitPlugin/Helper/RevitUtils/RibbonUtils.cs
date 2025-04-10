using Autodesk.Revit.UI;
using Autodesk.Windows;
using RevitPlugin.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class RibbonUtils
    {
        public static SplitButton AddSplitButton(string TenSplit, Autodesk.Revit.UI.RibbonPanel rp)
        {
            SplitButtonData splitButtonData01 = new SplitButtonData(TenSplit, TenSplit);
            SplitButton splitButton01 = rp.AddItem(splitButtonData01) as SplitButton;
            return splitButton01;
        }
        public static SplitButton AddSplitButton(string TenSplit, Autodesk.Revit.UI.RibbonPanel rp, string instructionLink)
        {
            SplitButtonData splitButtonData01 = new SplitButtonData(TenSplit, TenSplit);
            SplitButton splitButton01 = rp.AddItem(splitButtonData01) as SplitButton;
            HelpLink(splitButton01, instructionLink);
            return splitButton01;
        }
        public static void AddPushButtonInSplitButton(SplitButton spbutton,
            string nameaddin,
            string addinpath,
            string classname,
            Bitmap image,
            string tooltip)
        {
            PushButton pushButton = spbutton.AddPushButton(new PushButtonData(nameaddin, nameaddin, addinpath, classname)) as PushButton;
            pushButton.LargeImage = image.GetBitmapSource();
            pushButton.ToolTip = tooltip;
            HelpLink(pushButton);
        }
        public static void AddPushButtonInSplitButton(SplitButton spbutton,
    string nameaddin,
    string addinpath,
    string classname,
    Bitmap image,
    string tooltip,
    string instructionLink)
        {
            PushButton pushButton = spbutton.AddPushButton(new PushButtonData(nameaddin, nameaddin, addinpath, classname)) as PushButton;
            pushButton.LargeImage = image.GetBitmapSource();
            pushButton.ToolTip = tooltip;
            HelpLink(pushButton, instructionLink);
        }
        public static void AddSinglePushButton(Autodesk.Revit.UI.RibbonPanel rp,
            string nameaddin,
            string addinpath,
            string classname,
            Bitmap image,
            string tooltip)
        {
            PushButton pushButton = rp.AddItem(new PushButtonData(nameaddin, nameaddin, addinpath, classname)) as PushButton;
            pushButton.LargeImage = image.GetBitmapSource();
            pushButton.ToolTip = tooltip;
            HelpLink(pushButton);
        }
        public static void AddSinglePushButton(Autodesk.Revit.UI.RibbonPanel rp,
    string nameaddin,
    string addinpath,
    string classname,
    Bitmap image,
    string tooltip,
    string instructionLink)
        {
            PushButton pushButton = rp.AddItem(new PushButtonData(nameaddin, nameaddin, addinpath, classname)) as PushButton;
            pushButton.LargeImage = image.GetBitmapSource();
            pushButton.ToolTip = tooltip;
            HelpLink(pushButton, instructionLink);
        }
        //add singlebutton with tooltip video
        public static void AddSinglePushButton(Autodesk.Revit.UI.RibbonPanel rp, string nameaddin, string addinpath, string classname, Bitmap image, string tooltip, string instructionLink, string tooltipVideoName)
        {
            PushButton pushButton = rp.AddItem(new PushButtonData(nameaddin, nameaddin, addinpath, classname)) as PushButton;
            pushButton.LargeImage = image.GetBitmapSource();
            //pushButton.ToolTip = tooltip;
            HelpLink(pushButton);
            RibbonToolTip toolTip = new RibbonToolTip()
            {
                Title = nameaddin,
                ExpandedContent = tooltip,
                ExpandedVideo = new Uri(@"D:\PHONG\Github\RevitAddin\COFICO\Cofico\Cofico\bin\Debug\TooltipVideo\" + tooltipVideoName),
                //                ExpandedImage = BitmapSourceConverter
                //    .ConvertFromImage(Resources.logoITC_16)
            };
            SetRibbonItemToolTip(pushButton, toolTip);
        }
        public static void AddStackedItem(string tabname, Autodesk.Revit.UI.RibbonPanel rp, string AddInPath, string TenTool1, string classNameTool1, Bitmap image1, string tooltip1, string TenTool2, string classNameTool2, Bitmap image2, string tooltip2)
        {
            PushButtonData pushButton = new PushButtonData(TenTool1, TenTool1, AddInPath, classNameTool1);
            pushButton.LargeImage = image1.GetBitmapSource();
            pushButton.ToolTip = tooltip1;

            PushButtonData pushButton1 = new PushButtonData(TenTool2, TenTool2, AddInPath, classNameTool2);
            pushButton1.LargeImage = image2.GetBitmapSource();
            pushButton1.ToolTip = tooltip2;
            //Tạo StackedItem (2item)
            IList<Autodesk.Revit.UI.RibbonItem> Stackedit1 = rp.AddStackedItems(pushButton, pushButton1);
            var btn1 = GetButton(tabname, rp.Name, TenTool1);
            var btn2 = GetButton(tabname, rp.Name, TenTool2);
            btn1.Size = Autodesk.Windows.RibbonItemSize.Large;
            btn1.ShowText = false;
            btn2.Size = Autodesk.Windows.RibbonItemSize.Large;
            btn2.ShowText = false;
        }
        public static void AddStacked3Item(string tabname, Autodesk.Revit.UI.RibbonPanel rp, string AddInPath, string TenTool1, string classNameTool1, Bitmap image1, string tooltip1, string TenTool2, string classNameTool2, Bitmap image2, string tooltip2, string TenTool3, string classNameTool3, Bitmap image3, string tooltip3)
        {
            PushButtonData pushButton = new PushButtonData(TenTool1, TenTool1, AddInPath, classNameTool1);
            pushButton.Image = image1.GetBitmapSource();
            pushButton.ToolTip = tooltip1;

            PushButtonData pushButton1 = new PushButtonData(TenTool2, TenTool2, AddInPath, classNameTool2);
            pushButton1.Image = image2.GetBitmapSource();
            pushButton1.ToolTip = tooltip2;

            PushButtonData pushButton2 = new PushButtonData(TenTool3, TenTool3, AddInPath, classNameTool3);
            pushButton2.Image = image3.GetBitmapSource();
            pushButton2.ToolTip = tooltip3;
            //Tạo StackedItem (3item)
            IList<Autodesk.Revit.UI.RibbonItem> Stackedit1 = rp.AddStackedItems(pushButton, pushButton1, pushButton2);
            var btn1 = GetButton(tabname, rp.Name, TenTool1);
            var btn2 = GetButton(tabname, rp.Name, TenTool2);
            var btn3 = GetButton(tabname, rp.Name, TenTool3);
            btn1.ShowText = false;
            btn2.ShowText = false;
            btn3.ShowText = false;
        }

        public static List<RibbonTab> GetTabs()
        {

            RibbonControl ribbonControl = Autodesk.Windows.ComponentManager.Ribbon;
            return ribbonControl.Tabs.ToList();
        }
        
        #region Lấy pushbutton để set size thành 24x24 cho stackeditem (2Item)
        private static Autodesk.Windows.RibbonItem GetButton(string tabName, string panelName, string itemName)
        {
            Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Name == tabName)
                {
                    foreach (Autodesk.Windows.RibbonPanel panel in tab.Panels)
                    {
                        if (panel.Source.Title == panelName)
                        {
                            return panel.FindItem("CustomCtrl_%CustomCtrl_%"
                              + tabName + "%" + panelName + "%" + itemName,
                              true) as Autodesk.Windows.RibbonItem;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region HelpLink cho command

        /// <summary>
        /// example @"https://www.facebook.com/phong.huynh.90"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="InstructionLink"></param>
        private static void HelpLink(PushButton a, string InstructionLink = @"https://www.facebook.com/phong.huynh.90")
        {
            ContextualHelp contextualHelp = new ContextualHelp(ContextualHelpType.Url, InstructionLink);
            a.SetContextualHelp(contextualHelp);
        }
        private static void HelpLink(SplitButton a, string InstructionLink = @"https://www.facebook.com/phong.huynh.90")
        {
            ContextualHelp contextualHelp = new ContextualHelp(ContextualHelpType.Url, InstructionLink);
            a.SetContextualHelp(contextualHelp);
        }
        #endregion

        #region setTooltip cho button
        private static Autodesk.Windows.RibbonItem GetRibbonItem(Autodesk.Revit.UI.RibbonItem item)
        {
            Type itemType = item.GetType();

            var mi = itemType.GetMethod("getRibbonItem",
              BindingFlags.NonPublic | BindingFlags.Instance);

            var windowRibbonItem = mi.Invoke(item, null);

            return windowRibbonItem
              as Autodesk.Windows.RibbonItem;
        }
        private static void SetRibbonItemToolTip(Autodesk.Revit.UI.RibbonItem item, RibbonToolTip toolTip)
        {
            var ribbonItem = GetRibbonItem(item);
            if (ribbonItem == null)
                return;
            ribbonItem.ToolTip = toolTip;
        }
        #endregion

    }
}
