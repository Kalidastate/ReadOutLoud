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
        static SpeechSynthesizer synth = new SpeechSynthesizer();
        static string text;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        public MainWindow()
        {
            InitializeComponent();

            Thread workerThread = new Thread(WorkerThreadDelegate);
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();
        }

        private static void WorkerThreadDelegate()
        {
            _hookID = SetHook(_proc);
            System.Windows.Forms.Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                
                if ((Keys)vkCode == Keys.F7)
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
                        synth.Speak("Unalble TO Read The Text");
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);

        }

        private static void speakerThreadRoutine()
        {
            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.  
            synth.Speak(text);
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            synth.Resume();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            synth.Pause();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);
    }
}
