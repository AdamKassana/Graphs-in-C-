/* 
 * Alexander Wolf, Adam Kassana, Carmen Tullio
 * October 15, 2023
 * COIS 3020H
 * Assignment 1
*/
// Traversal.cs - This class will contain all needed traversal methods to be used with our Adjacency List, and includes algorithms such as Kruskal, Dijkstra, Breadth First and Depth First searches.

using System.Diagnostics;

public class Traversal
{
    // Creates an instance of a boolean array, and initializes it.
    private static bool[] visited = { false };

    //Define the Breadth First Search method, takes an adjacencylist and prints out values in a breadth first order. O(V+E) due to the while and nested foreach loop.
    public static string BreadthFirstSearch(AdjacencyList adj, int start)
    {
        string tbr = string.Format("\nBeginning BreadthFirstSearch from index: {0}.\n", start); //Initialize the string to be returned.
        //Redefine our visited boolean array to the appropriate length
        visited = new bool[adj.Count];
        //Create a queue to track which vertex we are currently visiting
        Queue<int> q = new Queue<int>();
        //Set our current index to have been visited.
        visited[adj.RecordVer.IndexOf(start)] = true;
        //Enqueue our current vertex into our queue.
        q.Enqueue(start);

        //While our queue has any elements, we should check for neighbors and queue them.
        while (q.Any())
        {
            //Obtain the value of the next element in the queue.
            int vertex = q.Dequeue();
            tbr = tbr + string.Format("\nVisited: " + vertex);
            //Obtain the neighbors of the current vertex.
            List<int> neighbors = adj.GetNeighbors(vertex);
            //For every neighbor, we should check if it has previously been visited
            //If not, we should visit them.
            foreach (int neighbor in neighbors)
            {
                if (!visited[adj.RecordVer.IndexOf(neighbor)])
                {
                    //Indicate in our array that the current neighbor has been visited.
                    visited[adj.RecordVer.IndexOf(neighbor)] = true;
                    //Enqueue the neighbor.
                    q.Enqueue(neighbor);
                }
            }
        }
        return tbr;
    }

    //Define the DepthFirstSearch method which will traverse an adjacency list in a depth first manner. As this is a recursive function, additionally with a foreach loop, O(V+E)
    public static void DepthFirstSearch(AdjacencyList adj, int start)
    {
        //Initialize the depth first search by setting our start vertex as visited in our boolean array.
        Console.WriteLine("\nBeginning DepthFirstSearch from index: {0}.\n", start);
        visited = new bool[adj.Count];
        //Begin calling our recursive DFS method.
        DFSRecursive(adj, start);
    }

    //Define our DFSRecursive method which will take an adjacency list and a vertex to navigate through all neighbors.
    private static void DFSRecursive(AdjacencyList adj, int vertex)
    {
        // Checks off the current vertex as being true for visited and outputs it to the console.
        visited[adj.RecordVer.IndexOf(vertex)] = true;
        Console.WriteLine("Visited: " + vertex);

        // Makes a list of all the neighbors that the vertex has.
        List<int> neighbors = adj.GetNeighbors(vertex);
        foreach (int neighbor in neighbors)
        {
            // If the vertex hasn't been visited...
            if (!visited[adj.RecordVer.IndexOf(neighbor)])
            {
                // Continue our search by recursively calling out method again.
                DFSRecursive(adj, neighbor);
            }
        }
    }

