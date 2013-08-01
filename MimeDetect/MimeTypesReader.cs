// ***************************************************************
//  MimeTypesReader   version:  1.0   date: 12/12/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright © 2005 Winista - All Rights Reserved
// ***************************************************************
// 
// ***************************************************************

using System;
using System.Xml;

namespace Winista.Mime
{
    /// <summary>
    /// Summary description for MimeTypesReader.
    /// </summary>
    public sealed class MimeTypesReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFileStream"></param>
        /// <returns></returns>
        internal MimeType[] Read(System.IO.Stream xmlFileStream)
        {
            var document = new System.Xml.XmlDocument();

            document.Load(xmlFileStream);

            return Visit(document);
        }


        /// <summary>Scan through the document. </summary>
		private MimeType[] Visit(XmlDocument document)
		{
			MimeType[] types = null;
			var element = document.DocumentElement;

			if ((element != null) && element.Name.Equals("mime-types"))
			{
				types = ReadMimeTypes(element);
			}
			return types ?? (new MimeType[0]);
		}

		private MimeType[] ReadMimeTypes(XmlElement element)
		{
		    var types = new System.Collections.ArrayList();
			var nodes = element.ChildNodes;

			for (var i = 0; i < nodes.Count; i++)
			{
				var node = nodes.Item(i);
			    if (Convert.ToInt16(node.NodeType) != (short) XmlNodeType.Element) continue;

			    var nodeElement = (XmlElement) node;

			    if (!nodeElement.Name.Equals("mime-type")) continue;

			    var type = ReadMimeType(nodeElement);

			    if (type != null)
			    {
			        types.Add(type);
			    }
			}
            return (MimeType[])SupportUtil.ToArray(types, new MimeType[types.Count]);
		}

		/// <summary>Read Element named mime-type. </summary>
		private MimeType ReadMimeType(XmlElement element)
		{
			string name = null;
			string description = null;
			MimeType type = null;
			var attrs = element.Attributes;

			for (var i = 0; i < attrs.Count; i++)
			{
				var attr = (XmlAttribute) attrs.Item(i);

				if (attr.Name.Equals("name"))
				{
					name = attr.Value;
				}
				else if (attr.Name.Equals("description"))
				{
					description = attr.Value;
				}
			}
			if ((name == null) || (name.Trim().Equals("")))
			{
				return null;
			}
			
			try
			{
                System.Diagnostics.Trace.WriteLine("Mime type:" + name);
				type = new MimeType(name);
			}
			catch (MimeTypeException mte)
			{
				// Mime Type not valid... just ignore it
				System.Diagnostics.Trace.WriteLine(mte + " ... Ignoring!");
				return null;
			}

			type.Description = description;
			
			var nodes = element.ChildNodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes.Item(i);
			    if (Convert.ToInt16(node.NodeType) != (short) XmlNodeType.Element) continue;

			    var nodeElement = (XmlElement) node;
			    if (nodeElement.Name.Equals("ext"))
			    {
			        ReadExt(nodeElement, type);
			    }
			    else if (nodeElement.Name.Equals("magic"))
			    {
			        ReadMagic(nodeElement, type);
			    }
			}

			return type;
		}

		/// <summary>Read Element named ext. </summary>
		private static void  ReadExt(XmlElement element, MimeType type)
		{
			var nodes = element.ChildNodes;
			for (var i = 0; i < nodes.Count; i++)
			{
				var node = nodes.Item(i);
				if (Convert.ToInt16(node.NodeType) == (short) XmlNodeType.Text)
				{
					type.AddExtension(((XmlText) node).Data);
				}
			}
		}

		/// <summary>Read Element named magic. </summary>
		private static void ReadMagic(XmlElement element, MimeType mimeType)
		{
			string offset = null;
			string content = null;
			string type = null;
			var attrs = element.Attributes;

			for (var i = 0; i < attrs.Count; i++)
			{
				var attr = (XmlAttribute) attrs.Item(i);

				if (attr.Name.Equals("offset"))
				{
					offset = attr.Value;
				}
				else if (attr.Name.Equals("type"))
				{
					type = attr.Value;
                    if (string.Compare(type, "byte", true) == 0)
                    {
                        type = "System.Byte";
                    }
				}
				else if (attr.Name.Equals("value"))
				{
					content = attr.Value;
				}
			}

			if ((offset != null) && (content != null))
			{
				mimeType.AddMagic(Int32.Parse(offset), type, content);
			}
		}
	}
}