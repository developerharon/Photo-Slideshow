using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhotoSlideshow
{
    internal class Slideshow
    {
        public int TotalInterestFactor = 0;
        private string inputFileName;
        private string outputFileName;

        public Dictionary<string, HashSet<string>> photos;
        private List<string> slideshow;
        private HashSet<string> slidesAlreadyInSlideshow;

        public Slideshow(string inputFileName, string outputFileName)
        {
            this.inputFileName = inputFileName;
            this.outputFileName = outputFileName;
            this.photos = new Dictionary<string, HashSet<string>>();
            this.slideshow = new List<string>();
            this.slidesAlreadyInSlideshow = new HashSet<string>();
        }

        public void ReadInputFile()
        {
            StreamReader reader = new StreamReader(inputFileName);
            try
            {
                int numberOfPhotos = int.Parse(reader.ReadLine());
                string[] photoDescription = null;
                string[] firstVerticalPhotoDescription = null;
                int firstVerticalPhotoId = 0;
                int verticalphotoCount = 0;

                for (int photoID = 0; photoID < numberOfPhotos; photoID++)
                {
                    photoDescription = reader.ReadLine().Split(" ");
                    int photoOrientation = char.Parse(photoDescription[0]);

                    if (photoOrientation == 'H')
                    {
                        // It is a vertical photo add it directly to the dictionary
                        photos.Add(photoID.ToString(), GenerateTags(photoDescription));
                        continue;
                    }
                    else
                    {
                        // It is a vertical photo
                        if (verticalphotoCount == 1)
                        {
                            HashSet<string> newPhotoTags = CombineTwoVerticalPhotoTags(firstVerticalPhotoDescription, photoDescription);
                            string newPhotoID = GenerateNewPhotoID(firstVerticalPhotoId.ToString(), photoID.ToString());
                            photos.Add(newPhotoID, newPhotoTags);
                            verticalphotoCount = 0;
                            continue;
                        }
                        firstVerticalPhotoDescription = photoDescription;
                        firstVerticalPhotoId = photoID;
                        verticalphotoCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when reading the file." + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private HashSet<string> GenerateTags(string[] photoDescription)
        {
            HashSet<string> tags = new HashSet<string>();
            int numberOfTags = int.Parse(photoDescription[1]);
            for (int tagId = 0; tagId < numberOfTags; tagId++)
            {
                tags.Add(photoDescription[tagId + 2]);
            }
            return tags;
        }

        private HashSet<string> CombineTwoVerticalPhotoTags(string[] firstPhotoDescription, string[] secondPhotoDescription)
        {
            HashSet<string> firstPhotoTags = GenerateTags(firstPhotoDescription);
            HashSet<string> secondPhotoTags = GenerateTags(secondPhotoDescription);
            firstPhotoTags.UnionWith(secondPhotoTags);
            return firstPhotoTags;
        }

        private string GenerateNewPhotoID(string firstPhotoId, string SecondphotoID)
        {
            return string.Join("+", firstPhotoId, SecondphotoID);
        }

        public void GenerateSlideShow()
        {
            string nextPhotoId = string.Empty;
            foreach (var photo in photos)
            {
                if (slidesAlreadyInSlideshow.Contains(photo.Key))
                {
                    continue;
                }
                else
                {
                    nextPhotoId = FindNextSlidePhotoId(photo.Key, photo.Value);
                    // We keep the photos in the list to keep the order of the slideshow
                    slideshow.Add(photo.Key);
                    slidesAlreadyInSlideshow.Add(photo.Key);
                    // We also keep the slides in a set to make it easy to check the slides we alreay have in the slideshow;
                    if (slidesAlreadyInSlideshow.Contains(nextPhotoId))
                    {
                        continue;
                    }
                    else
                    {
                        if (nextPhotoId == string.Empty)
                        {
                            continue;
                        }
                        slideshow.Add(nextPhotoId);
                        slidesAlreadyInSlideshow.Add(nextPhotoId);
                    }
                }
            }
            Console.WriteLine("Total Interesting Factor: " + TotalInterestFactor);
        }

        public string FindNextSlidePhotoId(string CurrentSlidePhotoId, HashSet<string> currentPhotoTags)
        {
            int currentInterestFactor = 0;
            int highestInterestFactor = 0;
            string currentPhotoId = string.Empty;
            string photoIdWithHighestIF = string.Empty;
            foreach (var photo in photos)
            {
                if (slidesAlreadyInSlideshow.Contains(photo.Key))
                {
                    continue;
                }
                if (photo.Key == CurrentSlidePhotoId)
                {
                    continue;
                }
                currentInterestFactor = CalculateInterestFactor(currentPhotoTags, photo.Value);
                currentPhotoId = photo.Key;
                if (currentInterestFactor > highestInterestFactor)
                {
                    highestInterestFactor = currentInterestFactor;
                    photoIdWithHighestIF = photo.Key;
                }
            }
            if (photoIdWithHighestIF == string.Empty)
            {
                TotalInterestFactor += currentInterestFactor;
                //Console.WriteLine("Current Interesting Factor: " + currentInterestFactor);
                return currentPhotoId;
            }
            else
            {
                TotalInterestFactor += highestInterestFactor;
                //Console.WriteLine("Highest Interest Factor: " + highestInterestFactor);
                return photoIdWithHighestIF;
            }
        }

        public int CalculateInterestFactor(HashSet<string> firstPhotoTags, HashSet<string> secondPhotoTags)
        {
            HashSet<string> commonTags = new HashSet<string>();
            commonTags.UnionWith(firstPhotoTags);
            commonTags.IntersectWith(secondPhotoTags);
            int interestFactor = FindMinimumValue(commonTags.Count, firstPhotoTags.Count - commonTags.Count, secondPhotoTags.Count - commonTags.Count);
            return interestFactor;
        }

        private int FindMinimumValue(int firstNumber, int secondNumber, int thirdNumber)
        {
            int minimumNumber = 0;
            if (firstNumber < secondNumber)
            {
                minimumNumber = FindMinimumNumber(firstNumber, thirdNumber);
            }
            else
            {
                minimumNumber = FindMinimumNumber(secondNumber, thirdNumber);
            }
            return minimumNumber;
        }

        private int FindMinimumNumber(int firstNumber, int secondNumber)
        {
            if (firstNumber < secondNumber)
            {
                return firstNumber;
            }
            return secondNumber;
        }

        public void PrintOutputFile()
        {
            StreamWriter writer = new StreamWriter(outputFileName);
            StringBuilder output = new StringBuilder();
            output.Append(slideshow.Count + "\n");
            foreach (string SlideId in slideshow)
            {
                if (SlideId.Contains("+"))
                {
                    output.Append((string.Join(" ", SlideId.Split("+"))) + "\n");
                }
                else
                {
                    output.Append(SlideId + "\n");
                }
            }

            try
            {
                string finalOutput = output.ToString();
                writer.Write(finalOutput.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while writing file. " + ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
