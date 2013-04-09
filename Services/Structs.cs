// <copyright file="Structs.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Services
{
    using System;

    using CookComputing.XmlRpc;

    #region Structs

    public struct BlogInfo
    {
        public string blogName;

        public string blogid;

        public string url;
    }

    public struct Category
    {
        public string categoryId;

        public string categoryName;
    }

    [Serializable]
    public struct CategoryInfo
    {
        public string categoryid;

        public string description;

        public string htmlUrl;

        public string rssUrl;

        public string title;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;

        public string type;

        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        public string[] categories;

        public DateTime dateCreated;

        public string description;

        public string mt_excerpt;

        public string mt_keywords;

        public string permalink;

        public object postid;

        public string title;

        public string userid;

        public string wp_slug;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct BloggerPost
    {
        public string content;

        public DateTime dateCreated;

        public object postid;

        public string userid;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;

        public string url;
    }

    public struct UserInfo
    {
        public string email;

        public string firstname;

        public string lastname;

        public string nickname;

        public string url;

        public string userid;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public byte[] bits;

        public string name;

        public string type;
    }

    [Serializable]
    public struct MediaObjectInfo
    {
        public string url;
    }

    public struct MTCategory
    {
        public string categoryId;

        public string categoryName;

        public bool isPrimary;
    }

    #endregion
}