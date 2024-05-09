#nullable enable
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\whim.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Bar\Whim.Bar.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.CommandPalette\Whim.CommandPalette.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.FloatingLayout\Whim.FloatingLayout.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.FocusIndicator\Whim.FocusIndicator.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Gaps\Whim.Gaps.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.LayoutPreview\Whim.LayoutPreview.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.SliceLayout\Whim.SliceLayout.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout\Whim.TreeLayout.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout.Bar\Whim.TreeLayout.Bar.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout.CommandPalette\Whim.TreeLayout.CommandPalette.dll"
// #r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Updater\Whim.Updater.dll"

#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.Bar.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.CommandPalette.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.FloatingLayout.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.FocusIndicator.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.Gaps.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.LayoutPreview.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.SliceLayout.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.TreeLayout.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.TreeLayout.Bar.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.TreeLayout.CommandPalette.dll"
#r "D:\Whim\src\Whim.Runner\bin\x64\Debug\net7.0-windows10.0.19041.0\Whim.Updater.dll"


using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Whim;
using Whim.Bar;
using Whim.CommandPalette;
using Whim.FloatingLayout;
using Whim.FocusIndicator;
using Whim.Gaps;
using Whim.LayoutPreview;
using Whim.SliceLayout;
using Whim.TreeLayout;
using Whim.TreeLayout.Bar;
using Whim.TreeLayout.CommandPalette;
using Whim.Updater;
using Windows.Win32.UI.Input.KeyboardAndMouse;

// Mal noch die Comments entfernen?

