//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.



namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Globalization;
    using System.Text;

    using DotNetNuke.Entities.Modules;
    /// <summary>
    /// Summary description for Version.
    /// </summary>
    public sealed class Version
    {
        private int _major;
        private int _minor;
        //private int revision = 0;
        private int _build;

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

            var version = new Version
                              {
                                      _major = Convert.ToInt32(versionInfo[0], provider),
                                      _minor = Convert.ToInt32(versionInfo[1], provider),
                                      _build = Convert.ToInt32(versionInfo[2], provider)
                              };

            //v._build = (int)dr["Build"];

            return version;
        }

        public bool LessThanEqualTo(string version)
        {
            return LessThanEqualTo(version, CultureInfo.InvariantCulture);
        }

        public bool LessThanEqualTo(string version, IFormatProvider provider)
        {
            Version v = ConvertToVersion(version, provider);

            return _major < v.Major ||
                   (_major == v.Major && _minor < v.Minor) || 
                   (_major == v.Major && _minor == v.Minor && _build < v._build);
        }

        #region "Properties"


        public int Major
        {
            get { return _major; }
        }

        public int Minor
        {
            get { return _minor; }
        }

        //public int Revision
        //{
        //    get { return this.revision; }
        //}

        public int Build
        {
            get { return _build; }
        }

        #endregion

        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            var sb = new StringBuilder(8);

            sb.Append(_major.ToString(provider));
            sb.Append(".");
            sb.Append(_minor.ToString(provider));
            sb.Append(".");
            sb.Append(_build.ToString(provider));

            return sb.ToString();
        }

    }
}

