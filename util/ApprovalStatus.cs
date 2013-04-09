// <copyright file="ApprovalStatus.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;

    using Engage.Dnn.Publish.Data;

    /// <summary>
    /// Summary description for ApprovalStatus.
    /// </summary>
    public class ApprovalStatus
    {
        public static readonly ApprovalStatus Approved = new ApprovalStatus("Approved");

        public static readonly ApprovalStatus Archived = new ApprovalStatus("Archived");

        public static readonly ApprovalStatus Edit = new ApprovalStatus("Edit");

        public static readonly ApprovalStatus Waiting = new ApprovalStatus("Waiting for Approval");

        private readonly string _name = string.Empty;

        private int _id = -1;

        private ApprovalStatus(string name)
        {
            this._name = name;
        }

        public string Name
        {
            get { return this._name; }
        }

        public static ApprovalStatus GetFromId(int id, Type ct)
        {
            if (ct == null)
            {
                throw new ArgumentNullException("ct");
            }

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException("id");
            }

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    var cot = f.GetValue(type) as ApprovalStatus;
                    if (cot != null)
                    {
                        // this prevents old, bogus classes defined in the code from killing the app
                        // client needs to check the return value
                        try
                        {
                            if (id == cot.GetId())
                            {
                                return cot;
                            }
                        }
                        catch
                        {
                            // drive on
                        }
                    }
                }

                type = type.BaseType; // check the super type 
            }

            return null;
        }

        public static ApprovalStatus GetFromName(string name, Type ct)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (ct == null)
            {
                throw new ArgumentNullException("ct");
            }

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    var cot = f.GetValue(type) as ApprovalStatus;
                    if (cot != null)
                    {
                        // this prevents old, bogus classes defined in the code from killing the app
                        // client needs to check the return value
                        try
                        {
                            if (name.Equals(cot._name, StringComparison.OrdinalIgnoreCase))
                            {
                                return cot;
                            }
                        }
                        catch
                        {
                            // drive on
                        }
                    }
                }

                type = type.BaseType; // check the super type 
            }

            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
        {
            if (this._id == -1)
            {
                IDataReader dr = null;

                try
                {
                    dr = DataProvider.Instance().GetApprovalStatusId(this._name);
                    if (dr.Read())
                    {
                        this._id = Convert.ToInt32(dr["ApprovalStatusId"], CultureInfo.InvariantCulture);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return this._id;
        }
    }
}