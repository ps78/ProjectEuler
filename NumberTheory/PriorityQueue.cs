using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    /// <summary>
    /// A queue of items ordered by priority
    /// </summary>
    public class PriorityQueue<T>
    {
        #region Types

        public enum PriorityQueueOrder
        {
            /// <summary>
            /// High priority = low values. i.e. small values come first
            /// </summary>
            Ascending,
            /// <summary>
            /// High priority = high values. i.e. large values come first
            /// </summary>
            Descending
        }

        #endregion
        #region Fields

        // the items are ordered to have min priority at index 0 and max at the end. i.e. Dequeue will return the last element
        private readonly List<(double Priority, T Item)> items = new List<(double, T)>();

        /// <summary>
        /// The priority of the queue when it is empty
        /// </summary>
        private readonly double DefaultPriority;

        #endregion
        #region Properties

        /// <summary>
        /// True if queue is empty
        /// </summary>
        public bool Empty => items.Count == 0;

        /// <summary>
        /// Number of elements in the queue
        /// </summary>
        public int Length => items.Count;

        /// <summary>
        /// Returns the priority value of the highest prio item (smallest value if order is ascending, largest value if order is descending)
        /// </summary>
        public double MaxPriority => Length == 0 ? DefaultPriority : items[Length - 1].Priority;

        /// <summary>
        /// Returns the priority value of the lowest prio item (largest value if order is ascending, smallest value if order is descending)
        /// </summary>
        public double MinPriority => Length == 0 ? DefaultPriority : items[0].Priority;

        /// <summary>
        /// Order of the queue.
        ///     Ascending (default): small priority values mean high priority, such elements come first.
        ///     Descending         : large priority values mean high priority, such elements come first.
        /// </summary>
        public PriorityQueueOrder Order { get; }

        #endregion
        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue(PriorityQueueOrder order = PriorityQueueOrder.Ascending)
        {
            this.Order = order;
            this.DefaultPriority = (order == PriorityQueueOrder.Ascending ? double.MinValue : double.MaxValue);
        }

        /// <summary>
        /// Add an element to the queue
        /// </summary>
        public void Queue(double Priority, T Item)
        {
            if (Length == 0)
                items.Add((Priority, Item));
            else
            {
                if (Order == PriorityQueueOrder.Ascending)
                {
                    int idx = 0;
                    while (idx < items.Count && Priority < items[idx].Priority) idx++;
                    items.Insert(idx, (Priority, Item));
                }
                else if (Order == PriorityQueueOrder.Descending)
                {
                    int idx = 0;
                    while (idx < items.Count && Priority > items[idx].Priority) idx++;
                    items.Insert(idx, (Priority, Item));
                }
            }
        }

        /// <summary>
        /// Returns the element with highest priority, without removing it from the queue
        /// </summary>
        public (double Priority, T Item) Peek()
        {
            if (Length == 0)
                throw new InvalidOperationException("Queue is empty, cannot execute Peek()");

            return items[Length - 1];
        }

        /// <summary>
        /// Returns the element with highest priority, removing it from the queue
        /// </summary>
        public (double Priority, T Item) Dequeue()
        {
            if (Length == 0)
                throw new InvalidOperationException("Queue is empty, cannot execute Peek()");

            var item = items[Length - 1];
            items.RemoveAt(Length - 1);
            return item;
        }

        /// <summary>
        /// Removes all items from the queue that have a priority lower than the given limit (but not equal)
        /// </summary>
        public void TruncateLowPriorityItems(double PriorityLimit)
        {
            if (Length == 0)
                return;

            int idx = -1;
            if (Order == PriorityQueueOrder.Ascending)
                while (idx < items.Count && items[idx].Priority >= PriorityLimit) idx++;
            else if (Order == PriorityQueueOrder.Descending)
                while (idx < items.Count && items[idx].Priority <= PriorityLimit) idx++;

            if (idx < items.Count)
                items.RemoveRange(idx, items.Count - idx);
        }

        public bool Contains(T Item)
        {
            foreach (var it in items)
                if (it.Item != null && it.Item.Equals(Item))
                    return true;
            return false;
        }

        #endregion
    }
}
