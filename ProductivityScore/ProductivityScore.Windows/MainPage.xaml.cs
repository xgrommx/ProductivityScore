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
using ReactiveUI;
using System.Reflection;
using System.Diagnostics;

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
        ModelList<Bounty> bounties = new ModelList<Bounty>();

        Subject<Template> AddTemplateStream = new Subject<Template>();
        Subject<Entry> AddEntryStream = new Subject<Entry>();
        Subject<Entry> DeleteEntryStream = new Subject<Entry>();
        Subject<Bounty> CompleteBountyStream = new Subject<Bounty>();
        Subject<Bounty> AddBountyStream = new Subject<Bounty>();


        public MainPage()
        {
            this.InitializeComponent();

            var entriesView = entries.CreateView(
                x => new
                {
                    Source = x,
                    Description = x.Description,
                    Points = x.Points,
                    Date = x.Date.ToString("dd MMM"),
                },
                x => x.Source
            );

            HistoryItems.ItemsSource = entriesView;
            TemplateItems.ItemsSource = templates;
            BountyItems.ItemsSource = bounties;

            TotalScore.DataContext = entries.DeriveObservableProperty(es => es.Sum(e => e.Points));
            TotalScoreToday.DataContext = entries.DeriveObservableProperty(es => 
                es.Where(e => e.Date.Date.Equals(DateTime.Now.Date))
                  .Sum(e => e.Points));

            AddTemplateStream.Subscribe(x => templates.Add(x));
            AddEntryStream.Subscribe(x => entries.Add(x));

            DeleteEntryStream.Subscribe(x => entries.Remove(x));

            AddBountyStream.Subscribe(bounty => bounties.Add(bounty));
            CompleteBountyStream.Subscribe(bounty =>
            {
                entries.Add(new Entry
                {
                    Points = bounty.Points,
                    Description = bounty.Description,
                });
                bounties.Remove(bounty);
            });
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

        private void AddBountyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AddBountyStream.OnNext(new Bounty
            {
                Description = NewBountyDescription.Text,
                Points = int.Parse(NewBountyPoints.Text),
            });
        }

        private void AddEntryButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Template context = XAMLHelper.GetDataContext<Template>(sender as DependencyObject);
            if (context != null)
                AddEntryStream.OnNext(new Entry
                {
                    Description = context.Description,
                    Points = context.Points,
                });
            else
                AddEntryStream.OnNext(new Entry
                {
                    Description = NewEntryDescription.Text,
                    Points = int.Parse(NewEntryPoints.Text),
                });
        }


        private void DeleteEntryButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Object context = XAMLHelper.GetDataContext<Object>(sender as DependencyObject);
            Entry entry = context.GetField<Entry>("Source");
            DeleteEntryStream.OnNext(entry);
        }

        private void BountyCompletedButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Bounty context = XAMLHelper.GetDataContext<Bounty>(sender);
            CompleteBountyStream.OnNext(context);
        }
    }


    static class ReflectiveExtension
    {
        public static T GetField<T>(this Object o, string field)
        {
            var v = o.GetType().GetRuntimeProperty(field).GetValue(o);
            if (v is T) return (T)v;
            throw new KeyNotFoundException(field);
        }
    }
}
