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
using System.Reactive;
using System.Reactive.Subjects;
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
        ModelList<Entry> entries = new ModelList<Entry>();
        ModelList<Template> templates = new ModelList<Template>();


        Subject<Template> AddTemplateStream = new Subject<Template>();
        Subject<Entry> AddEntryStream = new Subject<Entry>();

        Subject<Entry> DeleteEntryStream = new Subject<Entry>();


        public MainPage()
        {
            this.InitializeComponent();

            HistoryItems.ItemsSource = entries;
            TemplateItems.ItemsSource = templates;

            AddTemplateStream.Subscribe(x => templates.Add(x));
            AddEntryStream.Subscribe(x => entries.Add(x));

            DeleteEntryStream.Subscribe(x => entries.Remove(x));
        }


        //
        //  Handlers that feed to the event stream
        //


        private void AddTemplateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AddTemplateStream.OnNext(new Template
            { 
                Description = NewTemplateDescription.Text,
                Points = int.Parse(NewTemplatePoints.Text),
            });
        }


        private void AddEntryButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Template context = XAMLHelper.GetDataContext<Template>(sender as DependencyObject);
            AddEntryStream.OnNext(new Entry
            {
                Description = context.Description,
                Points = context.Points,
            });
        }

        private void DeleteEntryButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Entry context = XAMLHelper.GetDataContext<Entry>(sender as DependencyObject);
            DeleteEntryStream.OnNext(context);
        }
    }
}
