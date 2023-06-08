# Loupedeck-PowershellPlugin
**Use PowerShell with Loupedeck**

## *!!!! This is a beta version!!!!*

---
**To all developers:**
I think this is the most universal plugin “ever”, i can't develop it much further because of the lack of skill and time. Perhaps one can join and make this more professional and robust.
---

A Plugin that calls a ps1-script every minute or on key press or on button action.

The script should return a JSON-object like this:

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

Which will then be displayed on your Loupedeck. The indicator is an optional colored triangle on the upper-left side.
The background image is an 80x80 PNG file from your computer, or a simple background color is displayed.
There are two optional texts that can be displayed.

On each call of the ps1-file, there is a parameter named $mode.

|value|action|
|---|---|
|trigger|The key was pressed|
|left|the knob was turned left|
|right|the knob was turned right|
|click|the knob was clicked|
|refresh|call every minute without action|

The sample file results in toggling a light with home assistant and displays this
![Image](https://github.com/lubeda/Loupedeck-PowershellPlugin/blob/main/samples/test_Homeassistant.png?raw=true)

