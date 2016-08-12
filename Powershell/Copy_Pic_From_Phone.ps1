#Function description: used for copying *.jpg files from windows portable devices, i.e. Iphone ,Android phone, MP3 player etc.

#Please make sure:

#1. your device is connected to your computer 
#2. all target files are manually accessable


#The access to the Windows Portable Devices were copied from https://gist.github.com/cveld/8fa339306f8504095815
#Other use please contact the original creater  Carl in 't Veld https://github.com/cveld

#Peiren Yang 2016-08-10


#The destination to where the pics should be copied 
Set-Variable destination 'D:\temp\'
#The minimum size of the pics, in case there are some thumbnails
Set-Variable Minsize 100KB
#The global counter, to count how many pics were copied
Set-Variable Counter 0 -Scope Global
#The recusive depth, how many layers' folder can be reached
#if not deep enough, please enlarge this number
Set-Variable Maxdepth 10


# http://blogs.technet.com/b/heyscriptingguy/archive/2013/04/26/use-powershell-to-work-with-windows-explorer.aspx
$cp = New-Object -com Shell.Application
$dstfolder = $cp.NameSpace($destination)


# https://msdn.microsoft.com/en-us/library/windows/desktop/bb774096(v=vs.85).aspx
# ShellSpecialFolderConstants.ssfDRIVES == 0x11

$o = New-Object -com Shell.Application
$folder = $o.NameSpace(0x11)

Write-Output 'Here listed the folders which were read from Window, Please type in the Number of your target device, then press Enter!'

$items = $folder.Items()
for ($i= 0; $i -lt $items.Count; $i++) {
    write-output ([string]$i + ": " + $items.Item($i).Name)
}

$choice = Read-Host "Make your choice: "

$android = $items.Item([int]$choice)
$root = $android.GetFolder()

#The main function, which recusively check the folders and copy target files
Function Write-Items($item, $depth, $maxdepth) {
    if ($depth -ge $maxdepth) {
        return;
    }
    
    #
    if ($item.Title) 
    {

        if(($item.Title -like '.*') -or ($item.Title -like '_*'))
        {
            return
        }
     }
     else
     {        
        if($item.Name -like '*.jpg' -and (Get-sizestring $item.Parent.GetDetailsOf($item, 2)) -gt $Minsize)
        {
            $hash = @{
            Name = $item.Name
            Size = $item.Parent.GetDetailsOf($item, 2)
            Modified = $item.Parent.GetDetailsOf($item, 3)
            Parent = $item.Parent.Title
            Level = $depth
            }
            $Object = New-Object PSObject -Property $hash            
            Write-Output $object
            try
            {
                $dstfolder.CopyHere($item,0x418)
                $global:Counter += 1
                #$item.Parent.CopyHere($item,$destination)
            }
            catch
            {
            }
        }
    }
    if ($item.Count -gt 0) {
        # $item is a folder with its own items
        for ($i = 0; $i -lt $item.Count) {
            $item2 = $item.item($i)
            Write-Items $item2 $depth+1 $maxdepth
        }

    }
    else { 
        if ($item.Items) {
            $items = $item.Items()
            if ($items.Count -gt 0) {
                foreach ($i in $items) {
                    if ($i.IsFolder) {
                        $folder = $i.GetFolder()
                        Write-Items $folder ($depth+1) $maxdepth
                    }
                    else {                    
                        Write-Items $i ($depth+1) $maxdepth
                    }
                }
            }
        }
    }
}
#Handle strings like '100 KB' to int 100kb
Function Get-sizestring($string)
{
    $Strings = $string.split(' ')
    if($Strings.Count -ne 2)
    {
        return
    }
    $num = [int]($Strings[0])
    $unit = $Strings[1]
    switch($unit)
    {
        'KB'{return $num * 1000  }
        'MB'{return $num * 1000 * 1000}
        default {return $num}
    }

}

Write-Items $root 0 $Maxdepth
Write-Host 'Finished, please check your pictures at:' + $destination
Write-Host 'Totally ' + $Counter + ' pictures were copied from your phone'
Read-Host 'Press any key to continue...'
Exit
