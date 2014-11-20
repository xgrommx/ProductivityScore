using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ProductivityScore.Model;
using ProductivityScore.Utils;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductivityScore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Entries entries = Entries.Singleton; 

        public MainPage()
        {
            this.InitializeComponent();

            HistoryItems.ItemsSource = entries;
        }
    }

    class EntryP : Data
    {
        private int _id;
        public int Id 
        {
            get { return _id; }
            set { SetField(ref _id, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        private int _points;
        public int Points
        {
            get { return _points; }
            set { SetField(ref _points, value); }
        }

        public override string ToString()
        {
            return "[" + Description + ": " + Points + "]";
        }
    }
}
