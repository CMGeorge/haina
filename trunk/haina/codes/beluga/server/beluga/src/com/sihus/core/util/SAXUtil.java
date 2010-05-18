package com.sihus.core.util;

import java.io.IOException;
import java.io.InputStream;
import java.io.StringReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.Attributes;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;
import org.xml.sax.SAXParseException;
import org.xml.sax.XMLReader;
import org.xml.sax.helpers.DefaultHandler;
import org.xml.sax.helpers.XMLReaderFactory;

/**
 * SAX方式操作xml实用类。<br/>
 * 
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-23
 */
public class SAXUtil {

	private static final Log LOG = LogFactory.getLog(SAXUtil.class);

	/**
	 * 获取根节点。<br/>
	 * 
	 * @param url
	 *            xml的URL路径
	 * @return
	 */
	public static Element loadDocument(URL url) {
		Document doc = null;
		try {
			InputSource in = new InputSource(url.openStream());

			DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
					.newInstance();
			DocumentBuilder parser = docBuilderFactory.newDocumentBuilder();
			doc = parser.parse(in);
			Element root = doc.getDocumentElement();
			root.normalize();
			return root;
		} catch (SAXParseException err) {
			LOG.error("\t行:\t" + err.getLineNumber());
			LOG.error("\t列:\t" + err.getColumnNumber());
			LOG.error("\turi:\t" + err.getSystemId());
			LOG.error("\t错误信息:\t" + err.getMessage());
			LOG.error(err.getMessage());
		} catch (SAXException e) {
			LOG.error(e);
		} catch (MalformedURLException mfx) {
			LOG.error(mfx);
		} catch (IOException e) {
			LOG.error(e);
		} catch (Exception e) {
			LOG.error(e);
		}
		return null;
	}

	/**
	 * 获取根节点。<br/>
	 * 
	 * @param in
	 *            xml的输入流
	 * @return
	 */
	public static Element loadDocument(InputStream in) {
		Document doc = null;
		try {
			DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
					.newInstance();
			DocumentBuilder parser = docBuilderFactory.newDocumentBuilder();
			doc = parser.parse(in);
			Element root = doc.getDocumentElement();
			root.normalize();
			return root;
		} catch (SAXParseException err) {
			LOG.error("\t行:\t" + err.getLineNumber());
			LOG.error("\t列:\t" + err.getColumnNumber());
			LOG.error("\turi:\t" + err.getSystemId());
			LOG.error("\t错误信息:\t" + err.getMessage());
			LOG.error(err.getMessage());
		} catch (SAXException e) {
			LOG.error(e);
		} catch (MalformedURLException mfx) {
			LOG.error(mfx);
		} catch (IOException e) {
			LOG.error(e);
		} catch (Exception e) {
			LOG.error(e);
		}
		return null;
	}

	/**
	 * 获取某个子节点的属性。<br/>
	 * 
	 * @param root
	 * @param tagName
	 * @param subTagName
	 * @param attribute
	 * @return
	 */
	public static String getSubTagAttribute(Element root, String tagName,
			String subTagName, String attribute) {
		String returnString = "";
		NodeList list = root.getElementsByTagName(tagName);
		for (int loop = 0; loop < list.getLength(); loop++) {
			Node node = list.item(loop);
			if (node != null) {
				NodeList children = node.getChildNodes();
				for (int innerLoop = 0; innerLoop < children.getLength(); innerLoop++) {
					Node child = children.item(innerLoop);
					if ((child != null) && (child.getNodeName() != null)
							&& child.getNodeName().equals(subTagName)) {
						if (child instanceof Element) {
							return ((Element) child).getAttribute(attribute);
						}
					}
				}
			}
		}
		return returnString;
	}

	/**
	 * 获取子节点的值。<br/>
	 * 
	 * @param node
	 * @param subTagName
	 * @return
	 */
	public static String getSubTagValue(Node node, String subTagName) {
		String returnString = "";
		if (node != null) {
			NodeList children = node.getChildNodes();
			for (int innerLoop = 0; innerLoop < children.getLength(); innerLoop++) {
				Node child = children.item(innerLoop);
				if ((child != null) && (child.getNodeName() != null)
						&& child.getNodeName().equals(subTagName)) {
					Node grandChild = child.getFirstChild();
					if (grandChild.getNodeValue() != null)
						return grandChild.getNodeValue();
				}
			}
		}
		return returnString;
	}

