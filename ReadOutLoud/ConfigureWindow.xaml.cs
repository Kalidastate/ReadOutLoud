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

            Voice_Speed_Slider.Value = MainWindow.synth.Rate + 10;

            Voice_Speed.Content = string.Format("Speed: ({0})", Voice_Speed_Slider.Value);
        }

        private void Voice_Selection_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.synth.SelectVoice((string)Voice_Selection.SelectedItem);
        }

        private void Voice_Speed_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Voice_Speed.Content = string.Format("Speed: ({0})", Voice_Speed_Slider.Value);

            MainWindow.synth.Rate = (int)Voice_Speed_Slider.Value - 10;
        }
    }
}
