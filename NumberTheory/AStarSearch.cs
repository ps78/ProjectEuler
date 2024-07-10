using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NumberTheory
{    
    /// <summary>
    /// Signature of a heuristic function that returns a lower boundary to the cost of getting from the given node to the target condition
    /// </summary>    
    public delegate double AStarHeuristicFunction<TNode>(TNode node) where TNode: notnull;

    /// <summary>
    /// Signature of function that tests if the given node is a target of the A* search
    /// </summary>
    public delegate bool AStarIsGoalFunction<TNode>(TNode node) where TNode : notnull;

    /// <summary>
    /// Function that returns the cost of a given path
    /// </summary>
    public delegate double AStarCostFunction<TNode>(TNode node) where TNode : notnull;

    /// <summary>
    /// Returns all neighbors of the given node to be used in the search. Only 'forward-neighbors', not backwards!
    /// Pairs the neighbor node with a relative cost (or distance) to get from node to the neigbor
    /// </summary>
    public delegate List<(double CostToGetThere, TNode Neighbor)> AStarGetNeighborsFunction<TNode>(TNode node) where TNode : notnull;

    /// <summary>
    /// A path consisting of nodes of the given type in context of A* search
    /// </summary>
    public class AStarPath<TNode> where TNode : notnull
    {
        #region Fields

        private readonly List<TNode> nodes = new List<TNode>();
        private readonly AStarCostFunction<TNode> costFunction = (n) => 0;

        #endregion
        #region Properties

        /// <summary>
        /// Total cost of the path (according to the given cost function)
        /// </summary>
        public double Cost { get; private set; }

        /// <summary>
        /// Total length (number of nodes) of the path
        /// </summary>
        public int Length => nodes.Count;

        #endregion
        #region Constructors

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public AStarPath(AStarPath<TNode> copyFrom)
        {
            this.nodes.AddRange(copyFrom.nodes);
            this.costFunction = copyFrom.costFunction;
            this.Cost = copyFrom.Cost;
        }

        public AStarPath(AStarPath<TNode> copyFrom, TNode nodeToAdd) : this(copyFrom)
        {
            Append(nodeToAdd);
        }

        public AStarPath(AStarCostFunction<TNode> costFunc)
        {
            if (costFunc == null)
                throw new NullReferenceException("The cost function must not be null");
        }
        
        #endregion
        #region Public Methods

        /// <summary>
        /// Appends a node to the end of th path
        /// </summary>
        public void Append(TNode node)
        {
            nodes.Add(node);
            Cost += costFunction(node);
        }

        /// <summary>
        /// Removes the last node from the path
        /// </summary>
        public void RemoveLast()
        {
            if (nodes.Count == 0)
                return;
            Cost -= costFunction(nodes[nodes.Count - 1]);
            nodes.RemoveAt(nodes.Count - 1);
        }

        #endregion
    }

    public class AStarSearch<TNode> where TNode : notnull
    {
        #region Fields

        private readonly AStarHeuristicFunction<TNode> heuristicFunction;
        private readonly AStarIsGoalFunction<TNode> isGoalFunction;
        private readonly AStarCostFunction<TNode> costFunction;
        private readonly AStarGetNeighborsFunction<TNode> getNeighborsFunction;

        #endregion
        #region Properties

        /// <summary>
        /// Number of iterations the main while-loop in the Search did in the last call
        /// </summary>
        public ulong IterationCount { get; private set; }

        #endregion
        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public AStarSearch(AStarHeuristicFunction<TNode> heuristicFunc, AStarIsGoalFunction<TNode> isGoalFunc, AStarCostFunction<TNode> costFunc, AStarGetNeighborsFunction<TNode> getNeighborsFunc)
        {
            if (heuristicFunc == null || isGoalFunc == null || costFunc == null || getNeighborsFunc == null)
                throw new NullReferenceException("The A* search requires all 4 functions to be provided: Heuristic, Cost, IsGoal and getNeighbors");

            this.heuristicFunction = heuristicFunc;
            this.isGoalFunction = isGoalFunc;
            this.costFunction = costFunc;
            this.getNeighborsFunction = getNeighborsFunc;
        }

        /// <summary>
        /// Searches all optimal solutions, i.e. one with minimal cost
        /// 
        /// the function minimizes g(node) along the path, where g(node) is the sum of the cost from the start to that node
        /// to speed up search, h(node) is a heuristic function indicating the lower boundary of the total additional cost
        /// to get from that node to the goal state.
        /// We define the f-score of a node as the sum of the actual cost and the minmal estimated cost to the goal:
        ///     f(node) = g(node) + h(node)
        /// </summary>
        public IEnumerable<List<TNode>> Search(IEnumerable<TNode> startNodes)
        {
            IterationCount = 0;

            // set of nodes already evaluated
            var closedSet = new List<TNode>();

            // set of nodes not yet evaluated
            var openSet = new PriorityQueue<TNode>(PriorityQueue<TNode>.PriorityQueueOrder.Ascending);            

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<TNode, TNode>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<TNode, double>();

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<TNode, double>();

            // cache for the calculated heuristics
            var hScore = new Dictionary<TNode, double>();

            // initialize with the start nodes
            foreach (var node in startNodes)
            {                
                gScore.Add(node, costFunction(node));
                hScore.Add(node, heuristicFunction(node));
                fScore.Add(node, gScore[node] + hScore[node]);
                openSet.Queue(fScore[node], node);
            }

            while (!openSet.Empty)
            {
                IterationCount++;

                // return the node with the lowest f-score
                var current = openSet.Dequeue().Item;
                closedSet.Add(current);

                // check if we found a solution and return them if it is so
                if (isGoalFunction(current))
                    yield return ReconstructPath(cameFrom, current);

                // expand path to the neighbors of the current node
                foreach (var neighbor in getNeighborsFunction(current))
                {
                    // ignore neighbor, if it was already evaluated
                    if (closedSet.Contains(neighbor.Neighbor))
                        continue;

                    // The distance from start to a neighbor
                    double tentative_gScore = gScore[current] + neighbor.CostToGetThere;

                    // discovered a new node
                    if (!openSet.Contains(neighbor.Neighbor))
                    {
                        hScore[neighbor.Neighbor] = heuristicFunction(neighbor.Neighbor);
                        openSet.Queue(tentative_gScore + hScore[neighbor.Neighbor], neighbor.Neighbor);
                        gScore[neighbor.Neighbor] = double.MaxValue;
                        fScore[neighbor.Neighbor] = double.MaxValue;
                    }

                    // This is not a better path.
                    if (tentative_gScore >= gScore[neighbor.Neighbor])
                        continue;

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Neighbor] = current;
                    gScore[neighbor.Neighbor] = tentative_gScore;
                    fScore[neighbor.Neighbor] = tentative_gScore + hScore[neighbor.Neighbor];
                }
            }
            
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Reconstructs all optimal paths to the Current-node
        /// </summary>
        private List<TNode> ReconstructPath(Dictionary<TNode, TNode> CameFrom, TNode Current)
        {
            var path = new List<TNode>(new TNode[] { Current });
            while (CameFrom.ContainsKey(Current))
            {
                Current = CameFrom[Current];
                path.Insert(0, Current);
            }
            return path;
        }

        #endregion
    }
    
}
