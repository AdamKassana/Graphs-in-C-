/* 
 * Alexander Wolf, Adam Kassana, Carmen Tullio
 * October 15, 2023
 * COIS 3020H
 * Assignment 1
*/
// AdjacencyList.cs - This class is used to contain the adjacencyList (Graph) object and will contain it's general methods and properties.

using System.Runtime.InteropServices;

public class AdjacencyList
{
    // A dictionary to store the vertices and their adjacent vertices.
    private readonly Dictionary<int, List<int>> adjacencyList;

    // Also a List to record the vertices separately...
    private List<int> recordVer = new List<int>();

    // a List to record the edges separately...
    private List<int[]> recordEdge = new List<int[]>();

    // and a List to record the weights separately.
    private List<int> recordWei = new List<int>();

    // Constructor, intializes our Adjacency List.
    public AdjacencyList()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    // Getter for recordVer
    public List<int> RecordVer
    {
        get
        {
            return recordVer;
        }
    }
    // Getter for recordEdge
    public List<int[]> RecordEdge
    {
        get
        {
            return recordEdge;
        }
    }
    // Getter for recordWei
    public List<int> RecordWei
    {
        get
        {
            return recordWei;
        }
    }

    // Adds a vertex to the graph. This should be O(1).
    public void AddVertex(int vertex)
    {
        // If the vertex does not exist in the adjacency list, add and record it.
        if (!adjacencyList.ContainsKey(vertex))
        {
            adjacencyList.Add(vertex, new List<int>());
            //Record the addition of our vertex.
            recordVer.Add(vertex);
        }
        else
        {
            throw new ArgumentException(string.Format("The vertex with value {0} already exists, stop it.", vertex));
        }
    }

    // Adds an edge between two vertices in the graph. This should be O(1), however; it depends on how the dictionary ContainsKey method's optimization performs as it is used within GetNeighbors for error checking.
    public void AddEdge(int source, int destination, bool bidirect = false, int weight = 1)
    {
        //Check to ensure our weight is not zero, only non-zero values are permitted.
        if (weight == 0)
        {
            throw new ArgumentException("You cannot have a weight of zero.");
        }
        //To prevent our Kruskal's algorithm from breaking, we must prevent the integer from being set to the max possible value.
        else if (weight == int.MaxValue)
        {
            throw new ArgumentException("Integer value is too large.");
        }
        // Throw an exception if either vertex does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(destination))
        {
            throw new ArgumentException("Vertex does not exist");
        }
        if (source == destination)
        {
            throw new ArgumentException("Cannot make an edge between the same vertex.");
        }
        // The following check will determine if the vertices provided already share an edge. This check unfortunately makes the code less efficient.
        // Initially this seemed like a bad idea, however; we use the boolean for bidrectional edges anyways so there should never be a need to create two monodirectional edges back and fourth. This works!
        if (GetNeighbors(source).Contains(destination)) {
            throw new ArgumentException(string.Format("The source vertex {0} is already neighbors with the destination vertex {1}", source, destination));
        }

        // If the user has decided to go ahead and wanted the edge to be bidirectional, then...
        if (bidirect == true)
        {
            // Add the destination vertex to the source vertex's adjacency list.
            adjacencyList[source].Add(destination);
            // Add the source vertex to the destination vertex's adjacency list.
            adjacencyList[destination].Add(source);
            // Do the same to the edges with the weight...
            recordEdge.Add(new int[] { source, destination, weight });
            recordEdge.Add(new int[] { destination, source, weight });
            // And then record the weights separately
            recordWei.Add(weight);
            recordWei.Add(weight);
        }
        else
        {
            // Otherwise, only add the destination vertex to the source vertex's adjacency list.
            adjacencyList[source].Add(destination);
            // Do the same to the edges with the weight...
            recordEdge.Add(new int[] { source, destination, weight });
            // And then record the weights separately
            recordWei.Add(weight);
        }
    }

    // This method removes a vertex from the graph, along with any associated edges.
    // This method is O(N^2) as there are two foreach loops, one of which is nested, the only method of optimization would be to try and remove the nested foreach loop, likely resulting in O(N log N)
    public void RemoveVertex(int vertex)
    {
        // Throw an exception if the vertex does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(vertex))
        {
            throw new ArgumentException("Vertex does not exist");
        }

        // If there is any neighbors, do the following:
        if (this.GetNeighbors(vertex).Count > 0)
        {
            // For each neighbor of the vertex, remove the vertex from the neighbor's adjacency list, and from the record edge.
            foreach (int neighbor in adjacencyList[vertex])
            {
                adjacencyList[neighbor].Remove(vertex);
                // This unfortunately makes RemoveVertex O(n^2)...
                foreach (int[] edge in recordEdge)
                {
                    if ((edge.Contains(neighbor) && edge.ElementAt(0) == neighbor) && (edge.Contains(vertex) && edge.ElementAt(1) == vertex))
                    {
                        int index = recordEdge.IndexOf(edge); // Gets the index to remove the weight record (since they're parallel lists)
                        recordEdge.Remove(edge);
                        // Also remove it from RecordWei
                        recordWei.RemoveAt(index);
                    }
                }
            }
        }

        // Remove the vertex from the adjacency list and the recordVer, ensuring recordVer remains parallel.
        adjacencyList.Remove(vertex);
        recordVer.Remove(vertex);
    }

