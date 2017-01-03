using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestigatorGrouping
{
    public class SentencesGroup
    {
        #region Public Members
        //A group of similar sentences is represented by its first sentence appearance
        public string[] WordsRepresentingGroup  { get; set; }
        //Holds all sentences in group of similar sentences, by their keys
        public List<int> SentencesKeysInGroup { get; set; }
        //Holds all words that are mismatches in group of similar sentences
        public List<string> MismatchesInGroup { get; set; }
        #endregion Public Members


        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wordsRepresentingGroup"></param>
        /// <param name="sentencesKeysInGroup"></param>
        public SentencesGroup(string[] wordsRepresentingGroup, List<int> sentencesKeysInGroup)
        {
            WordsRepresentingGroup = wordsRepresentingGroup;
            SentencesKeysInGroup = sentencesKeysInGroup;
            MismatchesInGroup = new List<string>();
        }
        #endregion Public Methods
    }
}
