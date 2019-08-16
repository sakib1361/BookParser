using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Engine
{
    class FileEngine
    {
    }
}

/*
 * <?xml version="1.0" encoding="utf-8" ?>
<package unique-identifier="BookId" version="2.0" xmlns="http://www.idpf.org/2007/opf">
  <metadata xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:opf="http://www.idpf.org/2007/opf" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <dc:subject>Novel</dc:subject>
    <dc:title>দৃষ্টি প্রদীপ</dc:title>
    <dc:contributor opf:role="bkp">Jutoh 1.70 (http://www.jutoh.com)</dc:contributor>
    <dc:language>bn</dc:language>
    <dc:creator opf:role="aut">বিভূতিভূষণ বন্দ্যোপাধ্যায়</dc:creator>
    <dc:subject>Fiction</dc:subject>
    <dc:subject>Children's literature</dc:subject>
    <dc:relation>www.swiftboox.com</dc:relation>
    <dc:publisher>Swiftboox (http://www.swiftboox.com)</dc:publisher>
    <dc:date opf:event="publication">2013-07-15</dc:date>
    <meta content="0.9.5" name="Sigil version"/>
    <dc:date opf:event="modification">2016-04-28</dc:date>
    <dc:identifier id="BookId">Dristi Pradip</dc:identifier>
    <meta name="cover" content="CoverDesign.jpg"/>
  </metadata>
  <manifest>
    <item href="toc.ncx" id="toc_ncx" media-type="application/x-dtbncx+xml"/>
    <item href="Images/CoverDesign.jpg" id="cover" media-type="image/jpeg"/>
    <item href="Text/CoverPage.html" id="CoverPage_html" media-type="application/xhtml+xml"/>
    <item href="Text/TableOfContents.html" id="TableOfContents_html" media-type="application/xhtml+xml"/>
    <item href="Styles/styles.css" id="styles_css" media-type="text/css"/>
    <item href="Text/section-0001.html" id="section-0001_html" media-type="application/xhtml+xml"/>
    <item href="Fonts/kalpurush.ttf" id="kalpurush_ttf" media-type="application/vnd.ms-opentype"/>
  </manifest>
  <spine toc="toc_ncx">
    <itemref idref="CoverPage_html"/>
    <itemref idref="TableOfContents_html"/>
    <itemref idref="section-0001_html"/>
  </spine>
  <guide>
    <reference href="Text/CoverPage.html" title="Cover Page" type="cover"/>
    <reference href="Text/TableOfContents.html#tableofcontents" title="Table of Contents" type="toc"/>
  </guide>
</package>
*/