    // For if two vertices are not directly connected. Due to the foreach loop and the while with Nested foreach loop O(A+V+E), where A represents the quantity of neighbors.
    public static bool VerIndirConnect(AdjacencyList adj, int start, int destination)
    {
        // First, check if the start isn't connected to the destination
        List<int> startnei = adj.GetNeighbors(start);
        foreach (int neighbor in startnei)
        {
            if (neighbor == destination)
            {
                Debug.WriteLine("The vertices are directly connected, meaning that it is not indirectly connected.");
                return false;
            }
        }
        
        // Create a bool to check if it is indirectly connected...
        bool indir = false;

        // And loop through BFS until all of the vertices have been visited or the destination vertex is found.

        /* v MODIFIED BFS BELOW v */

        // Builds the queue and the visited boolean array.
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[adj.Count];

        // Enqueues and checks off the inputted value as true in the array.
        queue.Enqueue(start);
        visited[adj.RecordVer.IndexOf(start)] = true;

        // While the queue isn't empty and there is no indirect connection, do the following.
        while (queue.Count > 0 && indir == false)
        {
            // Dequeue the current vertex.
            int current = queue.Dequeue();

            // Makes a list of all the neighbors that the vertex has.
            List<int> neighbors = adj.GetNeighbors(current);

            // For each neighbor, do the following:
            foreach (var neighbor in neighbors)
            {
                // Check if the neighbor is the destination vertex.
                if (neighbor == destination)
                {
                    // If it is, then output that these vertices are indirectly connected and mark indir as true.
                    Console.WriteLine("\nThe Graph between vertices {0} and {1} are indirectly connected.", start, destination);
                    indir = true;
                }
                // Otherwise, check if the vertex hasn't been visited...
                else if (!visited[adj.RecordVer.IndexOf(neighbor)])
                {
                    // If it is, enqueue it and check it off as true.
                    queue.Enqueue(neighbor);
                    visited[adj.RecordVer.IndexOf(neighbor)] = true;
                }
                // Otherwise, move on to the next neighbor (if there is one)
            }
        }
        // If indir has been marked off as true, return true, otherwise output that the two vertices aren't connected and return false.
        if (indir == true)
        {
            return true;
        }
        else
        {
            Console.WriteLine("\nThe Graph between vertices {0} and {1} are not connected.", start, destination);
            return false;
        }
    }

    // Dijkstra's Algorithm: Using a modified form of BFS to find the shortest path from a vertex to another vertex
    // Due to the multiple nested forloops and while loop, the time complexity should be O(|E| + |V|log|V|)
    // Thanks to https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/ for the help with this.
    public static string Dijkstra(AdjacencyList adj, int start, int finish)
    {
        // Make an array for all of the distances, and another for the string paths
        int[] distance = new int[adj.Count];
        string[] path = new string[adj.Count];

        // Then fill the arrays, the int array with the max number it can take, and the string array with "".
        for (int i = 0; i < adj.Count; i++)
        {
            distance[i] = int.MaxValue;
            path[i] = "";
        }
        
        // Builds the queue and the visited boolean array.
        Queue<int> queue = new Queue<int>();
        visited = new bool[adj.Count];

        // Enqueues the inputted value.
        queue.Enqueue(start);
        
        // Also mark off the first indecies of distance and path with the correct data.
        distance[adj.RecordVer.IndexOf(start)] = 0;
        path[adj.RecordVer.IndexOf(start)] = "" + start;

        // While the queue isn't empty, do the following.
        while (queue.Count > 0)
        {
            // Sort the queue
            queue.OrderBy(x => x).Reverse();
            // Dequeue the current vertex.
            int current = queue.Dequeue();
            visited[adj.RecordVer.IndexOf(current)] = true;

            // Makes a list of all the neighbors that the vertex has.
            List<int> neighbors = adj.GetNeighbors(current);

            // For each neighbor, do the following: 
            foreach (var neighbor in neighbors)
            {
                // Make an int for weight.
                int weight = 0;
                // Get the correct weight from the foreach loop
                foreach (var edge in adj.RecordEdge)
                {
                    if (edge[0] == current && edge[1] == neighbor)
                    {
                        weight = edge[2];
                    }
                }
                // If the distance of the neighbor is shorter is shorter with the current vertex's distance with the weight...
                if (distance[adj.RecordVer.IndexOf(neighbor)] > distance[adj.RecordVer.IndexOf(current)] + weight)
                {
                    // Store the distance and the path, and enqueue the neighbor.
                    distance[adj.RecordVer.IndexOf(neighbor)] = distance[adj.RecordVer.IndexOf(current)] + weight;
                    path[adj.RecordVer.IndexOf(neighbor)] = path[adj.RecordVer.IndexOf(current)] + ", " + neighbor;
                    queue.Enqueue(neighbor);
                }
            }
        }
        // From here, return the path and the distance of the finishing vertex
        return "\nDistance: " + distance[adj.RecordVer.IndexOf(finish)] + "\nPath: " + path[adj.RecordVer.IndexOf(finish)];
    }

