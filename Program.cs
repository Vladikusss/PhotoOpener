/*
 * Created by Vladdy | 30.03.2025
   * Last Updated: 30.03.2025
   * Program: PhotoOpener - opens local images in the browser
   * Version: 1.0
   * Status: Started
   * Could Be Improved: Yes
 */

using System;
using System.IO; // To read image files from disk
using System.Net;
using System.Net.Http; // To create local http server
using System.Threading.Tasks; // For async operations


namespace PhotoOpener;

public class HttpServer
{
    private static HttpListener _listener; // Private Listener Object

    public static HttpListener ListenerObj
    {
        get => _listener; // Allows reading
        private set => _listener = value; // Prevents value change from outside
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        
        ListenerObj = new HttpListener(); // Initialisation of a listener object
        ListenerObj.Prefixes.Add("http://127.0.0.1:8080/"); // Local Host allocation
        try
        {

            ListenerObj.Start(); // Starting point
            Console.WriteLine("--[INFO]-- Server started on http://127.0.0.1:8080/");

            while (true)
            {
                // Await for a request
                HttpListenerContext context = ListenerObj.GetContext();
                // Send request to a function
                HandleRequest(context);
            }
        }
        catch (Exception ex)
        {
            // Printing unexpected errors
            Console.WriteLine("--[ERROR]-- " + ex.ToString());
        }
        finally
        {
            ListenerObj.Stop(); // Automatic termination
        }
    }

    private static void HandleRequest(HttpListenerContext context)
    {
        
    }
}