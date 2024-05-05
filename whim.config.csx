#nullable enable
#r "C:\Users\Slash\AppData\Local\Programs\Whim\whim.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Bar\Whim.Bar.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.CommandPalette\Whim.CommandPalette.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.FloatingLayout\Whim.FloatingLayout.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.FocusIndicator\Whim.FocusIndicator.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Gaps\Whim.Gaps.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.LayoutPreview\Whim.LayoutPreview.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.SliceLayout\Whim.SliceLayout.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout\Whim.TreeLayout.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout.Bar\Whim.TreeLayout.Bar.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.TreeLayout.CommandPalette\Whim.TreeLayout.CommandPalette.dll"
#r "C:\Users\Slash\AppData\Local\Programs\Whim\plugins\Whim.Updater\Whim.Updater.dll"

using System;
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
    // string file = context.FileManager.GetWhimFileDir("resources.xaml");
    // context.ResourceManager.AddUserDictionary(file);

    context.Logger.Config = new LoggerConfig();

    // Bar plugin.
    List<BarComponent> leftComponents = new() { WorkspaceWidget.CreateComponent() };
    List<BarComponent> centerComponents = new() { FocusedWindowWidget.CreateComponent() };
    List<BarComponent> rightComponents = new() { 
        DateTimeWidget.CreateComponent(),
        ActiveLayoutWidget.CreateComponent(),
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
    /* FocusIndicatorConfig focusIndicatorConfig = new() { Color = new SolidColorBrush(Colors.Red), FadeEnabled = true };
       FocusIndicatorPlugin focusIndicatorPlugin = new(context, focusIndicatorConfig);
       context.PluginManager.AddPlugin(focusIndicatorPlugin); */

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

    ////////////////////////////////////////////////////////////////////////////////////////////////
    //Own Config workspaces
    context.WorkspaceManager.Add("Main"); 
    context.WorkspaceManager.Add("Browser",
            new CreateLeafLayoutEngine[]
            {
            (id) => new FocusLayoutEngine(id)
            }); 
    // Workspace mit TreeLayoutEngine
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
    context.WorkspaceManager.CreateLayoutEngines = () => new CreateLeafLayoutEngine[]
    {
        (id) => SliceLayouts.CreateMultiColumnLayout(context, sliceLayoutPlugin, id, 1, 2, 0),
        (id) => new FocusLayoutEngine(id),
    };

    // Routen für App in workspaces

    context.RouterManager.AddProcessFileNameRoute("ubuntu.exe", "Main");
    context.RouterManager.AddProcessFileNameRoute("cmd.exe", "Main");
    context.RouterManager.AddProcessFileNameRoute("pwsh.exe", "Main");

    context.RouterManager.AddProcessFileNameRoute("firefox.exe", "Browser");

    context.RouterManager.AddProcessFileNameRoute("explorer.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("notepad.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("notepad++.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("WinSCP.exe", "Utils");
    context.RouterManager.AddProcessFileNameRoute("oodipro.exe", "Utils");

    context.RouterManager.AddProcessFileNameRoute("Steam.exe", "Launcher");
    context.RouterManager.AddProcessFileNameRoute("Battle.net Launcher.exe", "Launcher");
    context.RouterManager.AddTitleMatchRoute("Battle.net", "Launcher");
    context.RouterManager.AddTitleRoute("Steam", "Launcher");

    context.RouterManager.AddProcessFileNameRoute("Discord.exe", "Discord");

    context.RouterManager.AddProcessFileNameRoute("Obsidian.exe", "Private");
    context.RouterManager.AddProcessFileNameRoute("Mullvad VPN.exe", "Private");

    // Filter

    context.FilterManager.AddTitleFilter("Overwatch");
    context.FilterManager.AddTitleFilter("Enshrouded");
    context.FilterManager.AddTitleFilter("Terraria");
    context.FilterManager.AddProcessFileNameFilter("Terraria.exe");

    // Keybinds


    context.KeybindManager.SetKeybind("whim.core.restart_whim", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_R));
    context.KeybindManager.SetKeybind("whim.core.exit_whim", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_Q));
    context.KeybindManager.SetKeybind("whim.core.cycle_layout_engine.next", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_SPACE));

    context.KeybindManager.SetKeybind("whim.command_palette.move_window_to_workspace", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_M));

    // Vim like window movement
    KeyModifiers ShiftCtrl = KeyModifiers.LShift | KeyModifiers.LControl; 

    context.KeybindManager.SetKeybind("whim.core.focus_window_in_direction.left", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_H));
    context.KeybindManager.SetKeybind("whim.core.focus_window_in_direction.down", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_J));
    context.KeybindManager.SetKeybind("whim.core.focus_window_in_direction.up", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_K));
    context.KeybindManager.SetKeybind("whim.core.focus_window_in_direction.right", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_L));

    // Workspace Navigation mit Alt+Zahl

    context.KeybindManager.SetKeybind("whim.core.activate_workspace_1", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_1));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_2", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_2));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_3", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_3));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_4", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_4));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_5", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_8));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_6", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_9));
    context.KeybindManager.SetKeybind("whim.core.activate_workspace_7", new Keybind(KeyModifiers.LAlt, VIRTUAL_KEY.VK_0));

    context.KeybindManager.SetKeybind("whim.tree_layout.add_tree_direction_left", new Keybind(ShiftCtrl, VIRTUAL_KEY.VK_H));
    context.KeybindManager.SetKeybind("whim.tree_layout.add_tree_direction_down", new Keybind(ShiftCtrl, VIRTUAL_KEY.VK_J));
    context.KeybindManager.SetKeybind("whim.tree_layout.add_tree_direction_up", new Keybind(ShiftCtrl, VIRTUAL_KEY.VK_K));
    context.KeybindManager.SetKeybind("whim.tree_layout.add_tree_direction_right", new Keybind(ShiftCtrl, VIRTUAL_KEY.VK_L));

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

}

// We return doConfig here so that Whim can call it when it loads.
return DoConfig;
