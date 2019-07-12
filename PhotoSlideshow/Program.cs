using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoSlideshow
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            Slideshow examplePhotos = new Slideshow(@"\hash code\in\e_shiny_selfies.txt", @"\hash code\out\e_shiny_selfies.txt");
            Console.WriteLine("Object initialiation executed in {0}.", DateTime.Now - startTime);

            startTime = DateTime.Now;
            examplePhotos.ReadInputFile();
            Console.WriteLine("ReadInputFile executed in {0}.", DateTime.Now - startTime);

            startTime = DateTime.Now;
            examplePhotos.GenerateSlideShow();
            Console.WriteLine("GenerateSlideShow executed in {0}.", DateTime.Now - startTime);

            startTime = DateTime.Now;
            examplePhotos.PrintOutputFile();
            Console.WriteLine("PrintOutputFile executed in {0}.", DateTime.Now - startTime);
        }

        private static void TestMethod()
        {
            Slideshow example = new Slideshow(@"\hash code\in\a_example.txt", "some.txt");
            example.ReadInputFile();
            foreach (var photo in example.photos)
            {
                Console.WriteLine("Photo ID: " + photo.Key);
                Console.Write("Tags: ");
                foreach (string tag in photo.Value)
                {
                    Console.Write(tag + " ");
                }
                Console.WriteLine();
            }

            example.GenerateSlideShow();
            example.PrintOutputFile();
        }
    }
}
