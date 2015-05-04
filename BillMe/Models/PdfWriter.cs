using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using Pechkin;

namespace BillMe.Models
{
    public static class PdfWriter
    {

        public static Boolean WriteFacture(Bill bill, String path, Enterprise entreprise)
        {
            var html = writeHeaderFacture(entreprise, bill) + writeBodyFacture(bill) + writeFooterFacture();
            return htmlToPDF(path, html);
        }

        public static Boolean WriteDevis(Bill bill, String path, Enterprise entreprise)
        {
            var html = writeHeaderDevis(entreprise, bill) + writeBodyDevis(bill) + writeFooterDevis();
            return htmlToPDF(path, html);
        }

        private static Boolean htmlToPDF(String path, String html)
        {
            Document document = new Document(iTextSharp.text.PageSize.A4);
            var writer = new BinaryWriter(new FileStream(path, FileMode.Create), Encoding.UTF8);
            try
            {
                document.Open();

                //Transform the HTML into PDF
                var pechkin = new SimplePechkin(new GlobalConfig());
                var pdf = pechkin.Convert(new ObjectConfig()
                    .SetLoadImages(true).SetZoomFactor(3)
                    .SetPrintBackground(true)
                    .SetScreenMediaType(true)
                    .SetCreateExternalLinks(true), html);
                writer.Write(pdf);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                document.Close();
                writer.Flush();
                writer.Close();
            }
            return true;
        }

        //===================HTML=========================

