# Loupedeck-PowershellPlugin
**Use Powershell with Loupedeck**

## *!!!!This is a beta version!!!!*

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

which will them be displayed on your Loupedeck. The indicator is an optional colored triangle on the upper left side.
The backgroundimage is a 80x80 png file from ypur computer or a simple background coplor is displayed.
there a two optional texts tha can be displayed.

on each call of the ps1-file there is a parameter named $mode.

|value|action|
|---|---|
|trigger|The key was pressed|
|left|the knob was turned left|
|right|the knob was turned right|
|click|the knob was clicked|
|refresh|call every minute without action|

The sample file results in toggling a light with homeassistant and displays this
![.\samples\test_Homeassistant.png]

