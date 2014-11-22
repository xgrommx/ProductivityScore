using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Linq;

namespace ProductivityScore.Utils
{
    /// <summary>
    /// This represents a single value that is derived from a collection.
    /// The value is updated when the collection is updated and this object notifies of the change.
    /// </summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    /// <typeparam name="R">The type of the derived value</typeparam>
    class DerivedObservableProperty<T, R>
        : Data
    {
        private ReactiveList<T> source;
        private Func<ReactiveList<T>, R> produce;

        private R _value;
        /// <summary>
        /// The latest produced value
        /// </summary>
        public R Value
        {
            get { return _value; }
            private set { SetField<R>(ref _value, value); }
        }


        /// <summary>
        /// This represents a single value that is derived from a collection.
        /// The value is updated when the collection is updated and this object notifies of the change.
        /// </summary>
        /// <param name="source">The collection from which the value is produced</param>
        /// <param name="produce">The method that produces the value</param>
        public DerivedObservableProperty(ReactiveList<T> source, Func<ReactiveList<T>, R> produce)
        {
            this.source = source;
            this.produce = produce;

            source.CollectionChanged += (x,y) => Value = produce(source);

            this.Value = produce(source);
        }
    }


    class ReactiveView<TView, TSource>
        : ReactiveList<TSource>
    {
        private ReactiveList<TView> source;
        private Func<TView, TSource> sourceToLocal;
        private Func<TSource, TView> localToSource;


        /// <summary>
        /// Represents a view of a ReactiveCollection.
        /// The items in the view are mapped one for one on the source collection.
        /// Changes in either collection are reflected in the other.
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="sourceToLocal">The way to produce a view object from the source</param>
        /// <param name="localToSource">The way to produce a source object from the view</param>
        public ReactiveView(ReactiveList<TView> source,
                            Func<TView,TSource> sourceToLocal,
                            Func<TSource,TView> localToSource = null)
        {
            this.source = source;
            this.sourceToLocal = sourceToLocal;
            this.localToSource = localToSource;

            base.AddRange(source.Select(sourceToLocal));

            source.ItemsAdded.Subscribe(x => base.Add(sourceToLocal(x)));
            source.ItemsRemoved.Subscribe(x => base.Remove(sourceToLocal(x)));
            source.ItemsMoved.Subscribe(x => base.Move(x.From, x.To));
            //TODO on item changed
        }


        /// <summary>
        /// Adds an item to the collection. The source of the view is updated first, then the view.
        /// </summary>
        /// <param name="item">The item to be added</param>
        public new void Add(TSource item)
        {
            source.Add(localToSource(item));
        }


        /// <summary>
        /// Add a range of items to the collection. The source of the view is updated first, then the view.
        /// </summary>
        /// <param name="collection">The items to be added</param>
        public new void AddRange(IEnumerable<TSource> collection)
        {
            source.AddRange(collection.Select(localToSource));
        }


        /// <summary>
        /// Removes an item from the collection. The source of the view is updated first, then the view.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        public new bool Remove(TSource item)
        {
            return source.Remove(localToSource(item));
        }


        /// <summary>
        /// Moves an item inside this list. The source of the view is updated first, then the view.
        /// </summary>
        /// <param name="oldIndex">The old position</param>
        /// <param name="newIndex">The new position</param>
        public new void Move(int oldIndex, int newIndex)
        {
            source.Move(oldIndex, newIndex);
        }
    }


    static class ReactiveViewExtensions
    {
        public static ReactiveView<TSource, TView> CreateView<TSource, TView>(
            this ReactiveList<TSource> source,
            Func<TSource, TView> sourceToView)
        {
            return new ReactiveView<TSource, TView>(source, sourceToView);
        }

        public static ReactiveView<TSource, TView> CreateView<TSource, TView>(
            this ReactiveList<TSource> source,
            Func<TSource, TView> sourceToView,
            Func<TView, TSource> viewToSource)
        {
            return new ReactiveView<TSource, TView>(source, sourceToView, viewToSource);
        }


        public static DerivedObservableProperty<TSource, TView> DeriveObservableProperty<TSource, TView>(
            this ReactiveList<TSource> source,
            Func<ReactiveList<TSource>, TView> derivation)
        {
            return new DerivedObservableProperty<TSource, TView>(source, derivation);
        }
    }

}
