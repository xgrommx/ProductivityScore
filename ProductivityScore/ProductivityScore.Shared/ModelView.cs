using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Windows.UI.Xaml;
using System.Linq;

namespace ProductivityScore.MVVC
{
    /// <summary>
    /// Implementation of Entry
    /// </summary>
    class EntryModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
    }


    class EntryModelView
        : ItemModelView
    {

        private static SQLiteConnection _db;
        internal static SQLiteConnection DB
        {
            get
            {
                if (_db == null)
                {
                    _db = new SQLite.SQLiteConnection((Application.Current as App).DBPath);
                    _db.CreateTable<EntryModel>();
                }
                return _db;
            }
        }


        private int _id;
        internal protected int Id
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


        public override void Save()
        {
            var existing = DB.Table<EntryModel>().Where(c => c.Id == this.Id).SingleOrDefault();

            if (existing == null)
            {
                EntryModel model = new EntryModel
                {
                    Description = Description,
                    Points = Points,
                };
                DB.Insert(model);
                Id = model.Id;
            }
            else
            {
                existing.Description = Description;
                existing.Points = Points;
                DB.Update(existing);
            }
        }

        public override void Delete()
        {
            var existing = DB.Table<EntryModel>().Where(c => c.Id == this.Id).Single();

            DB.Delete(existing); //TODO check rv

            this.Id = 0; // No longer reflected in the database
        }
    }

    class EntriesModelView
        : ItemsModelView<EntryModelView>
    {
        protected override IEnumerable<EntryModelView> LoadAll()
        {
            return EntryModelView.DB.Table<EntryModel>().Select(x =>
                new EntryModelView 
                {
                    Id = x.Id,
                    Description = x.Description,
                    Points = x.Points,
                }
            );
        }
    }
}