        private static String writeHeaderFacture(Enterprise enterprise, Bill bill)
        {
            var html = @"<html>

<head>
<meta charset=""utf8""/>
<style>
<!--
 /* Font Definitions */
 @font-face
	{font-family:""Cambria Math"";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
h1
	{margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:20.0pt;
	font-family:""Tahoma"",sans-serif;
	color:gray;
	letter-spacing:.2pt;}
h2
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	text-transform:uppercase;
	letter-spacing:.2pt;}
h3
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:9.0pt;
	margin-left:0cm;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:normal;
	font-style:italic;}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
p.Companyname, li.Companyname, div.Companyname
	{mso-style-name:""Company name"";
	margin-top:7.0pt;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:12.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
p.ColumnHeading, li.ColumnHeading, div.ColumnHeading
	{mso-style-name:""Column Heading"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:center;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
p.RightAligned, li.RightAligned, div.RightAligned
	{mso-style-name:""Right Aligned"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	text-transform:uppercase;
	letter-spacing:.2pt;}
p.Amount, li.Amount, div.Amount
	{mso-style-name:Amount;
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
p.Thankyou, li.Thankyou, div.Thankyou
	{mso-style-name:""Thank you"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:center;
	line-height:110%;
	font-size:10.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
.MsoChpDefault
	{font-size:10.0pt;}
@page WordSection1
	{size:612.0pt 792.0pt;
	margin:36.0pt 36.0pt 36.7pt 36.0pt;}
div.WordSection1
	{page:WordSection1;}
 /* List Definitions */
 ol
	{margin-bottom:0cm;}
ul
	{margin-bottom:0cm;}
-->
</style>

</head>

<body lang=FR>

<div class=WordSection1>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:39.75pt'>
  <td width=902 rowspan=2 valign=top style='width:270.6pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <p class=Companyname>" + enterprise.Name + @"</p>
  <h3></h3>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal>Téléphone&nbsp;:" + enterprise.Telephone + @"<br/>Mail&nbsp;:" + enterprise.Mail +
  @"</p>
  <p class=MsoNormal>Adresse&nbsp;:" + enterprise.Address + @"</p>
  </td>
  <td width=898 valign=top style='width:269.4pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <h1>FACTURE</h1>
  </td>
 </tr>
 <tr style='height:39.75pt'>
  <td width=898 valign=bottom style='width:269.4pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <p class=RightAligned>FACTURE n<span lang=EN-US style='font-size:10.0pt;
  line-height:110%;font-family:""Arial"",sans-serif;color:black'>°" + bill.Id + @" </span><span
  lang=EN-US> </span></p>
  <p class=RightAligned>Date&nbsp;: "+ bill.Date + @"</p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:72.0pt'>
  <td width=900 valign=top style='width:270.0pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:72.0pt'>
  <h2>À l’attention de&nbsp;:" + bill.Client + @"</h2>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  </td>
  <td width=900 valign=top style='width:270.0pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:72.0pt'>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>";
            return html;
        }



        private static String writeBodyFacture(Bill bill)
        {
            var html = @"<div align=center>

<table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse;border:none'>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:solid windowtext 1.0pt;border-top:
  solid windowtext 1.5pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=ColumnHeading>DÉSIGNATION</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:solid windowtext 1.5pt;
  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=ColumnHeading>MONTANT HT</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>Nature et quantité des biens livrés ou étendue des
  services fournis&nbsp;:</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>TVA applicable au taux de &nbsp;:</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:solid windowtext 1.0pt;border-top:
  none;padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:none;border-left:none;
  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=RightAligned>TOTAL TTC</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:none;border-left:none;
  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
</table>

</div>";
            return html;
        }


        private static String writeFooterFacture()
        {
            var html = @"<p class=MsoNormal>&nbsp;</p>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:109.9pt'>
  <td width=1800 valign=bottom style='width:540.0pt;padding:5.75pt 5.75pt 5.75pt 5.75pt;
  height:109.9pt'>
  <p class=MsoNormal>Veuillez établir les chèques à l’ordre de </p>
  <p class=MsoNormal>Les règlements doivent être effectués dans les 30 jours.</p>
  <p class=MsoNormal>Pour toute question relative à cette facture, veuillez
  contacter </p>
  <p class=MsoNormal></p>
  </td>
 </tr>
 <tr style='height:21.6pt'>
  <td width=1800 style='width:540.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:21.6pt'>
  <p class=Thankyou>Avec nos remerciements&nbsp;!</p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

</div>

</body>

</html>";
            return html;
        }

        private static String writeHeaderDevis(Enterprise enterprise, Bill bill)
        {
            var html = @"<html>

<head>
<meta charset=""utf8""/>
<style>
<!--
 /* Font Definitions */
 @font-face
	{font-family:""Cambria Math"";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
h1
	{margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:20.0pt;
	font-family:""Tahoma"",sans-serif;
	color:gray;
	letter-spacing:.2pt;}
h2
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	text-transform:uppercase;
	letter-spacing:.2pt;}
h3
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:9.0pt;
	margin-left:0cm;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:normal;
	font-style:italic;}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{margin:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
p.Companyname, li.Companyname, div.Companyname
	{mso-style-name:""Company name"";
	margin-top:7.0pt;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:0cm;
	margin-bottom:.0001pt;
	line-height:110%;
	font-size:12.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
p.ColumnHeading, li.ColumnHeading, div.ColumnHeading
	{mso-style-name:""Column Heading"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:center;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
p.RightAligned, li.RightAligned, div.RightAligned
	{mso-style-name:""Right Aligned"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:8.0pt;
	font-family:""Tahoma"",sans-serif;
	text-transform:uppercase;
	letter-spacing:.2pt;}
p.Amount, li.Amount, div.Amount
	{mso-style-name:Amount;
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:right;
	line-height:110%;
	font-size:8.5pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;}
p.Thankyou, li.Thankyou, div.Thankyou
	{mso-style-name:""Thank you"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:center;
	line-height:110%;
	font-size:10.0pt;
	font-family:""Tahoma"",sans-serif;
	letter-spacing:.2pt;
	font-weight:bold;}
.MsoChpDefault
	{font-size:10.0pt;}
@page WordSection1
	{size:612.0pt 792.0pt;
	margin:36.0pt 36.0pt 36.7pt 36.0pt;}
div.WordSection1
	{page:WordSection1;}
 /* List Definitions */
 ol
	{margin-bottom:0cm;}
ul
	{margin-bottom:0cm;}
-->
</style>

</head>

<body lang=FR>

<div class=WordSection1>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:39.75pt'>
  <td width=902 rowspan=2 valign=top style='width:270.6pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <p class=Companyname>" + enterprise.Name + @"</p>
  <h3></h3>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal>Téléphone&nbsp;:" + enterprise.Telephone + @"<br/>Mail&nbsp;:" + enterprise.Mail +
  @"</p>
  <p class=MsoNormal>Adresse&nbsp;:" + enterprise.Address + @"</p>
  </td>
  <td width=898 valign=top style='width:269.4pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <h1>Devis</h1>
  </td>
 </tr>
 <tr style='height:39.75pt'>
  <td width=898 valign=bottom style='width:269.4pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:39.75pt'>
  <p class=RightAligned>Devis n<span lang=EN-US style='font-size:10.0pt;
  line-height:110%;font-family:""Arial"",sans-serif;color:black'>°" + bill.Id + @" </span><span
  lang=EN-US> </span></p>
  <p class=RightAligned>Date&nbsp;: " + bill.Date + @"</p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:72.0pt'>
  <td width=900 valign=top style='width:270.0pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:72.0pt'>
  <h2>À l’attention de&nbsp;:" + bill.Client + @"</h2>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  </td>
  <td width=900 valign=top style='width:270.0pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:72.0pt'>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  <p class=MsoNormal></p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>";
            return html;
        }

        private static String writeBodyDevis(Bill bill)
        {
            var html = @"<div align=center>

<table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse;border:none'>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:solid windowtext 1.0pt;border-top:
  solid windowtext 1.5pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=ColumnHeading>DÉSIGNATION</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:solid windowtext 1.5pt;
  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=ColumnHeading>MONTANT HT</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>Nature et quantité des biens livrés ou étendue des
  services fournis&nbsp;:</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>TVA applicable au taux de &nbsp;:</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border-top:none;border-left:solid windowtext 1.0pt;
  border-bottom:none;border-right:solid windowtext 1.0pt;padding:2.15pt 5.75pt 2.15pt 5.75pt;
  height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:solid windowtext 1.0pt;border-top:
  none;padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=MsoNormal>&nbsp;</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:none;border-left:none;
  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
 <tr style='page-break-inside:avoid;height:14.4pt'>
  <td width=1290 style='width:387.0pt;border:none;border-right:solid windowtext 1.0pt;
  padding:2.15pt 5.75pt 2.15pt 5.75pt;height:14.4pt'>
  <p class=RightAligned>TOTAL TTC</p>
  </td>
  <td width=510 style='width:153.0pt;border-top:none;border-left:none;
  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
  padding:2.15pt 10.8pt 2.15pt 10.8pt;height:14.4pt'>
  <p class=Amount>&nbsp;</p>
  </td>
 </tr>
</table>

</div>";
            return html;
        }


        private static String writeFooterDevis()
        {
            var html = @"<p class=MsoNormal>&nbsp;</p>

<div align=center>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1800
 style='width:540.0pt;border-collapse:collapse'>
 <tr style='height:109.9pt'>
  <td width=1800 valign=bottom style='width:540.0pt;padding:5.75pt 5.75pt 5.75pt 5.75pt;
  height:109.9pt'>
  <p class=MsoNormal>Veuillez établir les chèques à l’ordre de </p>
  <p class=MsoNormal>Les règlements doivent être effectués dans les 30 jours.</p>
  <p class=MsoNormal>Pour toute question relative à cette facture, veuillez
  contacter </p>
  <p class=MsoNormal></p>
  </td>
 </tr>
 <tr style='height:21.6pt'>
  <td width=1800 style='width:540.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:21.6pt'>
  <p class=Thankyou>Avec nos remerciements&nbsp;!</p>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

</div>

</body>

</html>";
            return html;
        }

    }
}
