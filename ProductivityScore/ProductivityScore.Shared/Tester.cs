using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ProductivityScore.MVVC;

namespace ProductivityScore
{
    static class Tester
    {

        public static void Test()
        {
            Debug.WriteLine("Testing");

            var entries = new EntriesModelView();
            entries.ItemsAdded.Subscribe(x => Debug.WriteLine("Added " + x.Description));
            entries.ItemsRemoved.Subscribe(x => Debug.WriteLine("Removed " + x.Description));

            foreach (EntryModelView entry in entries)
                Debug.WriteLine("Existing: " + entry);

            entries.Add(new EntryModelView { Description = "Desc1", Points = 100 });
            entries.Add(new EntryModelView { Description = "Desc2", Points = 200 });
            entries.Add(new EntryModelView { Description = "Desc3", Points = 300 });
            entries.Remove(entries[1]);
        }
    }
}
