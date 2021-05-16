using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReadOutLoud
{
    /// <summary>
    /// Interaction logic for ConfigureWindow.xaml
    /// </summary>
    public partial class ConfigureWindow : Window
    {
        public ConfigureWindow()
        {
            InitializeComponent();
            Topmost = true;

            ReadOnlyCollection<InstalledVoice> installedVoices = MainWindow.synth.GetInstalledVoices();

            foreach (InstalledVoice iv in installedVoices)
            {
                Voice_Selection.Items.Add(iv.VoiceInfo.Name);
            }

            Voice_Selection.SelectedItem = MainWindow.synth.Voice.Name;


        }

        private void Voice_Selection_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.synth.SelectVoice((string)Voice_Selection.SelectedItem);
        }
    }
}
