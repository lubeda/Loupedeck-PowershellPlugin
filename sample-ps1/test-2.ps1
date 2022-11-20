$proc = get-process
$sz = 80
$bmp = [byte[]]::new($sz*$sz)  
for ($i = 0; $i -lt $sz; $i++)
{
   for ($j = 0; $j -lt $sz; $j += 2)
   {
     $bmp[$i*$sz+$j] = 0..255 | Get-Random
   }
}
@{"Text" = "$($proc.count) ==> $($sz)"
"con" = $bmp
}