
using System.Collections.Generic;

namespace Audiomata
{
    /// <summary>
    /// A structure class used to store data for query manager operations
    /// </summary>
    public class Op
    {
        /// <summary>
        /// Character used to show no operation character
        /// </summary>
       public static char NoOpChar
        {
            get
            {
                return '\0';
            }
        }


        /// <summary>
        /// An Operation to be done with the query manager
        /// </summary>
        /// <param name="guidSetLeft"> the current result of all previous operations, modify this to apply ops </param>
        /// <param name="guidSetRight">the results of a dicionary tag query to the right hand side of operator of ids</string></param>
        /// <remarks>All IEnumerables are of type List<string>, a change of type can cause unintended faults</remarks>       
        public delegate IEnumerable<string> QueryOperation(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qmInstance = null);


        /// <summary>
        /// delegate function which performs this operation
        /// </summary>
        public QueryOperation opDelegate;
        /// <summary>
        /// The type of operation
        /// </summary>
        public OpType opType;
        /// <summary>
        /// the simple that indicates the operation or the start of it
        /// </summary>
        public char opSymbol;
        /// <summary>
        /// the symbol that indicates the end of the operation (optional and not always required
        /// </summary>
        public char opEnder = NoOpChar;

        public string GetString()
        {
            if (opEnder == NoOpChar)
            {
                return opSymbol.ToString();
            }
            else return "" + opSymbol + opEnder;
        }
    }

    /// <summary>
    /// The way in which the operation character is inseterted into a query
    /// </summary>
    public enum OpType
    {
        /// <summary>
        /// Before the tag only operates on one tag
        /// </summary>
        Prefix,
        /// <summary>
        /// After the tag, only operates on 2 tags
        /// </summary>
        Postfix,
        /// <summary>
        /// As a seperator between multiple tags e.g. [example&other]&something-else
        /// </summary>
        /// <remarks>Does the operations within first then returns the values, this may change in the future</remarks>
        Group,
        //Tag opType was to be done, this would have done stuff to process and output a tag
    }
}
