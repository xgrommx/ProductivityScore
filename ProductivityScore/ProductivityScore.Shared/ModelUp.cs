using ProductivityScore.Utils;
using ReactiveUI;
using SQLite;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using ProductivityScore.MVVC;
using System.Linq;



namespace ProductivityScore.MVVC
{
    abstract class ItemModelView
        : Data
    {
        public abstract void Save();
        public abstract void Delete();

    }


    abstract class ItemsModelView<T>
        : ReactiveList<T>
        where T: ItemModelView
    {
        /// <summary>
        /// 
        /// </summary>
        public ItemsModelView()
            : base()
        {
            base.AddRange(LoadAll());

            ItemsAdded.Subscribe(x => x.Save());
            ItemsRemoved.Subscribe(x => x.Delete());
        }


        /// <summary>
        /// Get every instance
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<T> LoadAll();
    }

}
