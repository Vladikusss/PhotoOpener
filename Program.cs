/*
 * Created by Vladdy | 30.03.2025
   * Last Updated: 30.03.2025
   * Program: PhotoOpener - opens local images in the browser
   * Version: 1.2
   * Status: Completed
   * Could Be Improved: Yes
   * GitHub Repository: https://github.com/Vladikusss/PhotoOpener
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
        ListenerObj.Prefixes.Add("YOUR LOCAL HOST"); // Local Host allocation
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
        // Extract requested file path
        string requestedFile = context.Request.Url.LocalPath.TrimStart('/');
        
        requestedFile = Path.GetFileName(requestedFile);
        
        SendFile(context, requestedFile);
    }

    private static void SendFile(HttpListenerContext context, string requestedFile)
    {
        Console.WriteLine("--[INFO]-- Requested File: " + requestedFile);
        
        string directory = "YourPath";
        string fileName = requestedFile;
        string filePath = Path.Combine(directory, fileName);
        
        Console.WriteLine("--[INFO]-- File Path: " + filePath);

        if (File.Exists(filePath))
        {
            try
            {
                Console.WriteLine("--[INFO]-- File exists: " + filePath);
                context.Response.ContentType = "image/jpg"; // Specify file type
                // Number of bytes in the edata included in response
                context.Response.ContentLength64 = new FileInfo(filePath).Length;

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Copies file data into an outputable stream that is sent to Response object to client
                    file.CopyTo(context.Response.OutputStream);
                }

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--[ERROR]-- " + ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Close();
            }
            
        }
        else
        { // File is not found 
            Console.WriteLine("--[ERROR]-- File was not found, sending 404.");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Close();
        }
    }
}