    // This method removes an edge between two vertices in the graph. This method is O(N) as there is a foreach loop, separated by an if and else statement, but is used either way.
    public void RemoveEdge(int source, int destination, bool bidirect)
    {
        // Throw an exception if either vertex does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(destination))
        {
            throw new ArgumentException("Vertex does not exist");
        }

        // Throw an exception if the edge does not exist.
        if (!adjacencyList[source].Contains(destination))
        {
            throw new ArgumentException("Edge does not exist");
        }

        // If the user has decided to go ahead and wanted to remove an edge in both directions, then...
        if (bidirect == true)
        {
            // Throw an exception if the opposite edge does not exist.
            if (!adjacencyList[destination].Contains(source))
            {
                throw new ArgumentException("Edge does not exist");
            }

            // Remove the destination vertex from the source vertex's adjacency list.
            adjacencyList[source].Remove(destination);

            // Remove the source vertex from the destination vertex's adjacency list.
            adjacencyList[destination].Remove(source);

            // Also make sure to remove the recordVer of it in the edge and weights recordVer list.
            foreach (int[] edge in recordEdge)
            {
                if ((edge.Contains(source) && edge.ElementAt(0) == source) && (edge.Contains(destination) && edge.ElementAt(1) == destination))
                {
                    int index = recordEdge.IndexOf(edge); // Gets the index to remove the weight record (since they're parallel lists)
                    recordEdge.Remove(edge);
                    recordWei.RemoveAt(index);
                }
                if ((edge.Contains(destination) && edge.ElementAt(0) == destination) && (edge.Contains(source) && edge.ElementAt(1) == source))
                {
                    int index = recordEdge.IndexOf(edge); // Gets the index to remove the weight record (since they're parallel lists)
                    recordEdge.Remove(edge);
                    recordWei.RemoveAt(index);
                }
            }
        }
        else
        {
            // Otherwise, remove the destination vertex from the source vertex's adjacency list.
            adjacencyList[source].Remove(destination);

            // Also make sure to remove the recordVer of it in the edge and weights recordVer list.
            foreach (int[] edge in recordEdge)
            {
                if ((edge.Contains(source) && edge.ElementAt(0) == source) && (edge.Contains(destination) && edge.ElementAt(1) == destination))
                {
                    int index = recordEdge.IndexOf(edge); // Gets the index to remove the weight record (since they're parallel lists)
                    recordEdge.Remove(edge);
                    recordWei.RemoveAt(index);
                }
            }
        }
    }

    // Gets the neighbors of a vertex in the graph (For connectivity). Should be O(1) but once again depends on the Dictionary usage of ContainsKey.
    public List<int> GetNeighbors(int vertex)
    {
        // Throw an exception if the vertex does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(vertex))
        {
            throw new ArgumentException("Vertex does not exist");
        }

        // Return the neighbors of the vertex.
        return adjacencyList[vertex];
    }

    //Create a public property for Count. (This might not be necessary, but it's good practice.) O(1)

    public int Count
    {
        get { return adjacencyList.Count; }
    }

    //Create non-static methods for the following functions O(V+E)
    public void DepthFirstSearch(int start)
    {
        Traversal.DepthFirstSearch(this, start);
    }

    //Wrap the BFS method as a non-static method, O(V+E)
    public string BreadthFirstSearch(int start)
    {
        return Traversal.BreadthFirstSearch(this, start);
    }

    // Does the Cycle Detections through VerConnect, O(2A+V+E)
    public bool CycleDetect(int vertex, bool print = true)
    {
        bool tbr = VerConnect(vertex, vertex, false);
        if (print)
        {
            if (tbr)
                Console.WriteLine("\nThe vertex {0} is cyclic", vertex);
            else
                Console.WriteLine("\nThe vertex {0} is acyclic!", vertex);
        }
        return tbr;
    }

    //Define the shortpath method which uses Dijkstra's algorithm and returs a string with the shortest path between two vertices. O(|E| + |V|log|V|).
    public string ShortPath(int source, int destination)
    {
        // Throw an exception if the either of the vertices does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(destination))
        {
            throw new ArgumentException("Vertex does not exist");
        }
        // Also if there is no path between the two vertices, then return a string saying as much.
        else if (this.VerConnect(source, destination) == false)
        {
            return "\nThe two vertices " + source + " and " + destination + " have no path.";
        }
        return "Starting Vertex: " + source + "\nEnding Vertex: " + destination + Traversal.Dijkstra(this, source, destination);
    }
    //Wrap the Kruskal method from the Traversal class.
    public int MST(bool print = true)
    {
        return Traversal.Kruskal(this, print);
    }

    //This method will provide a boolean repsone if two verticies are connected. O(2A+V+E)
    public bool VerConnect(int source, int destination, bool print = true)
    {
        // Throw an exception if the either of the vertices does not exist in the adjacency list.
        if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(destination))
        {
            throw new ArgumentException("Vertex does not exist");
        }

        // For every neighbor that the source vertex has, check if it's the destination vertex
        foreach (int neighbor in adjacencyList[source])
        {
            if (neighbor == destination)
            {
                // If it is, then output that these vertices are directly connected and return true (aka exit).
                if (print)
                    Console.WriteLine("\nThe Graph between vertices {0} and {1} are directly connected.", source, destination);
                return true;
            }
        }
        // Otherwise loop through BFS until all of the vertices have been visited or the destination vertex is found, returning the result.
        return Traversal.VerIndirConnect(this, source, destination);
    }
}