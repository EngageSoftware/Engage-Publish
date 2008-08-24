//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish.Tokens
{
    public enum TokenType
    {
        ItemAttribute = 1,
        UserControl = 2
    }

    /// <summary>
    /// Summary description for PublishToken.
    /// </summary>
    public class Token
    {
        public Token()
        {

        }
        private string tokenName;
        private List<string> tokenProperties;
        private TokenType tokenTypeId;

        public string TokenName
        {
            get
            {
                return tokenName;
            }
            set
            {
                tokenName = value;
            }
        }

        public List<string> TokenProperties
        {
            get { return tokenProperties; }
            set { tokenProperties = value; }
        }

        public TokenType TokenTypeId
        {
            get
            {
                return tokenTypeId;
            }
            set
            {
                tokenTypeId = value;
            }
        }



        //TODO: add token
        //TODO: delete token
        //TODO: load token

    }
}

