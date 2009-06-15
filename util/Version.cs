//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Text;

using DotNetNuke.Entities.Modules;

namespace Engage.Dnn.Publish.Util
{
    /// <summary>
    /// Summary description for Version.
    /// </summary>
    public sealed class Version
    {
        private int major;
        private int minor;
        //private int revision = 0;
        private int build;

        public const string VersionSupportingIPortable = "04.05.01";

        private Version()
        {
        }

        public static Version GetCurrentVersion(int moduleId)
        {
            ModuleInfo info = Utility.GetPublishModuleInfo(moduleId);
            Version version = ConvertToVersion(info.Version, CultureInfo.InvariantCulture);
            return version;
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "moduleId", Justification = "Not Implemented")]
        public static Version[] GetVersions(int moduleId)
        {
            throw new NotImplementedException();
        }

        private static Version ConvertToVersion(string versionString, IFormatProvider provider)
        {
            string[] versionInfo = versionString.Split('.');

            Version version = new Version();

            version.major = Convert.ToInt32(versionInfo[0], provider);
            version.minor = Convert.ToInt32(versionInfo[1], provider);
            version.build = Convert.ToInt32(versionInfo[2], provider);
            //v.build = (int)dr["Build"];

            return version;
        }

        public bool LessThanEqualTo(string version)
        {
            return LessThanEqualTo(version, CultureInfo.InvariantCulture);
        }

        public bool LessThanEqualTo(string version, IFormatProvider provider)
        {
            Version v = ConvertToVersion(version, provider);

            return this.major < v.Major ||
                   (this.major == v.Major && this.minor < v.Minor) || 
                   (this.major == v.Major && this.minor == v.Minor && this.build < v.build);
        }

        #region "Properties"


        public int Major
        {
            get { return this.major; }
        }

        public int Minor
        {
            get { return this.minor; }
        }

        //public int Revision
        //{
        //    get { return this.revision; }
        //}

        public int Build
        {
            get { return this.build; }
        }

        #endregion

        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            StringBuilder sb = new StringBuilder(8);

            sb.Append(this.major.ToString(provider));
            sb.Append(".");
            sb.Append(this.minor.ToString(provider));
            sb.Append(".");
            sb.Append(this.build.ToString(provider));

            return sb.ToString();
        }

    }
}

