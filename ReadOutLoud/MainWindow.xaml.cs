using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace ReadOutLoud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SpeechSynthesizer synth = new SpeechSynthesizer();
        static string text;
        
        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;
        }

        private static void speakerThreadRoutine()
        {
            try
            {
                // Configure the audio output.   
                synth.SetOutputToDefaultAudioDevice();

                // Speak a string.  
                synth.Speak(text);
            }
            catch (Exception e)
            {

            }
            
        }

        private void Pause_Resume_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            String keyword = btn.Content.ToString();

            if (keyword == "Resume")
            {
                synth.Resume();
                Current_State.Content = "Resumed";
                btn.Content = "Pause";
            }
            else
            {
                synth.Pause();
                Current_State.Content = "Paused";
                btn.Content = "Resume";
            }
        }
        
        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            Window helpWindow = new HelpWindow();

            helpWindow.ShowDialog();
        }

        private void Read_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                text = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Text);

                Thread speakerThread = new Thread(speakerThreadRoutine);
                speakerThread.SetApartmentState(ApartmentState.STA);
                speakerThread.Start();
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                synth.Speak("Unalble To Read The Text");
            }
        }

        private void Configure_Button_Click(object sender, RoutedEventArgs e)
        {
            Window configureWindow = new ConfigureWindow();

            configureWindow.ShowDialog();
        }
    }
}
