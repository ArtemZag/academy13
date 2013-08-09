// ***************************************************************
//  MimeTypes   version:  1.0   Date: 12/12/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright © 2005 - Winista All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Reflection;

namespace Winista.Mime
{
	/// <summary>
	/// Summary description for MimeTypes.
	/// </summary>
	public sealed class MimeTypes
    {
        #region Class Members
        /// <summary>The default <code>application/octet-stream</code> MimeType </summary>
		public const string DEFAULT = "application/octet-stream";
		
		/// <summary>All the registered MimeTypes </summary>
		private readonly ArrayList _types = new ArrayList();
		
		/// <summary>All the registered MimeType indexed by name </summary>
		private readonly Hashtable _typesIdx = new Hashtable();

		/// <summary>MimeTypes indexed on the file extension </summary>
		private readonly IDictionary _extIdx = new Hashtable();
		
		/// <summary>List of MimeTypes containing a magic char sequence </summary>
		private readonly IList _magicsIdx = new ArrayList();
		
		/// <summary>The minimum length of data to provide to check all MimeTypes </summary>
		private int _minLength = 0;
       

        /// <summary> My registered instances
		/// There is one instance associated for each specified file while
		/// calling the {@link #get(String)} method.
		/// Key is the specified file path in the {@link #get(String)} method.
		/// Value is the associated MimeType instance.
		/// </summary>
		private static System.Collections.IDictionary instances = new System.Collections.Hashtable();
        #endregion

        /// <summary>
        /// Read mime types from embedded resource file 'mime-types.xml'
        /// </summary>
	    public MimeTypes()
	    {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Winista.Mime.mime-types.xml";

            var stream = assembly.GetManifestResourceStream(resourceName);

            this.Add(GeMimeTypes(stream));
	    }

        /// <summary>Should never be instanciated from outside </summary>
		public MimeTypes(string strFilepath)
        {
            using (var fileStream = new FileStream(strFilepath, FileMode.Open, FileAccess.Read))
            {
                this.Add(GeMimeTypes(fileStream));
            }
        }

	    public MimeTypes(Stream xmlFileStream)
	    {
            this.Add(GeMimeTypes(xmlFileStream));
	    }

        private static MimeType[] GeMimeTypes(Stream xmlFileStream)
	    {
            var reader = new MimeTypesReader();
            return reader.Read(xmlFileStream);
	    }

		/// <summary> Return the minimum length of data to provide to analyzing methods
		/// based on the document's content in order to check all the known
		/// MimeTypes.
		/// </summary>
		/// <returns> the minimum length of data to provide.
		/// </returns>
		public int MinLength
		{
			get
			{
				return _minLength;
			}
			
		}

		/// <summary> Return a MimeTypes instance.</summary>
		/// <param name="filepath">is the mime-types definitions xml file.
		/// </param>
		/// <returns> A MimeTypes instance for the specified filepath xml file.
		/// </returns>
		public static MimeTypes Get(string filepath)
		{
			MimeTypes instance = null;
			lock (instances.SyncRoot)
			{
				instance = (MimeTypes) instances[filepath];
				if (instance == null)
				{
					//instance = new MimeTypes(filepath, null);
					instance = new MimeTypes(filepath);
					instances[filepath] = instance;
				}
			}
			return instance;
		}

		/// <summary> Find the Mime Content Type of a document from its URL.</summary>
		/// <param name="url">of the document to analyze.
		/// </param>
		/// <returns> the Mime Content Type of the specified document URL, or
		/// <code>null</code> if none is found.
		/// </returns>
		public MimeType GetMimeType(Uri url)
		{
			return GetMimeType(url.AbsolutePath);
		}

		/// <summary> Find the Mime Content Type of a document from its name.</summary>
		/// <param name="name">of the document to analyze.
		/// </param>
		/// <returns> the Mime Content Type of the specified document name, or
		/// <code>null</code> if none is found.
		/// </returns>
		public MimeType GetMimeType(string name)
		{
			var founds = GetMimeTypes(name);

			if ((founds == null) || (founds.Length < 1))
			{
				// No mapping found, just return null
				return null;
			}
		    
            // Arbitraly returns the first mapping
		    return founds[0];
		}

