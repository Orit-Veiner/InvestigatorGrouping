using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InvestigatorGrouping
{
    public class RedundancyCleaner
    {
        #region Private Members
        private Regex SpacesRegex;
        private Regex NewLineRegex;
        #endregion Private Members

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public RedundancyCleaner()
        {
            SpacesRegex = new Regex(@"[\t ]+");
            NewLineRegex = new Regex(@"[\n\r]+");
        }

        /// <summary>
        /// Clean redundancy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string NormalizeText(string input)
        {
            string returnValue = input;

            returnValue = returnValue.Trim();
            returnValue = SpacesRegex.Replace(returnValue, " ");
            returnValue = NewLineRegex.Replace(returnValue, "\n");

            return returnValue;
        }
        #endregion Public Methods
    }
}
