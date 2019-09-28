using System;
using System.IO;
using System.Collections.Generic;

namespace PhotoSlideshow
{
    internal static class Program
    {
        private static int noOfPhotos;
        private static Dictionary<string, HashSet<string>> photoList = new Dictionary<string, HashSet<string>>();
        private static List<string> SlideShow = new List<string>();

        private static HashSet<string> photosAlreadyInSlideShow = new HashSet<string>();
        private static int TotalMarks = 0;

        private static string inputFileName = @"\Hash Code\in\a_example.txt";
        private static string outputFileName = @"\Hash Code\out\a_example.txt";

        private static void ReadInputFile()
        {
            StreamReader reader;
            using (reader = new StreamReader(inputFileName))
            {
                noOfPhotos = int.Parse(reader.ReadLine());
                //Console.WriteLine(noOfPhotos);

                string[] verticalPhotoDescription = null;
                int verticalPhotoId = 0;
                int verticalPhotoCount = 0;

                for (int photoId = 0; photoId < noOfPhotos; photoId++)
                {
                    string[] photoDescription = reader.ReadLine().Split(" ");
                    char photoOrientation = char.Parse(photoDescription[0]);

                    

                    if (photoOrientation == 'H')
                    {
                        photoList.Add(photoId.ToString(), GetTags(photoDescription));
                        //Console.WriteLine("Horizontal Photo Added.");
                    }
                    else
                    {
                        //Console.WriteLine("Vertical Photo Count: " + verticalPhotoCount);
                        if ((verticalPhotoCount + 1) % 2 == 0)
                        {
                            photoList.Add(GetNewPhotoId(verticalPhotoId, photoId), CombineTwoVerticalPhotoTags(verticalPhotoDescription, photoDescription));
                            //Console.WriteLine("Two Vertical Photos Added.");
                            verticalPhotoCount = 0;
                        }
                        else
                        {
                            //Console.WriteLine("Adding One Vertical Photo To The Queue.");
                            verticalPhotoCount++;
                            //Console.WriteLine("Vertical Photo Count: " + verticalPhotoCount);
                            verticalPhotoDescription = photoDescription;
                            verticalPhotoId = photoId;
                        }
                    }
                }
            }
        }

        private static HashSet<string> GetTags(string[] photoDescription)
        {
            HashSet<string> photoTags = new HashSet<string>();
            for(int i = 2; i < photoDescription.Length; i++)
            {
                photoTags.Add(photoDescription[i]);
            }
            return photoTags;
        }

        private static string GetNewPhotoId(int firstPhotoId, int secondPhotoId)
        {
            string newPhotoId = firstPhotoId.ToString() + " " + secondPhotoId.ToString();
            //Console.WriteLine("Two Vertical Photos Combined With ID: " + newPhotoId);
            return newPhotoId;
        }

        private static HashSet<string> CombineTwoVerticalPhotoTags(string[] firstPhotoDescription, string[] secondPhotoDescription)
        {
            HashSet<string> photoTags = GetTags(firstPhotoDescription);
            photoTags.UnionWith(GetTags(secondPhotoDescription));
            //Console.WriteLine("Two Vertical PhotoTags Combined.");
            return photoTags;
        }

        private static void CreateSlideShow()
        {
            foreach (var photoDescription in photoList)
            {
                Console.WriteLine("Current photo id: " + photoDescription.Key);
                if (photosAlreadyInSlideShow.Contains(photoDescription.Key))
                {
                    // Skip this photo
                    continue;
                }
                else
                {
                    SlideShow.Add(photoDescription.Key);
                    //Console.WriteLine("Photo added to the slideshow.");
                    photosAlreadyInSlideShow.Add(photoDescription.Key);
                    string nextPhotoID = GetTheNextSlide(photoDescription.Value);
                    if (nextPhotoID == null)
                    {
                        continue;
                    }
                    else
                    {
                        SlideShow.Add(nextPhotoID);
                        photosAlreadyInSlideShow.Add(nextPhotoID);
                    }
                }
            }
        }

        private static string GetTheNextSlide(HashSet<string> photoTags)
        {
            foreach(var photoDescription in photoList)
            {
                if (photoDescription.Value == photoTags || photosAlreadyInSlideShow.Contains(photoDescription.Key))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Calling CalculateInterestFactor with photo id: " + photoDescription.Key);
                    int interestFactor = CalculateInterestFactor(photoTags, photoDescription.Value);
                    if (interestFactor > 0)
                    {
                        TotalMarks += interestFactor;
                        return photoDescription.Key;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return null;
        }

        private static int CalculateInterestFactor(HashSet<string> firstPhotoTags, HashSet<string> secondPhotoTags)
        {
            int interestFactor = 0;
            return interestFactor;
        }

        private static int GetNumberOfCommonTags(HashSet<string> setOne, HashSet<string> setTwo)
        {
            HashSet<string> commonTags = setOne;
            commonTags.IntersectWith(setTwo);
            return setOne.Count;
        }

        private static int GetNumberOfUniqueTags(HashSet<string> setOne, HashSet<string> setTwo)
        {
            int numberOfCommonTags = GetNumberOfCommonTags(setOne, setTwo);
            int numberOfUniqueTags = setOne.Count - numberOfCommonTags;
            return numberOfUniqueTags;
        }

        private static void PrintSlideShowOnConsole()
        {
            foreach(string str in SlideShow)
            {
                Console.WriteLine(str);
            }
        }

        static void Main(string[] args)
        {
            //ReadInputFile();
            //CreateSlideShow();
            //PrintSlideShowOnConsole();
            //Console.WriteLine("Total Marks: " + TotalMarks);

            HashSet<string> setOne = new HashSet<string>();
            setOne.Add("cat");
            setOne.Add("beach");
            setOne.Add("sun");
            HashSet<string> setTwo = new HashSet<string>();
            setTwo.Add("garden");
            setTwo.Add("cat");
            Console.WriteLine("Number of Common Tags: " + GetNumberOfCommonTags(setOne, setTwo));
            Console.WriteLine("Number of Unique Tags: " + GetNumberOfUniqueTags(setOne, setTwo));
            Console.WriteLine("Number of Common Tags: " + GetNumberOfCommonTags(setTwo, setOne));
            Console.WriteLine("Number of Unique Tags: " + GetNumberOfUniqueTags(setTwo, setOne));
        }
    }
}
