using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestigatorGrouping
{
    public class Row
    {
        #region Public Members
        public string Date { get; set; }
        public string SentenceWithoutDate { get; set; }
        public string[] Words { get; set; }
        #endregion Public Members

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sentence"></param>
        public Row(string date, string sentence)
        {
            Date = date;
            SentenceWithoutDate = sentence;
            Words = SentenceWithoutDate.Split(' ');
        }
        #endregion Public Methods
    }
}
