using Playnite.SDK;
using Playnite.SDK.Models;
using System;
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
            InitializeComponent();
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
                plugin.PlayniteApi.Database.Games.Update(game);
                ((Window)this.Parent).Close();
            } catch (Exception E)
            {
                logger.Error(E, "Error when parsing time");
                plugin.PlayniteApi.Dialogs.ShowErrorMessage(E.Message, "Error when parsing time");
            }
        }
    }
}
