using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FriendlySetTime
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SetTimeWindow : UserControl
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Seconds { get; set; }

        public List<string> statuses { get; set; } = new List<string>();
        private readonly FriendlySetPlugin plugin;
        private readonly Game game;

        public SetTimeWindow(FriendlySetPlugin plugin, Game game)
        {
            this.game = game;
            ulong curseconds = game.Playtime;
            Seconds = (curseconds % 60).ToString();
            ulong bigMinutes = curseconds / 60;
            Minutes = (bigMinutes % 60).ToString();
            Hours = (bigMinutes / 60).ToString();
            this.plugin = plugin;

            foreach (CompletionStatus completionStatus in plugin.PlayniteApi.Database.CompletionStatuses)
            {
                statuses.Add(completionStatus.Name);
            }

            InitializeComponent();
            newStatus.SelectedIndex = statuses.IndexOf(game.CompletionStatus.Name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint mins = UInt32.Parse(minutes.Text.Trim());
                uint hrs = UInt32.Parse(hours.Text.Trim());
                ulong scnds = UInt64.Parse(seconds.Text.Trim());
                scnds += mins * 60;
                scnds += hrs * 3600;
                game.Playtime = scnds;
                if ((bool)updateStatus.IsChecked)
                {
                    string status = newStatus.SelectedItem.ToString();
                    game.CompletionStatusId = plugin.PlayniteApi.Database.CompletionStatuses.Where(x => x.Name == status).DefaultIfEmpty(game.CompletionStatus).First().Id;
                }
                plugin.PlayniteApi.Database.Games.Update(game);
                ((Window)this.Parent).Close();
            } catch (Exception E)
            {
                logger.Error(E, "Error when parsing time");
                plugin.PlayniteApi.Dialogs.ShowErrorMessage(E.Message, "Error when parsing time");
            }
        }

        private void statusChanged(object sender, SelectionChangedEventArgs e)
        {
            if (game.CompletionStatus.Name != newStatus.SelectedItem.ToString())
            {
                updateStatus.IsChecked = true;
            }
        }
    }
}
