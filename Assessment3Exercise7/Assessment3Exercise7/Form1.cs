using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //added to allow for working with files

namespace Assessment3Exercise7
{
    public partial class Form1 : Form
    {
    //Paxston Gulledge
    //July 27, 2017
    //Assignment 3 Exercise 7. Read name and three scores from file and write average to new file.
        private StreamWriter fil; // declare a file string object

        //Counter exists on a global scope to be used outside of the loading of the form
        int counter = 0;
        //Setting up our studentName and the rest of our score variables as arrays, going up to a size of 250, just as a large number that might
        //be in use. We set all of student name to "" because we check for the value of it later on.
        string[] studentName = new string[250];
        int[] studentScoreOne = new int[250];
        int[] studentScoreTwo = new int[250];
        int[] studentScoreThree = new int[250];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 250; i++)
            {
                studentName[i] = "";
            }
            //Two counters will be utilized. counter is used to increment for a new student. scoreCount is used to increment the test scores.
            int scoreCount = 0;
            if (File.Exists("numbers.txt"))
            {
                //Try catch to make sure we can read the file, if something goes wrong we display an error message.
                try
                {
                    //We open up the numbers.txt file, after checking to make sure it exists.
                    using (var streamReader = File.OpenText("numbers.txt"))
                    {
                        //The file is opened and read all the way to the end, creating a new line of data everytime there is a space or line break
                        var lines = streamReader.ReadToEnd().Split(' ','\n');
                        //This is our method to go through every line and assess what the data is.
                        //This works given the fact that entries are Name # # #
                        foreach (var line in lines)
                        {
                            //When we get to a line, we set that line to our currentString, we check to see if this string can pass a check
                            //to be an integer. If it can't, we know it is a name, if it can we know it is a number.
                            string currentString = line;
                            int currentNumber = 0;
                            bool isANumber = false;
                            isANumber = Int32.TryParse(currentString, out currentNumber);
                            //We want to make sure there isn't already any characters in the index of the studentName and that we've confirmed the
                            //current line is not a number, if this is true we can set the currentString to the studentName of the current number
                            //represented by the counter.
                            if (studentName[counter] == "" && (!isANumber))
                            {
                                studentName[counter] = currentString;
                                lblDisplay.Text = lblDisplay.Text + "Student Name: " + studentName[counter] + " ";
                            //The only cases we should have outside of a string is a number. Our scoreCount starts at 0, which is where we assume
                            //the next line is a number that is our first score. We set this to studentScoreOne[counter], and increment our scoreCount.
                            //The same thing happens for when scoreCount is equal to 1. However, when scoreCount is equal to 2, we have our third score.
                            //We set scoreCount back to 0. , we increment counter, implying there is a new record, and make sure to add a linebreak to our lbl.
                            } else if (scoreCount == 0 && isANumber)
                            {
                                studentScoreOne[counter] = currentNumber;
                                lblDisplay.Text = lblDisplay.Text + "First Score: " + studentScoreOne[counter] + " ";
                                scoreCount++;
                            }
                            else if (scoreCount == 1 && isANumber)
                            {
                                studentScoreTwo[counter] = currentNumber;
                                lblDisplay.Text = lblDisplay.Text + "Second Score: " + studentScoreTwo[counter] + " ";
                                scoreCount++;
                            }
                            else if (scoreCount == 2 && isANumber)
                            {
                                studentScoreThree[counter] = currentNumber;
                                lblDisplay.Text = lblDisplay.Text + "Third Score: " + studentScoreThree[counter] + "\n";
                                scoreCount = 0;
                                counter++;
                            }

                        }
                    }
                }
                catch (System.IO.IOException exc)
                {
                    lblDisplay.Text = exc.Message;
                }
            }
            else
            {
                lblDisplay.Text = "File unavailable";
            }

            //Now that we have loaded up our numbers file and got our information, we open up our averages file so that we can write to it when a button
            //is pressed.
            try
            {
                fil = new StreamWriter("averages.txt");
            }
            catch (DirectoryNotFoundException exc)
            {
                lblDisplay.Text = "Invalid directory" + exc.Message;
            }

            catch (System.IO.IOException exc)
            {
                lblDisplay.Text = exc.Message;
            }
        }

        private void btnAverage_Click(object sender, EventArgs e)
        {
            //Attempt to write to the file, catch for if there is an exception or error
            try
            {
                //We want the average to be to a decimal place, like a real grade, so we are going to create a float
                float average = 0;
                //This will go for as many records as were counted in the initial.
                for (int i = 0; i < counter; i++)
                {                    
                    //For division, we cast the integers as float so that when we divide we aren't doing integer division
                    average = (float)(studentScoreOne[i] + studentScoreTwo[i] + studentScoreThree[i]) / 3;
                    //Write to the line the student name and their test average
                    fil.WriteLine(studentName[i] + " " + average.ToString("F"));
                }
                MessageBox.Show("Averages written to file averages.txt");
            }
            catch (System.IO.IOException exc)
            {
                lblDisplay.Text = exc.Message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //We have to close the file before we close the program for averages.txt to update.
                fil.Close();
            }
            catch
            {

            }
        }
    }
}
