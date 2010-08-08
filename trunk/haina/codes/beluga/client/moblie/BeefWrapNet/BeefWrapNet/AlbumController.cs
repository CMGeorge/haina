using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class AlbumController : AControl
    {
        public AlbumController()
        {
        }

        public override string RequestAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.RequestHost))
                {
                    throw new Exception("服务器地址为空");
                }
                return this.RequestHost + "/album.do?method=";
            }
        }

        #region Covers

        /// <summary>
        /// token
        /// </summary>
        private static string tokenCovers = null;

        /// <summary>
        /// Covers
        /// </summary>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Covers(int curPage, int pageSize, string passport, out Result result)
        {
            string nameBase = string.Format("{0}_Covers_{1}", Command.C_ALBUM_COVERS_REQUEST, passport);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenCovers = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}getUserAlbumInfoList?passport={1}&curPage={2}&pageSize={3}", this.RequestAddress, passport, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenCovers, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenCovers.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);

                // 写缓存
                if (HttpStatusCode.OK.Equals(resultTmp.StatusCode))
                {
                    if (curPage == 1)
                    {
                        DeleteCacheLike(nameBase);
                    }
                    if (!WriteCache(name, ExpireSeconds, resultTmp))
                    {
                        DeleteCache(name);
                    }
                }
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region Photos

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPhotos = null;

        /// <summary>
        /// Photos
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="email"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Photos(string albumId, string email, int curPage, int pageSize, string passport, out Result result)
        {
            string nameBase = string.Format("{0}_Photos_{1}_{2}_{3}", Command.C_ALBUM_PHOTOS_REQUEST, passport, albumId, email);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenPhotos = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}getUserPhotoInfoList?passport={1}&albumId={2}&email={3}&curPage={4}&pageSize={5}", this.RequestAddress, passport, albumId, email, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPhotos, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPhotos.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);

                // 写缓存
                if (HttpStatusCode.OK.Equals(resultTmp.StatusCode))
                {
                    if (curPage == 1)
                    {
                        DeleteCacheLike(nameBase);
                    }
                    if (!WriteCache(name, ExpireSeconds, resultTmp))
                    {
                        DeleteCache(name);
                    }
                }
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region PhotoView

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPhotoView = null;

        /// <summary>
        /// PhotoView
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="albumId"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PhotoView(string photoId, string albumId, int curPage, int pageSize, string passport, out Result result)
        {
            string nameBase = string.Format("{0}_PhotoView_{1}_{2}_{3}", Command.C_ALBUM_PHOTO_VIEW_REQUEST, passport, photoId, albumId);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenPhotoView = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}getUserPhotoInfo?passport={1}photoId={2}&albumId={3}&curPage={4}&pageSize={5}", this.RequestAddress, passport, photoId, albumId, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPhotoView, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPhotoView.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);

                // 写缓存
                if (HttpStatusCode.OK.Equals(resultTmp.StatusCode))
                {
                    if (curPage == 1)
                    {
                        DeleteCacheLike(nameBase);
                    }
                    if (!WriteCache(name, ExpireSeconds, resultTmp))
                    {
                        DeleteCache(name);
                    }
                }
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region Update

        /// <summary>
        /// token
        /// </summary>
        private static string tokenUpdate = null;

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="albumName"></param>
        /// <param name="description"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Update(string albumId, string albumName, string description, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}editUserAlbumInfo?passport={1}&albumId={2}&albumName={3}&email={4}", this.RequestAddress, passport, albumId, albumName, description);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenUpdate.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region Add

        /// <summary>
        /// token
        /// </summary>
        private static string tokenAdd = null;

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="albumName"></param>
        /// <param name="description"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Add(string albumName, string description, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenAdd = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}addUserAlbumInfo?passport={1}&albumName={2}&description={3}", this.RequestAddress, passport, albumName, description);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenAdd, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenAdd.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region Del

        /// <summary>
        /// token
        /// </summary>
        private static string tokenDel = null;

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="albumIds"></param>
        /// <param name="deteleEmail"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Del(string albumIds, string deteleEmail, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenDel = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}deleteUserAlbumInfo?passport={1}&albumIds={2}&deteleEmail={3}", this.RequestAddress, passport, albumIds, deteleEmail);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenDel, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenDel.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region CommentDel

        /// <summary>
        /// token
        /// </summary>
        private static string tokenCommentDel = null;

        /// <summary>
        /// CommentDel
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="commentIds"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool CommentDel(string photoId, string commentIds, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenCommentDel = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}deleteUserPhotoComment?passport={1}&photoId={2}&commentIds={3}", this.RequestAddress, passport, photoId, commentIds);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenCommentDel, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenCommentDel.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region CommentAdd

        /// <summary>
        /// token
        /// </summary>
        private static string tokenCommentAdd = null;

        /// <summary>
        /// CommentAdd
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="commentContent"></param>
        /// <param name="commentTime"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool CommentAdd(string photoId, string commentContent, string commentTime, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenCommentAdd = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}addUserPhotoComment?passport={1}&photoId={2}&commentContent={3}&commentTime={4}", this.RequestAddress, passport, photoId, commentContent, commentTime);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenCommentAdd, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenCommentAdd.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region PhotosDel

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPhotosDel = null;

        /// <summary>
        /// PhotosDel
        /// </summary>
        /// <param name="photoIds"></param>
        /// <param name="albumId"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PhotosDel(string photoIds, string albumId, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenPhotosDel = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}deleteUserPhotoInfo?passport={1}&photoIds={2}&albumId={3}", this.RequestAddress, passport, photoIds, albumId);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPhotosDel, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPhotosDel.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region PhotoAdd

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPhotoAdd = null;

        /// <summary>
        /// PhotoAdd
        /// </summary>
        /// <param name="photoDescription"></param>
        /// <param name="photoName"></param>
        /// <param name="mime"></param>
        /// <param name="oriFileName"></param>
        /// <param name="photoData"></param>
        /// <param name="passport"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PhotoAdd(string photoDescription, string photoName,string mime,string oriFileName,string photoData, string passport, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenPhotoAdd = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}addUserPhotoInfo?photoDescription={1}photoName&={2}mime&={3}oriFileName&={4}photoData&={5}passport={6}", this.RequestAddress, photoDescription, photoName, mime, oriFileName, photoData, passport);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPhotoAdd, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPhotoAdd.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion
    }
}
