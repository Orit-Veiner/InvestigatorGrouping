using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InvestigatorGrouping
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read input text
            StreamReader inputReader = new StreamReader(@"Appendix/InvestigatorInput.txt");
            string content = inputReader.ReadToEnd();
            inputReader.Close();
            
            //Normalize text (f.e. delete multiple spaces in sentence)
            RedundancyCleaner textCleaner = new RedundancyCleaner();
            string normalizedText = textCleaner.NormalizeText(content);

            //Group text
            TextGroupingManager textGrouping = new TextGroupingManager();
            textGrouping.GroupText(normalizedText);
            
            //Print grouped text to file
            StringBuilder outputText = textGrouping.GetResults();
            StreamWriter outputFile = new StreamWriter(@"Appendix/Output.txt");
            outputFile.WriteLine(outputText.ToString());
            outputFile.Close();
        }
    }
}
