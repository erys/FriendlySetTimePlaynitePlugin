using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FriendlySetTime
{
    // renamed this from FriendlySetTime because visual studio was getting confused about the same namespace+name
    public class FriendlySetPlugin : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private FriendlySetTimeSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("1f05ed2e-f22d-45e0-8e81-16378a8464c7");

        public FriendlySetPlugin(IPlayniteAPI api) : base(api)
        {
            Properties = new GenericPluginProperties
            {
                HasSettings = false
            };
        }

        public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            List<GameMenuItem> gameMenuItems = new List<GameMenuItem>();
            if (args.Games.Count == 1)
            {
                gameMenuItems.Add(new GameMenuItem
                {
                    Description = "Set Playtime",
                    Action = (GameMenuItem) =>
                    {
                        DoSetTime(args.Games[0]);
                    }
                });
            }
            return gameMenuItems;
        }

        private void DoSetTime(Game game)
        {
            try
            {
                SetTimeWindow view = new SetTimeWindow(this, game);
                var window = PlayniteApi.Dialogs.CreateWindow(new WindowCreationOptions
                {
                    ShowMinimizeButton = false,
                });

                var themeName = PlayniteApi.ApplicationSettings.DesktopTheme;

                if (themeName == "Stardust 2.0_1fb333b2-255b-43dd-aec1-8e2f2d5ea002")
                {
                    window.Height = 195;
                }
                else
                {
                    window.Height = 170;
                }

                window.Width = 520;
                window.Title = "Set Time";
                window.Content = view;

                window.Owner = PlayniteApi.Dialogs.GetCurrentAppWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                window.ShowDialog();
            } catch (Exception E) {
                logger.Error(E, "Error during creatin set time window");
                PlayniteApi.Dialogs.ShowErrorMessage(E.Message, "Error during set time");
            }
        }

        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(OnGameStartedEventArgs args)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(OnGameStartingEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            // Add code to be executed when Playnite is initialized.
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new FriendlySetTimeSettingsView();
        }
    }
}