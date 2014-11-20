using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using SQLite;
using System.Linq;
using System.Diagnostics;

namespace ProductivityScore.Model
{
    class Entry
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }

        public override string ToString()
        {
            return "[" + Description + ": " + Points + "]";
        }
    }

    class Entries
        : ReactiveList<Entry>
    {
        private static Entries _singleton;
        public static Entries Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = new Entries();
                return _singleton;
            }
        }

        private Entries()
        {
            DB.Default.CreateTable<Entry>();
            AddRange(DB.Default.Table<Entry>());
            Debug.WriteLine("Loaded " + Count + " " + GetType().Name);

            this.ItemsAdded.Subscribe(x => DB.Default.Insert(x));
            this.ItemsRemoved.Subscribe(x => DB.Default.Delete(x));
        }
    }
}
