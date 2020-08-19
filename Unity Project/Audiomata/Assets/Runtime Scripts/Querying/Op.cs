
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
        /// <param name="guidSetLeft"> the current result of all previous operations</param>
        /// <param name="guidSetRight">the results of a dicionary tag query to the right hand side of operator of ids</string></param>
        /// <remarks>All IEnumerables are of type List<string>, a change of type can cause unintended faults</remarks>      
        public delegate IEnumerable<string> QueryOperation(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qmInstance);


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
    /// The way in which the operation character is inseterted into a query, with int values corresponding to precedence
    /// </summary>
    public enum OpType
    {
        /// <summary>
        /// Before the tag only operates on one tag (lhs will always be null)
        /// </summary>
        Prefix = 10,
        /// <summary>
        /// After the tag, only operates on 2 tags
        /// </summary>
        Postfix = 1,
        /// <summary>
        /// As a seperator between multiple tags e.g. [example&other]&something-else
        /// </summary>
        /// <remarks>Does the operations within it's tags then does it's operation afterwards</remarks>
        CaptureGroup = 100,
        //Other Optype ideas:
        //tag -> does something on the tag (can be used to compute the correct tag with known tag patterns)
        //post op - does something when the op is finished,
        //preop - does something at the start of the operation
    }
}
