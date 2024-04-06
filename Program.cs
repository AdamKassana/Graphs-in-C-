/* 
 * Alexander Wolf, Adam Kassana, Carmen Tullio
 * October 15, 2023
 * COIS 3020H
 * Assignment 1
*/
// Program.cs - This class will contain our main method as well as the code used for testing our Adjacency List and Traversal methods.

using System;

class Program
{
    public static void Main(string[] args)
    {
        //Catch the errors that were being thrown.
        try
        {
            Testing(args);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //Print out the stacktrace in red so it catches our attention.
            Console.WriteLine(e.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(69420);
        }
        
    }

    public static void Testing(string[] args)
    {
        AdjacencyList list = new AdjacencyList();
        list.AddVertex(1);
        list.AddVertex(2);
        //list.AddVertex(2); //Use this to test duplicate vertices.
        list.AddVertex(3);
        list.AddVertex(4);
        list.AddVertex(5);
        list.AddVertex(6);
        list.AddVertex(7);
        list.AddEdge(1, 2, true, 2);
        //list.AddEdge(1, 2, true, 2); //Use this to test duplicate edges.
        //list.AddEdge(2, 1, true, 2); //Use this to test duplicate edges.
        list.AddEdge(1, 3, true, 3);
        //list.AddEdge(3, 3, true, 3); //Use this to test the duplicate vertex edge error.
        list.AddEdge(1, 4, true, 3);
        list.AddEdge(2, 3, true, 4);
        list.AddEdge(2, 5, true, 3);
        list.AddEdge(3, 4, true, 5);
        list.AddEdge(3, 5, true, 1);
        list.AddEdge(4, 6, true, 7);
        list.AddEdge(5, 6, true, 8);
        list.AddEdge(6, 7, true, 9);


        list.DepthFirstSearch(1);
        Console.WriteLine(list.BreadthFirstSearch(1));

        /*
        list.VerConnect(2, 4);
        list.VerConnect(4, 2);
        list.VerConnect(3, 3);
        */
        Console.WriteLine(list.ShortPath(4, 2));
        Console.WriteLine(list.ShortPath(1, 4));
        Console.WriteLine(list.ShortPath(1, 7));


        int listMinWeight = list.MST();

        AdjacencyList list2 = new AdjacencyList();

        list2.AddVertex(1);
        list2.AddVertex(2);
        list2.AddVertex(3);
        list2.AddVertex(4);
        list2.AddEdge(1, 2, true, 1);
        list2.AddEdge(2, 4, true, 1);
        list2.AddEdge(2, 3, true, 2);
        list2.MST();

        list.CycleDetect(4);
        list.CycleDetect(3);
    }
}