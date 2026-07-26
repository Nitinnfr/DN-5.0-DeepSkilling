using System;
using System.Threading;

namespace ProxyPatternExample
{
    // --- STEP 2: DEFINE SUBJECT INTERFACE ---
    public interface IImage
    {
        void Display();
    }


    // --- STEP 3: IMPLEMENT REAL SUBJECT CLASS ---
    // This represents the resource-heavy class that connects to a remote server.
    public class RealImage : IImage
    {
        private readonly string _fileName;

        public RealImage(string fileName)
        {
            _fileName = fileName;
            LoadFromRemoteServer();
        }

        // Heavy simulation of network lag/disk I/O
        private void LoadFromRemoteServer()
        {
            Console.WriteLine($"[Network] Connecting to remote server...");
            Thread.Sleep(1500); // Simulate network latency
            Console.WriteLine($"[Network] Successfully downloaded and cached raw bytes for '{_fileName}'.");
        }

        public void Display()
        {
            Console.WriteLine($"[Render] Displaying crisp high-res image: '{_fileName}'\n");
        }
    }


    // --- STEP 4: IMPLEMENT PROXY CLASS ---
    // Manages the lifecycle of RealImage, adding Lazy Loading and Caching features.
    public class ProxyImage : IImage
    {
        private RealImage _realImage;
        private readonly string _fileName;

        public ProxyImage(string fileName)
        {
            _fileName = fileName;
            // Notice: The RealImage is NOT instantiated here during construction!
            Console.WriteLine($"[Proxy] Placeholder created for '{_fileName}' (No download yet).");
        }

        public void Display()
        {
            // LAZY INITIALIZATION & CACHING: 
            // If it's the first time displaying, download it. Otherwise, reuse the cached instance.
            if (_realImage == null)
            {
                Console.WriteLine($"[Proxy] First time display request. Initializing RealImage...");
                _realImage = new RealImage(_fileName);
            }
            else
            {
                Console.WriteLine($"[Proxy] Cache Hit! Using already loaded instance for '{_fileName}'.");
            }

            // Delegate the actual drawing/rendering to the Real Subject
            _realImage.Display();
        }
    }


    // --- STEP 5: TEST THE PROXY IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Photo Gallery App Initializing ---\n");

            // Create placeholders for our high-res photos
            IImage landscapePhoto = new ProxyImage("mountain_sunset_4k.png");
            IImage portraitPhoto = new ProxyImage("family_portrait_hd.png");
            Console.WriteLine(new string('-', 50));

            // User scrolls down and clicks on the landscape photo
            Console.WriteLine("\nAction: User clicks on Landscape Photo (First Time Loading)");
            landscapePhoto.Display();
            Console.WriteLine(new string('-', 50));

            // User backs out and clicks on the landscape photo again
            Console.WriteLine("\nAction: User clicks on Landscape Photo AGAIN (Should be instant)");
            landscapePhoto.Display();
            Console.WriteLine(new string('-', 50));

            // User switches to the portrait photo (never loaded before)
            Console.WriteLine("\nAction: User clicks on Portrait Photo");
            portraitPhoto.Display();
            Console.WriteLine(new string('-', 50));
        }
    }
}