void DoConfig(IContext context)
{

    context.Logger.Config = new LoggerConfig();
    // Bar plugin.
    List<BarComponent> leftComponents = new() { WorkspaceWidget.CreateComponent() };
    List<BarComponent> centerComponents = new() { DateTimeWidget.CreateComponent() };
    List<BarComponent> rightComponents = new() {
            ActiveLayoutWidget.CreateComponent()
    };

    BarConfig barConfig = new(leftComponents, centerComponents, rightComponents);
    BarPlugin barPlugin = new(context, barConfig);
    context.PluginManager.AddPlugin(barPlugin);

    // Gap plugin.
    GapsConfig gapsConfig = new() { OuterGap = 5, InnerGap = 8 };
    GapsPlugin gapsPlugin = new(context, gapsConfig);
    context.PluginManager.AddPlugin(gapsPlugin);

    /* // Floating window plugin.
       FloatingLayoutPlugin floatingLayoutPlugin = new(context);
       context.PluginManager.AddPlugin(floatingLayoutPlugin); */

    // Focus indicator.
    // Border width of focused window
    // int borderSize = 4;

    // Brush borderBrush = new SolidColorBrush(ColorHelper.FromArgb(255, 235, 111, 146));
    // FocusIndicatorConfig focusIndicatorConfig = new() { Color = borderBrush, FadeEnabled = true,  BorderSize = borderSize };
    // FocusIndicatorPlugin focusIndicatorPlugin = new(context, focusIndicatorConfig);
    // context.PluginManager.AddPlugin(focusIndicatorPlugin);

    // Command palette.
    CommandPaletteConfig commandPaletteConfig = new(context);
    CommandPalettePlugin commandPalettePlugin = new(context, commandPaletteConfig);
    context.PluginManager.AddPlugin(commandPalettePlugin);

    // Slice layout.
    SliceLayoutPlugin sliceLayoutPlugin = new(context);
    context.PluginManager.AddPlugin(sliceLayoutPlugin);

    // Tree layout.
    TreeLayoutPlugin treeLayoutPlugin = new(context);
    context.PluginManager.AddPlugin(treeLayoutPlugin);

    // Tree layout bar.
    TreeLayoutBarPlugin treeLayoutBarPlugin = new(treeLayoutPlugin);
    context.PluginManager.AddPlugin(treeLayoutBarPlugin);
    rightComponents.Add(treeLayoutBarPlugin.CreateComponent());

    // Tree layout command palette.
    TreeLayoutCommandPalettePlugin treeLayoutCommandPalettePlugin =
        new(context, treeLayoutPlugin, commandPalettePlugin);
    context.PluginManager.AddPlugin(treeLayoutCommandPalettePlugin);

    // Layout preview.
    LayoutPreviewPlugin layoutPreviewPlugin = new(context);
    context.PluginManager.AddPlugin(layoutPreviewPlugin);

    // Updater.
    UpdaterConfig updaterConfig = new() { ReleaseChannel = ReleaseChannel.Alpha };
    UpdaterPlugin updaterPlugin = new(context, updaterConfig);
    context.PluginManager.AddPlugin(updaterPlugin);

    // Original Config Set up workspaces.
    /* context.WorkspaceManager.Add("1");
       context.WorkspaceManager.Add("2");
       context.WorkspaceManager.Add("3");
       context.WorkspaceManager.Add("4");

    // Set up layout engines.
    context.WorkspaceManager.CreateLayoutEngines = () =>
    new CreateLeafLayoutEngine[]
    {
    (id) => SliceLayouts.CreateMultiColumnLayout(context, sliceLayoutPlugin, id, 1, 2, 0),
    (id) => SliceLayouts.CreatePrimaryStackLayout(context, sliceLayoutPlugin, id),
    (id) => SliceLayouts.CreateSecondaryPrimaryLayout(context, sliceLayoutPlugin, id),
    (id) => new FocusLayoutEngine(id),
    (id) => new TreeLayoutEngine(context, treeLayoutPlugin, id)
    }; */

    ////////////////////////////////////////////////////////////////////////////////////////////////
    
    string file = context.FileManager.GetWhimFileDir("bar.rose-pine.xaml");
    context.ResourceManager.AddUserDictionary(file);

    //Config workspaces
    context.WorkspaceManager.Add("\udb80\udedc");

    context.WorkspaceManager.Add(
            "\ueaae",
            new CreateLeafLayoutEngine[]
            {
                (id) => new FocusLayoutEngine(id),
                (id) => SliceLayouts.CreateMultiColumnLayout(context, sliceLayoutPlugin, id, 1, 2, 0)
            });
    context.WorkspaceManager.Add(
            "Utils",
            new CreateLeafLayoutEngine[]
            {
                (id) => SliceLayouts.CreateMultiColumnLayout(context, sliceLayoutPlugin, id, 1, 2, 0),
                (id) => new TreeLayoutEngine(context, treeLayoutPlugin, id),
                (id) => new FocusLayoutEngine(id)
            });
    context.WorkspaceManager.Add(
            "Launcher",
            new CreateLeafLayoutEngine[]
            {
                (id) => new TreeLayoutEngine(context, treeLayoutPlugin, id)
            });
    context.WorkspaceManager.Add(
            "Misc",
            new CreateLeafLayoutEngine[]
            {
                (id) => new TreeLayoutEngine(context, treeLayoutPlugin, id),
                (id) => new FocusLayoutEngine(id)
            });
    context.WorkspaceManager.Add("Discord");
    context.WorkspaceManager.Add("Private");

    // Default LayoutEngine
    //
    context.WorkspaceManager.CreateLayoutEngines = () => new CreateLeafLayoutEngine[] {
            (id) => new TreeLayoutEngine(context, treeLayoutPlugin, id),
            (id) => SliceLayouts.CreateMultiColumnLayout(context, sliceLayoutPlugin, id, 1, 2, 0),
            (id) => new FocusLayoutEngine(id),
        };

    // Workspace RouterManager
    context.RouterManager.AddTitleMatchRoute("PowerShell", "\udb80\udedc");
    context.RouterManager.AddTitleMatchRoute("Ubuntu", "\udb80\udedc");

    context.RouterManager.AddProcessFileNameRoute("firefox.exe", "\ueaae");

    context.RouterManager.AddProcessFileNameRoute("explorer.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("notepad.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("notepad++.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("WinSCP.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("nvim-qt.exe", "Utils");
    context.RouterManager.AddTitleMatchRoute("Einstellungen", "Utils");

    context.RouterManager.AddProcessFileNameRoute("Steam.exe", "Launcher");
    context.RouterManager.AddProcessFileNameRoute("Battle.net Launcher.exe", "Launcher");
    context.RouterManager.AddTitleMatchRoute("Battle.net", "Launcher");
    context.RouterManager.AddTitleRoute("Steam", "Launcher");

    context.RouterManager.AddProcessFileNameRoute("OUTLOOK.EXE", "Misc");
    context.RouterManager.AddProcessFileNameRoute("Discord.exe", "Discord");

    context.RouterManager.AddProcessFileNameRoute("Obsidian.exe", "Private");
    context.RouterManager.AddProcessFileNameRoute("KeePassXC.exe", "Private");
    context.RouterManager.AddProcessFileNameRoute("Spotify.exe", "Private");

    // Filter

    // https://github.com/urob/whim-config/blob/main/whim.config.csx#L254
    // Custom filters (aka ignored windows)
    context.FilterManager.AddTitleMatchFilter(".*[s|S]etup.*");
    context.FilterManager.AddTitleMatchFilter(".*[i|I]nstaller.*");
    context.FilterManager.Add((window) => window.WindowClass.StartsWith("WindowsForms10.Window.20008.app"));  // preview window of explorer on Windows10

    // Own Filters
    context.FilterManager.AddTitleMatchFilter("Erweiterung: *"); // firefox TreeStyle Tabs

    context.FilterManager.AddTitleFilter("Overwatch");
    context.FilterManager.AddTitleFilter("Enshrouded");
    context.FilterManager.AddTitleFilter("Terraria");
    context.FilterManager.AddProcessFileNameFilter("Terraria.exe");

    // Keybinds

    // https://github.com/urob/whim-config/blob/main/whim.config.csx#L189
    KeyModifiers Alt = KeyModifiers.LAlt;
    KeyModifiers Ctrl = KeyModifiers.LControl;
    KeyModifiers ShiftAlt = KeyModifiers.LAlt | KeyModifiers.LShift;
    KeyModifiers ShiftCtrl = KeyModifiers.LShift | KeyModifiers.LControl;

    void Bind(KeyModifiers mod, string key, string cmd)
    {
        VIRTUAL_KEY vk = (VIRTUAL_KEY)Enum.Parse(typeof(VIRTUAL_KEY), "VK_" + key);
        context.KeybindManager.SetKeybind(cmd, new Keybind(mod, vk));
    }
    
    Bind(Ctrl, "P", "whim.command_palette.toggle");

    Bind(ShiftCtrl, "Q", "whim.core.exit_whim");
    Bind(ShiftCtrl, "R", "whim.core.restart_whim");

    // cycle layout engine
    Bind(Alt, "SPACE", "whim.core.cycle_layout_engine.next");
    // promote and demote a window
    Bind(Alt, "P", "whim.slice_layout.window.promote");

    // Move Window to Workspace
    Bind(Alt, "M", "whim.command_palette.move_window_to_workspace");

    // Vim like window movement
    Bind(Alt, "H", "whim.core.focus_window_in_direction.left");
    Bind(Alt, "J", "whim.core.focus_window_in_direction.down");
    Bind(Alt, "K", "whim.core.focus_window_in_direction.up");
    Bind(Alt, "L", "whim.core.focus_window_in_direction.right");

    // Workspace Navigation mit Alt+number
    Bind(Alt, "1", "whim.core.activate_workspace_1");
    Bind(Alt, "2", "whim.core.activate_workspace_2");
    Bind(Alt, "3", "whim.core.activate_workspace_3");
    Bind(Alt, "4", "whim.core.activate_workspace_4");
    Bind(Alt, "8", "whim.core.activate_workspace_5");
    Bind(Alt, "9", "whim.core.activate_workspace_6");
    Bind(Alt, "0", "whim.core.activate_workspace_7");

    // TreeLayout Keybinds
    //
    // add window in direction
    Bind(ShiftCtrl, "H", "whim.tree_layout.add_tree_direction_left");
    Bind(ShiftCtrl, "J", "whim.tree_layout.add_tree_direction_down");
    Bind(ShiftCtrl, "K", "whim.tree_layout.add_tree_direction_up");
    Bind(ShiftCtrl, "L", "whim.tree_layout.add_tree_direction_right");

    // swap window in direction
    //
    //
    // Create the command.
    context.CommandManager.Add(
            // Automatically namespaced to `whim.custom`.
            identifier: "close_window",
            title: "Close focused window",
            callback: () => context.WorkspaceManager.ActiveWorkspace.LastFocusedWindow.Close()
            );
    Bind(ShiftCtrl, "D", "whim.custom.close_window");
}
// We return doConfig here so that Whim can call it when it loads.
return DoConfig;