    //Kruskal's Algorithm, returns the min weight/min spanning tree and prints all connections between nodes.
    // Due the foreach loop and the while loop, the timecomplexity should be roughly O(E log E)
    // Thanks to https://www.geeksforgeeks.org/kruskals-minimum-spanning-tree-algorithm-greedy-algo-2/ for getting us started and for research.
    public static int Kruskal(AdjacencyList adj, bool print = true)
    {
        //Make a new list as to not cause funkiness since it's passed as reference.
        List<int> traversedWei = new List<int>(adj.RecordWei);
        List<int[]> edgeList = adj.RecordEdge; //Import our edgelist to make things easier. (Note it is passed as reference.)
        int minWeight = 0;
        visited = new bool[adj.Count];
        int Count = 0; //Note that a Minimum Spanning Tree will always contain V-1 edges, so we must check that the tree contains this number, otherwise include the next following edge.
        if (print) { Console.WriteLine("\nStarting Kruskal's Algorithm to calculate the MST and MinWeight\n"); }

        for (int i = 0; i < traversedWei.Count; i++)
        {
            int weight = traversedWei.Min();

            //Obtain our two iterative vertices based on the index from the traversed weights.
            int vertex1 = adj.RecordVer.IndexOf(edgeList[traversedWei.IndexOf(weight)][0]);
            int vertex2 = adj.RecordVer.IndexOf(edgeList[traversedWei.IndexOf(weight)][1]);

            //We can obtain the connected vertices from MinWeight by directly calling the edgelist at the equivalent index.
            //See the following debug logs to retrieve the following edges from the weights accordingly.

            //Debug.WriteLine("The current weight being tested is {0} and the edges connecting this weight are {1} and {2}", weight, adj.RecordVer[vertex1], adj.RecordVer[vertex2]);

            

            //Check and see if both vertices have yet to have been discovered.
            if (!(visited[vertex1] && visited[vertex2]))
            {
                //Check if we are printing to the console.
                if (print)
                    Console.WriteLine("The current weight being added is {0} which is between edges {1} and {2}", weight, adj.RecordVer[vertex1], adj.RecordVer[vertex2]);
                //Increment the weight of the MST.
                minWeight += weight;
                //By setting the current value to the highest possible int value, we will prevent the algorithm from finding the same minimum value twice.
                

                //Discover both vertices.
                visited[vertex1] = true;
                visited[vertex2] = true;
                //Increase the value of the count.
                Count++;
            }
            traversedWei[traversedWei.IndexOf(weight)] = int.MaxValue; //Set to the maximum integer value because i'm lazy (don't set the weight to this please.) We will need to test for this. Done.
        }
        //We must also check for the condition that all nodes are discovered and are all interconnected, as we might get independant loops.
        //Fortunately, a MST will also always contain the amount of edges which is equal to the amount of vertices minus one.
        while (Count < adj.RecordVer.Count - 1)
        {
            //The Minimum spanning tree does not link all verticies, include the next lowest edge.
            //Reset our weight array back to normal
            traversedWei = new List<int>(adj.RecordWei);
            //Sort our weighted array
            traversedWei.Sort();
            //Increase the weight with the next available lowest weight, thereby bridging any split trees.
            minWeight += traversedWei[Count + 1];
            if (print)
                Console.WriteLine("The current weight being added is {0} which is between edges {1} and {2}", traversedWei[Count + 1], adj.RecordEdge[adj.RecordWei.LastIndexOf(traversedWei[Count + 1])][0], adj.RecordEdge[adj.RecordWei.LastIndexOf(traversedWei[Count + 1])][1]);
            //Increase the count in case we need to once again bridge more vertices.
            Count++;
        }
        if (print) { Console.WriteLine("\nThe final MinWeight of the MST was calculated as {0}\n", minWeight); }
        return minWeight;
    }
}
