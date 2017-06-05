using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using OpenBBot.Services;
using OpenBBot.Validators;


namespace OpenBBot
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ClickingThreadService ClickingThread;

        public MainWindow()
        {
            this.ClickingThread = new ClickingThreadService(20);
            GlobalListenerService.InstallHook(ClickingThread.Switch);
            InitializeComponent();
        }

        // Validates Numbers Only
        private void IntervalInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InputValidators.ValidateAgainstNumber(e.Text);
        }

        private void IntervalInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
    }
}
