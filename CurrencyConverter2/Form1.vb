Imports System.Xml
Imports System.IO
Imports System.Net

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Create a WebRequest to the remote site

        'www.floatrates.com/daily/usd/xml
        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://www.floatrates.com/daily/usd.xml")

        '' NB! Use the following line ONLY if the website is protected
        '' request.Credentials = New System.Net.NetworkCredential("username", "password")

        '' Call the remote site, and parse the data in a response object
        Dim response As System.Net.HttpWebResponse = request.GetResponse()

        '' Create a new, empty XML document
        Dim doc As New XmlDocument()

        '' Check if the response is OK (status code 200)
        If response.StatusCode = System.Net.HttpStatusCode.OK Then

            ' Parse the contents from the response to a stream object
            Dim stream As System.IO.Stream = response.GetResponseStream()
            ' Create a reader for the stream object
            Dim reader As New System.IO.StreamReader(stream)
            ' Read from the stream object using the reader, put the contents in a string
            Dim contents As String = reader.ReadToEnd()

            ' Load the contents into the XML document
            doc.LoadXml(contents)

            Dim currencyDict As New Dictionary(Of String, Single)
            Dim targetCurrency As String = ""
            Dim exchangeRate As Single = 0
            Dim desiredCurrency() = {"EUR", "GBP", "JPY", "AUD", "ZAR"}
            Dim nodes As XmlNodeList = doc.DocumentElement.SelectNodes("item")

            For Each node As XmlNode In nodes
                targetCurrency = node.SelectSingleNode("targetCurrency").InnerText
                If desiredCurrency.Contains(targetCurrency) Then
                    exchangeRate = node.SelectSingleNode("exchangeRate").InnerText
                    currencyDict.Add(targetCurrency, exchangeRate)
                End If
            Next

            Dim dt As DataTable = New DataTable
            dt.Columns.Add("Currency")
            dt.Columns.Add("USD to")
            dt.Columns.Add("USD from")
            Dim pair As KeyValuePair(Of String, Single)
            For Each pair In currencyDict
                dt.Rows.Add(pair.Key, pair.Value.ToString("N2"), (1 / pair.Value).ToString("N2"))
            Next

            DataGrid1.DataSource = dt

        End If


        ' Now you have a XmlDocument object that contains the XML from the remote site, you can
        ' use the objects and methods in the System.Xml namespace to read the document

    End Sub


End Class

