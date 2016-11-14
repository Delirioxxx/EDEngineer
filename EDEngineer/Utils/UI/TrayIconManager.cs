using System;
using System.Diagnostics;
using System.Windows.Forms;
using EDEngineer.Models.Localization;
using EDEngineer.Utils.System;

namespace EDEngineer.Utils.UI
{
    public static class TrayIconManager
    {
        public static IDisposable Init(EventHandler showHandler,
            EventHandler quitHandler,
            EventHandler configureShortcutHandler,
            EventHandler unlockWindowHandler,
            EventHandler resetWindowHandler,
            EventHandler selectLanguageHandler)
        {
            var menu = BuildContextMenu(showHandler, quitHandler, configureShortcutHandler, unlockWindowHandler, resetWindowHandler, selectLanguageHandler);
            
            var icon = new NotifyIcon
            {
                Icon = Properties.Resources.elite_dangerous_icon,
                Visible = true,
                Text = "ED - Engineer",
                ContextMenu = menu
            };

            return Disposable.Create(() =>
            {
                icon.Visible = false;
                icon.Icon = null;
                icon.Dispose();
            });
        }

        private static ContextMenu BuildContextMenu(EventHandler showHandler, 
            EventHandler quitHandler, 
            EventHandler configureShortcutHandler, 
            EventHandler unlockWindowHandler, 
            EventHandler resetWindowHandler,
            EventHandler selectLanguageHandler)
        {
            var translator = Languages.Instance;

            var showItem = new MenuItem();
            showItem.Click += showHandler;

            var unlockItem = new MenuItem();
            unlockItem.Click += unlockWindowHandler;

            var resetItem = new MenuItem();
            resetItem.Click += resetWindowHandler;

            var selectLanguageItem = new MenuItem();
            selectLanguageItem.Click += selectLanguageHandler;

            var setShortCutItem = new MenuItem();
            setShortCutItem.Click += configureShortcutHandler;

            var helpItem = new MenuItem();
            helpItem.Click += (o,e) => Process.Start("https://github.com/msarilar/EDEngineer/wiki/Troubleshooting-Issues");

            var quitItem = new MenuItem();

            SetItemsText(quitItem, translator, helpItem, setShortCutItem, selectLanguageItem, resetItem, unlockItem, showItem);
            translator.PropertyChanged += (o, e) =>
            {
                SetItemsText(quitItem, translator, helpItem, setShortCutItem, selectLanguageItem, resetItem, unlockItem, showItem);
            };

            quitItem.Click += quitHandler;

            var menu = new ContextMenu();
            menu.MenuItems.Add(showItem);
            menu.MenuItems.Add(unlockItem);
            menu.MenuItems.Add(resetItem);
            menu.MenuItems.Add(setShortCutItem);
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(selectLanguageItem);
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(helpItem);
            menu.MenuItems.Add(quitItem);
            return menu;
        }

        private static void SetItemsText(MenuItem quitItem, Languages translator, MenuItem helpItem, MenuItem setShortCutItem,
                                         MenuItem selectLanguageItem, MenuItem resetItem, MenuItem unlockItem, MenuItem showItem)
        {
            quitItem.Text = translator.Translate("Quit");
            helpItem.Text = translator.Translate("Help");
            setShortCutItem.Text = translator.Translate("Set Shortcut");
            selectLanguageItem.Text = translator.Translate("Select Language");
            resetItem.Text = translator.Translate("Reset Window Position");
            unlockItem.Text = translator.Translate("Toggle Window Mode (Locked/Unlocked)");
            showItem.Text = translator.Translate("Show");
        }
    }
}