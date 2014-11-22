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

        public ReactiveView(ReactiveList<TView> source,
                            Func<TView,TSource> sourceToLocal,
                            Func<TSource,TView> localToSource = null)
        {
            this.source = source;
            this.sourceToLocal = sourceToLocal;
            this.localToSource = localToSource;

            AddRange(source.Select(sourceToLocal));

            source.ItemsAdded.Subscribe(x => base.Add(sourceToLocal(x)));
            source.ItemsRemoved.Subscribe(x => base.Remove(sourceToLocal(x)));
            source.ItemsMoved.Subscribe(x => base.Move(x.From, x.To));
            //TODO on item changed
        }

        public override void Add(TSource item)
        {
            source.Add(localToSource(item));
        }

        public override bool Remove(TSource item)
        {
            return source.Remove(localToSource(item));
        }

        public override void Move(int oldIndex, int newIndex)
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