	/**
	 * 获取子节点的值。<br/>
	 * 
	 * @param root
	 * @param tagName
	 * @param subTagName
	 * @return
	 */
	public static String getSubTagValue(Element root, String tagName,
			String subTagName) {
		String returnString = "";
		NodeList list = root.getElementsByTagName(tagName);
		for (int loop = 0; loop < list.getLength(); loop++) {
			Node node = list.item(loop);
			if (node != null) {
				NodeList children = node.getChildNodes();
				for (int innerLoop = 0; innerLoop < children.getLength(); innerLoop++) {
					Node child = children.item(innerLoop);
					if ((child != null) && (child.getNodeName() != null)
							&& child.getNodeName().equals(subTagName)) {
						Node grandChild = child.getFirstChild();
						if (grandChild.getNodeValue() != null)
							return grandChild.getNodeValue();
					}
				}
			}
		}
		return returnString;
	}

	/**
	 * 获取子节点的值。<br/>
	 * 
	 * @param root
	 * @param tagName
	 * @return
	 */
	public static String getTagValue(Element root, String tagName) {
		String returnString = "";
		NodeList list = root.getElementsByTagName(tagName);
		for (int loop = 0; loop < list.getLength(); loop++) {
			Node node = list.item(loop);
			if (node != null) {
				Node child = node.getFirstChild();
				if ((child != null) && child.getNodeValue() != null)
					return child.getNodeValue();
			}
		}
		return returnString;
	}

	public static XMLReader getXmlReader() {
		try {
			return XMLReaderFactory.createXMLReader();
		} catch (final SAXException e) {
			LOG.equals(new RuntimeException("Unable to create XMLReader", e));
			return null;
		}
	}

	public static List<String> getTextForElements(final String xmlAsString,
			final String element) {
		final List<String> elements = new ArrayList<String>(2);
		final XMLReader reader = getXmlReader();

		final DefaultHandler handler = new DefaultHandler() {

			private boolean foundElement = false;

			private StringBuffer buffer = new StringBuffer();

			public void startElement(final String uri, final String localName,
					final String qName, final Attributes attributes)
					throws SAXException {
				if (localName.equals(element)) {
					this.foundElement = true;
				}
			}

			public void endElement(final String uri, final String localName,
					final String qName) throws SAXException {
				if (localName.equals(element)) {
					this.foundElement = false;
					elements.add(this.buffer.toString());
					this.buffer = new StringBuffer();
				}
			}

			public void characters(char[] ch, int start, int length)
					throws SAXException {
				if (this.foundElement) {
					this.buffer.append(ch, start, length);
				}
			}
		};

		reader.setContentHandler(handler);
		reader.setErrorHandler(handler);

		try {
			reader.parse(new InputSource(new StringReader(xmlAsString)));
		} catch (final Exception e) {
			LOG.error(e);
			return null;
		}

		return elements;
	}

	public static String getTextForElement(final String xmlAsString,
			final String element) {
		final XMLReader reader = getXmlReader();
		final StringBuffer buffer = new StringBuffer();

		final DefaultHandler handler = new DefaultHandler() {

			private boolean foundElement = false;

			public void startElement(final String uri, final String localName,
					final String qName, final Attributes attributes)
					throws SAXException {
				if (localName.equals(element)) {
					this.foundElement = true;
				}
			}

			public void endElement(final String uri, final String localName,
					final String qName) throws SAXException {
				if (localName.equals(element)) {
					this.foundElement = false;
				}
			}

			public void characters(char[] ch, int start, int length)
					throws SAXException {
				if (this.foundElement) {
					buffer.append(ch, start, length);
				}
			}
		};

		reader.setContentHandler(handler);
		reader.setErrorHandler(handler);

		try {
			reader.parse(new InputSource(new StringReader(xmlAsString)));
		} catch (final Exception e) {
			LOG.error(e, e);
			return null;
		}

		return buffer.toString();
	}
}
