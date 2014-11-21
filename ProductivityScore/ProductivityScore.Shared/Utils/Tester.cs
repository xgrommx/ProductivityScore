using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ProductivityScore.Model;

namespace ProductivityScore
{
    static class Tester
    {

        public static void Test()
        {
            //Test1();
        }

        //private static void Test1()
        //{
        //    Debug.WriteLine("Testing");

        //    var entries = Entries.Singleton;
        //    entries.ItemsAdded.Subscribe(x => Debug.WriteLine("Added " + x.Description));
        //    entries.ItemsRemoved.Subscribe(x => Debug.WriteLine("Removed " + x.Description));

        //    foreach (Entry entry in entries)
        //        Debug.WriteLine("Existing: " + entry);

        //    entries.Add(new Entry { Description = "Desc1", Points = 100 });
        //    entries.Add(new Entry { Description = "Desc2", Points = 200 });
        //    entries.Add(new Entry { Description = "Desc3", Points = 300 });
        //    entries.Remove(entries[1]);

        //    Debug.WriteLine("Final: ");
        //    foreach (Entry entry in entries)
        //        Debug.WriteLine("Existing: " + entry);
        //}
    }
}
