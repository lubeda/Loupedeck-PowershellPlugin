# Loupedeck-PowershellPlugin
User Powershell with Loupedeck
a Plugin that calls a ps1-script every minute or on key press or on button action.

the script should return a json-object like this:

```
@{ indicator = @{
    R= 0,255 | Get-Random
    G = 0 | Get-Random
    B= 0,255 | Get-Random
  }
    text= @(
        @{ text="Hallo","Wallo","Mallo","Krallo" | Get-Random
            fontsize = 15,18,20 | Get-Random
            position = @{
                x=1..50 | Get-Random
                y=1..50 | Get-Random
            }
              color = @{
                R= 0..255 | Get-Random
                G = 0..255 | Get-Random
                B= 0..255 | Get-Random
                A= 0..255 | Get-Random
              }
            }, @{ text="Text 2"
            position = @{
                x=1..20 | Get-Random
                y=10..40 | Get-Random
            }
             color = @{
               r= 100
               g = 200
               b= 100
               a= 100
             }
             fontsize = 20,30,12,18,13 | Get-Random
           })
    backgroundimage = $file
    bgcolor = @{
        r= 100
        g = 200
        b= 100
        a= 100
      }
} 
```

which will them be displayed on your Loupedeck
