//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;

    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// Summary description for Version.
    /// </summary>
    public sealed class Version
    {
        public const string VersionSupportingIPortable = "04.05.01";

        private int _build;

        private int _major;

        private int _minor;

        // private int revision = 0;
        private Version()
        {
        }

        public int Build
        {
            get { return this._build; }
        }

        public int Major
        {
            get { return this._major; }
        }

        public int Minor
        {
            get { return this._minor; }
        }

        public static Version GetCurrentVersion(int moduleId)
        {
            ModuleInfo info = Utility.GetPublishModuleInfo(moduleId);
            return ConvertToVersion(info.DesktopModule.Version, CultureInfo.InvariantCulture);
        }

        [Obsolete("Not implemented")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "moduleId", Justification = "Not Implemented")]
        public static Version[] GetVersions(int moduleId)
        {
            throw new NotImplementedException();
        }

        public bool LessThanEqualTo(string version)
        {
            return this.LessThanEqualTo(version, CultureInfo.InvariantCulture);
        }

        public bool LessThanEqualTo(string version, IFormatProvider provider)
        {
            Version v = ConvertToVersion(version, provider);

            return this._major < v.Major || (this._major == v.Major && this._minor < v.Minor) ||
                   (this._major == v.Major && this._minor == v.Minor && this._build < v._build);
        }

        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            var sb = new StringBuilder(8);

            sb.Append(this._major.ToString(provider));
            sb.Append(".");
            sb.Append(this._minor.ToString(provider));
            sb.Append(".");
            sb.Append(this._build.ToString(provider));

            return sb.ToString();
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

            // v._build = (int)dr["Build"];
            return version;
        }
    }
}