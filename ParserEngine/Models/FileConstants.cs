using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Models
{
    class FileConstants
    {
        internal const string CustomContent = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE html><html xmlns = ""http://www.w3.org/1999/xhtml"" xmlns:epub=""http://www.idpf.org/2007/ops"">
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
        internal const string OpfContent = @"<?xml version='1.0' encoding='utf-8'?>
<package xmlns=""http://www.idpf.org/2007/opf"" version=""2.0"" unique-identifier=""uuid_id"">
  <metadata xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
			xmlns:opf=""http://www.idpf.org/2007/opf"" 
			xmlns:dcterms=""http://purl.org/dc/terms/""
			xmlns:calibre=""http://calibre.kovidgoyal.net/2009/metadata"" 
			xmlns:dc=""http://purl.org/dc/elements/1.1/"">
    <dc:language>en</dc:language>
    <dc:creator opf:file-as=""CustomAuthor"" opf:role=""aut"">CustomAuthor</dc:creator>
    <meta name = ""calibre:timestamp"" content=""2012-11-28T19:43:58.426478+00:00""/>
    <dc:title>CustomTitle</dc:title>
    <meta name = ""cover"" content=""cover""/>
    <dc:contributor opf:role=""bkp"">calibre(0.8.38) [http://calibre-ebook.com]</dc:contributor>
    <dc:identifier id=""uuid_id"" opf:scheme=""CustomUUID</dc:identifier>
    <dc:subject>antique</dc:subject>
  </metadata>
  <manifest>
    <item href=""cover.jpg"" id=""cover"" media-type=""image/jpeg""/>
    <item href=""index_split_000.xhtml"" id=""id54"" media-type=""application/xhtml+xml""/>
    <CustomContent>
    <item href=""titlepage.xhtml"" id=""titlepage"" media-type=""application/xhtml+xml""/>
    <item href=""toc.ncx"" media-type=""application/x-dtbncx+xml"" id=""ncx""/>
  </manifest>
  <spine toc=""ncx"">
    <itemref idref=""titlepage""/>
    <itemref idref=""id54""/>
    <CustomRef>
  </spine>
  <guide>
    <reference href=""titlepage.xhtml"" type=""cover"" title=""Cover""/>
  </guide>
</package>
";
        internal const string TitleContent = @"
<?xml version='1.0' encoding='utf-8'?>
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
<?xml version='1.0' encoding='utf-8'?>
<ncx xmlns=""http://www.daisy.org/z3986/2005/ncx/"" version=""2005-1"" xml:lang=""eng"">
  <head>
    <meta content = ""e56c8a27-03e6-401f-99a7-0fde950f3211"" name=""dtb:uid""/>
    <meta content = ""2"" name=""dtb:depth""/>
    <meta content = ""calibre (0.8.38)"" name=""dtb:generator""/>
    <meta content = ""0"" name=""dtb:totalPageCount""/>
    <meta content = ""0"" name=""dtb:maxPageNumber""/>
  </head>
  <docTitle>
    <text>Bloodline</text>
  </docTitle>
  <navMap>
    <navPoint id = ""501501df-ff99-493d-b5da-d3d8e5b56d74"" playOrder=""1"">
      <navLabel>
        <text>Start</text>
      </navLabel>
      <content src = ""titlepage.xhtml""/>
    </navPoint>
  </navMap>
</ncx>
";
    }
}
