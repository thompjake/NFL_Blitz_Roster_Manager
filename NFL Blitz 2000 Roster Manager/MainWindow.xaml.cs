using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NFL_Blitz_2000_Roster_Manager.Models;
using System.IO;
using System.Globalization;
using Microsoft.Win32;
using System.ComponentModel;
using NFL_Blitz_2000_Roster_Manager.Helpers;
using System.Collections.ObjectModel;

namespace NFL_Blitz_2000_Roster_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region vars
        private Teams blitzTeams;
        public Teams BlitzTeams
        {
            get { return blitzTeams; }
            set { blitzTeams = value; RaisePropertyChanged("BlitzTeams"); }
        }

        private Dictionary<string, ObservableCollection<BlitzGame>> systemToGames;
        public Dictionary<string, ObservableCollection<BlitzGame>> SystemToGames
        {
            get { return systemToGames; }
            set { systemToGames = value; RaisePropertyChanged("SystemToGames"); }
        }

        private ObservableCollection<BlitzGame> selectedSystemGames;
        public ObservableCollection<BlitzGame> SelectedSystemGames
        {
            get { return selectedSystemGames; }
            set { selectedSystemGames = value; RaisePropertyChanged("SelectedSystemGames"); }
        }

        private BlitzGame selectedGame;
        public BlitzGame SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; RaisePropertyChanged("SelectedGame"); }
        }

        private int teamCountOverride;

        public int TeamCountOverride
        {
            get { return teamCountOverride; }
            set { teamCountOverride = value; RaisePropertyChanged("TeamCountOverride"); }
        }

        private string currentOpenRomName;

        private string   xmlFilePath;
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Dictionary<string, ObservableCollection<BlitzGame>> games = new Dictionary<string, ObservableCollection<BlitzGame>>();
            List<BlitzGame> listOfGameFiles = GameFileManager.LoadAllGameFiles();
            foreach (BlitzGame gameFile in listOfGameFiles)
            {
                if (!games.Keys.Contains(gameFile.SystemName))
                {
                    games.Add(gameFile.SystemName, new ObservableCollection<BlitzGame>());
                }
                games[gameFile.SystemName].Add(gameFile);
            }
            SystemToGames = games;
        }

        internal void CallPropertyChangedOnAll()
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
        }

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSystemGames == null && SelectedGame == null)
            {
                MessageBox.Show("Please select a system and game version");
                return;
            }
            SaveFileDialog savDialog = new SaveFileDialog();
            Nullable<bool> result = savDialog.ShowDialog();

            if (result == true)
            {
                string savFile = savDialog.FileName;
                if (SelectedGame.ExpandableTeams)
                    SelectedGame.GameTeamCount = BlitzTeams.Count;
                RomEditor.SaveToRom(savFile, SelectedGame, BlitzTeams);
            }
        }

        private void WriteStringToFile(string toBeWritten, string filePath, string offset, bool writeToBlank = true)
        {
            using (var fs = new FileStream(filePath,
FileMode.Open,
FileAccess.ReadWrite))
            {
                fs.Position = long.Parse(offset);
                foreach (char letter in toBeWritten)
                {
                    fs.WriteByte(Convert.ToByte(letter));
                }
                //if write to blank is set to true we will write blanks until we hit a blank
                while (true)
                {
                    long currentPostiton = fs.Position;
                    byte thisByte = byte.Parse(fs.ReadByte().ToString());
                    if (thisByte == 00)
                    {
                        break;
                    }
                    fs.Position = currentPostiton;
                    fs.WriteByte(00);
                }
            }
        }


        private void WriteIntToFile(int toBeWritten, string filePath, string offset)
        {

            using (var fs = new FileStream(filePath,
FileMode.Open,
FileAccess.ReadWrite))
            {
                fs.Position = long.Parse(offset);
                var numberByte = Byte.Parse(toBeWritten.ToString(), NumberStyles.HexNumber);
                fs.WriteByte((byte)numberByte);
            }
        }


        private void btnSaveRosterXml_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savDialog = new SaveFileDialog();
            savDialog.Filter = "NFL Blitz 2000 Roster File(*.xml)\"|*.xml";
            savDialog.AddExtension = true;

            Nullable<bool> result = savDialog.ShowDialog();

            if (result == true)
            {
                string savFile = savDialog.FileName;
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Teams));
                System.IO.StreamWriter file = new System.IO.StreamWriter(savFile);
                writer.Serialize(file, BlitzTeams);
                file.Close();
            }
        }

        private void btnLoadRosterXml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "NFL Blitz 2000 Roster File(*.xml)\"|*.xml";
            openDialog.AddExtension = true;
            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true)
            {
                string openFile = openDialog.FileName;
                BlitzTeams = XmlHelper.DeSerializeObject<Teams>(openFile);
            }
        }

        private void btnReadRom_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSystemGames == null && SelectedGame == null)
            {
                MessageBox.Show("Please select a system and game version");
                return;
            }
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.AddExtension = true;
            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true)
            {
                string openFile = currentOpenRomName = openDialog.FileName;
                BlitzTeams = RomEditor.ReadRom(openFile, SelectedGame, selectedGame.GameTeamCount);
            }
        }

    }


}
