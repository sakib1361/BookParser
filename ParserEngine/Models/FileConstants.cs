using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Models
{
    class FileConstants
    {

        internal const string TitleReplace = "{CustomTitle}";
        internal const string ContentReplace = "{CustomContent}";
        internal const string AuthorReplace = "{CustomAuthor}";
        internal const string UUIDReplace = "{CustomUUID}";
        internal const string ReferenceReplace = "{CustomRef}";
        internal const string IDReplace = "{Id}";
        internal const string SrcReplace = "{SourcePage}";

        internal const string OpfManifest = @"<item href=""{CustomContent}"" id=""{Id}"" media-type=""application/xhtml+xml""/>";
        internal const string OpfRef = @"<itemref idref=""{Id}""/>";

        internal const string CustomContent = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<html xmlns = ""http://www.w3.org/1999/xhtml"" xmlns:epub=""http://www.idpf.org/2007/ops"">
 <head>
  <title>
  {CustomTitle}
  </title>
 </head>
 <body>
  <h1>
  {CustomTitle}
  </h1>
  <br/>
  <br/>
  <div>
  {CustomContent}
  </div>
 </body>
</html>";
        internal const string OpfContent = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://www.idpf.org/2007/opf"" unique-identifier=""uuid_id"" version=""2.0"">
  <metadata xmlns:calibre=""http://calibre.kovidgoyal.net/2009/metadata"" 
            xmlns:dc=""http://purl.org/dc/elements/1.1/"" 
            xmlns:dcterms=""http://purl.org/dc/terms/"" 
            xmlns:opf=""http://www.idpf.org/2007/opf"" 
            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <dc:title>{CustomTitle}</dc:title>
    <dc:language>bn</dc:language>
    <dc:creator opf:role=""aut"" opf:file-as=""Author"">{CustomAuthor}</dc:creator>
    <dc:contributor opf:role=""bkp""> calibre(3.46.0)[https://calibre-ebook.com]</dc:contributor>
    <dc:identifier id=""uuid_id"" opf:scheme=""uuid"">{CustomUUID}</dc:identifier>
    <dc:identifier opf:scheme=""calibre"">{CustomUUID}</dc:identifier>
    <meta name=""cover"" content=""cover""/>
    <meta name=""calibre:timestamp"" content =""2019-08-15T16:58:42.414000+00:00"" />
    <meta name=""calibre:title_sort"" content =""{CustomTitle}"" />
    <meta name=""calibre:author_link_map"" content=""{{CustomAuthor}}"" />
  </metadata>
  <manifest>
    <item href=""cover.jpg"" id=""cover"" media-type=""image/jpeg""/>
    {CustomContent}
    <item href=""titlepage.xhtml"" id=""titlepage"" media-type=""application/xhtml+xml""/>
    <item href=""toc.ncx"" media-type=""application/x-dtbncx+xml"" id=""ncx""/>
  </manifest>
  <spine toc=""ncx"">
    <itemref idref=""titlepage""/>
    {CustomRef}
  </spine>
  <guide>
    <reference href=""titlepage.xhtml"" type=""cover"" title=""Cover""/>
  </guide>
</package>
";
        internal const string TitleContent = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
        <meta name = ""calibre:cover"" content=""true""/>
        <title>Cover</title>
        <style type = ""text/css"" title=""override_css"">
            @page {padding: 0pt; margin:0pt}
            body { text-align: center; padding:0pt; margin: 0pt; }
        </style>
    </head>
    <body>
        <div>
            <svg xmlns = ""http://www.w3.org/2000/svg""
                 xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" width=""100%"" height=""100%"" viewBox=""0 0 333 500"" preserveAspectRatio=""none"">
                <image width = ""333"" height=""500"" xlink:href=""cover.jpg""/>
            </svg>
        </div>
    </body>
</html>
";
        internal const string TOCContent = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<ncx xmlns=""http://www.daisy.org/z3986/2005/ncx/"" version=""2005-1"" xml:lang=""eng"">
  <head>
    <meta content = ""{CustomUUID}"" name=""dtb:uid""/>
    <meta content = ""2"" name=""dtb:depth""/>
    <meta content = ""calibre (0.8.38)"" name=""dtb:generator""/>
    <meta content = ""0"" name=""dtb:totalPageCount""/>
    <meta content = ""0"" name=""dtb:maxPageNumber""/>
  </head>
  <docTitle>
    <text>{CustomTitle}</text>
  </docTitle>
  <navMap>
    <navPoint id = ""A01501df-ff99-493d-b5da-d3d8e5b56d74"" playOrder=""1"">
      <navLabel>
        <text>Start</text>
      </navLabel>
      <content src = ""titlepage.xhtml""/>
    </navPoint>
{CustomContent}
  </navMap>
</ncx>
";
        internal const string NavMap = @"
<navPoint id = ""{CustomUUID}"" playOrder=""1"">
      <navLabel>
        <text>{CustomTitle}</text>
      </navLabel>
      <content src = ""{SourcePage}""/>
    </navPoint>
";

        internal const string Container = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<container xmlns = ""urn:oasis:names:tc:opendocument:xmlns:container"" version=""1.0"">
<rootfiles>
<rootfile media-type=""application/oebps-package+xml"" full-path=""content.opf""/>
</rootfiles>
</container>";
    }
}
