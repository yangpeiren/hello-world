Sub Savetheattachment()

    Dim olApp As New Outlook.Application

    Dim nmsName As Outlook.NameSpace

    Dim vItem As Object

    Set nmsName = olApp.GetNamespace("MAPI")

    Set myFolder = nmsName.GetDefaultFolder(olFolderInbox)

    Set fldFolder = myFolder       '.Folders("For Download")

         

    For Each vItem In fldFolder.Items

       '-----Save Attachment-------

        For Each att In vItem.Attachments
            If (InStr(att.FileName, "template_205-Daily_report") <> 0) Then
                'MsgBox (att.FileName)
                att.SaveAsFile "D:\Temp\export\" & att.FileName
            End If
        Next

        '------Save Attachment--------

    Next

     

    Set fldFolder = Nothing

    Set nmsName = Nothing

End Sub