		/// <summary> Find the Mime Content Type of a stream from its content.
		/// 
		/// </summary>
		/// <param name="data">are the first bytes of data of the content to analyze.
		/// Depending on the length of provided data, all known MimeTypes are
		/// checked. If the length of provided data is greater or egals to
		/// the value returned by {@link #getMinLength()}, then all known
		/// MimeTypes are checked, otherwise only the MimeTypes that could be
		/// analyzed with the length of provided data are analyzed.
		/// 
		/// </param>
		/// <returns> The Mime Content Type found for the specified data, or
		/// <code>null</code> if none is found.
		/// </returns>
		/// <seealso cref="#getMinLength()">
		/// </seealso>
		public MimeType GetMimeType(sbyte[] data)
		{
			// Preliminary checks
			if ((data == null) || (data.Length < 1))
			{
				return null;
			}
			var iter = _magicsIdx.GetEnumerator();
		    // TODO: This is a very naive first approach (scanning all the magic)
			//       bytes since one is matching.
			//       A first improvement could be to use a search path on the magic
			//       bytes.
			// TODO: A second improvement could be to search for the most qualified
			//       (the longuest) magic sequence (not the first that is matching).
			while (iter.MoveNext())
			{
			    var type = (MimeType) iter.Current;

			    if (type.Matches(data))
				{
					return type;
				}
			}

		    return null;
		}

	    public MimeType GetFileMimeType(string fileName)
	    {
            sbyte[] fileData;

	        using (var srcFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
	        {
	            var data = new byte[srcFile.Length];
	            srcFile.Read(data, 0, (Int32) srcFile.Length);
	            fileData = SupportUtil.ToSByteArray(data);
	        }

	        return this.GetMimeType(fileData);
	    }

		/// <summary> Find the Mime Content Type of a document from its name and its content.
		/// 
		/// </summary>
		/// <param name="name">of the document to analyze.
		/// </param>
		/// <param name="data">are the first bytes of the document's content.
		/// </param>
		/// <returns> the Mime Content Type of the specified document, or
		/// <code>null</code> if none is found.
		/// </returns>
		/// <seealso cref="#getMinLength()">
		/// </seealso>
		public MimeType GetMimeType(string name, sbyte[] data)
		{
			
			// First, try to get the mime-type from the name
			MimeType mimeType = null;
			var mimeTypes = GetMimeTypes(name);

			if (mimeTypes == null)
			{
				// No mime-type found, so trying to analyse the content
				mimeType = GetMimeType(data);
			}
			else if (mimeTypes.Length > 1)
			{
				// TODO: More than one mime-type found, so trying magic resolution
				// on these mime types
				//mimeType = getMimeType(data, mimeTypes);
				// For now, just get the first one
				mimeType = mimeTypes[0];
			}
			else
			{
				mimeType = mimeTypes[0];
			}
			return mimeType;
		}

		/// <summary> Return a MimeType from its name.</summary>
		public MimeType ForName(string name)
		{
			return (MimeType) _typesIdx[name];
		}

		/// <summary> Add the specified mime-types in the repository.</summary>
		/// <param name="mimeTypes">are the mime-types to add.
		/// </param>
		internal void Add(MimeType[] mimeTypes)
		{
		    if (mimeTypes == null)
			{
				return;
			}

		    foreach (var type in mimeTypes)
		    {
		        Add(type);
		    }
		}

	    /// <summary> Add the specified mime-type in the repository.</summary>
		/// <param name="type">is the mime-type to add.
		/// </param>
		internal void Add(MimeType type)
		{
			_typesIdx[type.Name] = type;
			_types.Add(type);

			// Update minLentgth
			_minLength = System.Math.Max(_minLength, type.MinLength);

			// Update the extensions index...
			var exts = type.Extensions;

			if (exts != null)
			{
				foreach (string ext in exts)
				{
				    var list = (IList) _extIdx[ext];

				    if (list == null)
				    {
				        // No type already registered for this extension...
				        // So, create a list of types
				        list = new ArrayList();
				        _extIdx[ext] = list;
				    }
				    list.Add(type);
				}
			}
			// Update the magics index...
			if (type.HasMagic())
			{
				_magicsIdx.Add(type);
			}
		}

		/// <summary> Returns an array of matching MimeTypes from the specified name
		/// (many MimeTypes can have the same registered extensions).
		/// </summary>
		private MimeType[] GetMimeTypes(System.String name)
		{
			IList mimeTypes = null;

			var index = name.LastIndexOf('.');

			if ((index != - 1) && (index != name.Length - 1))
			{
				// There's an extension, so try to find
				// the corresponding mime-types
				var ext = name.Substring(index + 1);
				mimeTypes = (IList) _extIdx[ext];
			}
			
			return (mimeTypes != null)
                ? (MimeType[]) SupportUtil.ToArray(mimeTypes, new MimeType[mimeTypes.Count])
                : null;
		}
	}
}