using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using SQLite;
using System.Linq;

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
        public Entries()
        {
            DB.Default.CreateTable<Entry>();
            AddRange(DB.Default.Table<Entry>());

            this.ItemsAdded.Subscribe(x => DB.Default.Insert(x));
            this.ItemsRemoved.Subscribe(x => DB.Default.Delete(x));
        }
    }
}
