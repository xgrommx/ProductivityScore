using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Windows.UI.Xaml;


namespace ProductivityScore
{
    public class DB
    {
        private static SQLiteConnection _db;
        internal static SQLiteConnection Default
        {
            get
            {
                if (_db == null)
                {
                    _db = new SQLite.SQLiteConnection((Application.Current as App).DBPath);
                }
                return _db;
            }
        }
    }
}
