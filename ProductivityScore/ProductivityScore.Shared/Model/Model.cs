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
        public DateTime Date { get; set; }

        public Entry()
        {
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return "[" + Description + ": " + Points + "]";
        }
    }


    class Template
        : Entry
    {

    }


    class ModelList<T>
        : ReactiveList<T>
        where T: new()
    {
        public ModelList()
        {
            DB.Default.CreateTable<T>();
            AddRange(DB.Default.Table<T>());

            Debug.WriteLine("Loaded " + Count + " " + GetType().Name + "(" + typeof(T).Name + ")");

            this.ItemsAdded.Subscribe(x => DB.Default.Insert(x));
            this.ItemsRemoved.Subscribe(x => DB.Default.Delete(x));
        }
    }
}
