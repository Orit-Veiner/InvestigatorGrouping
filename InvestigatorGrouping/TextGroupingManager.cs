using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InvestigatorGrouping
{
    public class TextGroupingManager
    {
        #region Private Members
        private Dictionary<int, Row> Sentences;
        private Dictionary<int, SentencesGroup> SentencesGroups;
        private Regex DateFormatRegex;
        #endregion Private Members


        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public TextGroupingManager()
        {
            Sentences = new Dictionary<int, Row>();
            SentencesGroups = new Dictionary<int, SentencesGroup>();
      
            //Regular expression to match date in the input file, f.e. 01-01-2016 06:00:00
            DateFormatRegex = new Regex("([0-9]{2}-[0-9]{2}-[0-9]{4} [0-9]{2}:[0-9]{2}:[0-9]{2})");
        }        

        /// <summary>
        /// Part text into groupings
        /// </summary>
        /// <param name="inputText"></param>
        public void GroupText(string inputText)
        {
            //Get rows, seperated by line breaks
            string[] rows = inputText.Split('\n');
            //Add rows as sentences to memory
            SaveRowsInMemory(rows);

            //Part rows in memory to groups
            foreach (int sentenceKey in Sentences.Keys)
            {
                AddSentenceToGroup(sentenceKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Text ready to be written to file</returns>
        public StringBuilder GetResults()
        {
            StringBuilder outputText = new StringBuilder();
            outputText.Append("====");

            //Print Groups
            foreach (int groupKey in SentencesGroups.Keys)
            {
                outputText.Append(Environment.NewLine);

                //Print sentences in group
                foreach (int sentenceKey in SentencesGroups[groupKey].SentencesKeysInGroup)
                {
                    outputText.Append(Sentences[sentenceKey].Date + " " +
                        Sentences[sentenceKey].SentenceWithoutDate);
                    outputText.Append(Environment.NewLine);
                }

                //Print mismatches
                outputText.Append("The changing word was: ");
                foreach (string mismatch in SentencesGroups[groupKey].MismatchesInGroup)
                {
                    outputText.Append(mismatch + "; ");
                }

                outputText.Append(Environment.NewLine);
            }
            
            outputText.Append("====");
            return outputText;
        }
        #endregion Public Methods


        #region Private Methods
        /// <summary>
        /// Add sentence to relevant group if exists
        /// Create new group if it does not exist
        /// </summary>
        /// <param name="sentenceKey"></param>
        private void AddSentenceToGroup(int sentenceKey)
        {
            string[] currentSentence = Sentences[sentenceKey].Words;
            string[] sentenceInMemory = null;

            bool newGroupIsNeeded = true;
            HashSet<string> mismatches = new HashSet<string>();                        

            //Scan all groups in memory to see if relevant group exists
            foreach (int key in SentencesGroups.Keys)
            {
                sentenceInMemory = SentencesGroups[key].WordsRepresentingGroup;
                mismatches = GetMismatchesIfSimilarSentences(currentSentence, sentenceInMemory);

                //Sentences are similar, mismatches were filled
                if (mismatches.Count > 0)
                {
                    //A relevant group exists
                    newGroupIsNeeded = false;
                    //Add sentence and its mismatches to the relevant group
                    SentencesGroups[key].SentencesKeysInGroup.Add(sentenceKey);
                    SentencesGroups[key].MismatchesInGroup.AddRange(mismatches);
                }
            }

            if (newGroupIsNeeded)
            {                
                //Add this first sentence to the new group
                List<int> sentencesInGroup = new List<int>();
                sentencesInGroup.Add(sentenceKey);
                //Init group
                SentencesGroup newGroup = new SentencesGroup(Sentences[sentenceKey].Words, 
                    sentencesInGroup);

                //Add group
                int newGroupKey = SentencesGroups.Keys.Count;                
                SentencesGroups.Add(newGroupKey, newGroup);
            }
        }

        /// <summary>
        /// Get mismatches if sentences are similar
        /// </summary>
        /// <param name="wordsOfSentence1"></param>
        /// <param name="wordsOfSentence2"></param>
        /// <returns>Empty set if sentences are not similar</returns>
        private HashSet<string> GetMismatchesIfSimilarSentences(string[] wordsOfSentence1, 
            string[] wordsOfSentence2)
        {
            HashSet<string> mismatchesInGroup = new HashSet<string>();
            int mismatchesCounter = 0;
            int sentence1Length = wordsOfSentence1.Length;

            //Different lengths indicate not similar sentences
            if (wordsOfSentence1.Length != wordsOfSentence2.Length)
            {
                mismatchesInGroup.Clear();
                return mismatchesInGroup;
            }
                                 
            for (int i = 0; i < sentence1Length; i++)
            {
                //Compare sentences word by word
                if (wordsOfSentence1[i] != wordsOfSentence2[i])
                {
                    //Add the non identical words as mismatched
                    mismatchesCounter++;
                    mismatchesInGroup.Add(wordsOfSentence1[i]);
                    mismatchesInGroup.Add(wordsOfSentence2[i]);
                    
                    //More than one word is mismatching - sentences are not similar
                    if (mismatchesCounter > 1)
                    {
                        mismatchesInGroup.Clear();
                        return mismatchesInGroup;
                    }
                }
            }

            return mismatchesInGroup;
        }

        /// <summary>
        /// Add rows as sentences and save their dates aside
        /// </summary>
        /// <param name="rows">Input rows, after redundancy</param>
        private void SaveRowsInMemory(string[] rows)
        {
            string dateOfRow = string.Empty;
            string sentenceWithoutDate = string.Empty;
            int keyForNewSentence = 0;
            
            foreach (string row in rows)
            {
                //Get date from row, by regex match (the only and first group value is relevant)
                dateOfRow = DateFormatRegex.Match(row).Groups[1].Value;
                //Get sentence without the date from row, by replacing date with blank
                sentenceWithoutDate = DateFormatRegex.Replace(row, "").Trim();

                //Add date to its dictionary and sentence without date to its dictionary
                //Both will have the same key 
                Row extractedRow = new Row(dateOfRow, sentenceWithoutDate);
                Sentences.Add(keyForNewSentence, extractedRow);
                
                keyForNewSentence++;
            }
        }        
        #endregion Private Methods
    }
}
