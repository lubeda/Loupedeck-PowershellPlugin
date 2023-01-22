param ($mode)
$file = 'C:\Users\lubeda\Desktop\Test.jpg', 'C:\Users\lubeda\Desktop\Test50.png', 'C:\Users\lubeda\Desktop\Test100.png' | Get-Random

@{ indicator      = @{
    R = 0, 255 | Get-Random
    G = 0 | Get-Random
    B = 0, 255 | Get-Random
    A = 0..255 | Get-Random
  }
  text            = @(
    @{ text    = $mode + ' => ' + 'Hallo', 'Test', 'Power' | Get-Random
      fontsize = 15, 18, 20 | Get-Random
      position = @{
        x = 1..50 | Get-Random
        y = 1..50 | Get-Random
      }
      color    = @{
        R = 0..255 | Get-Random
        G = 0..255 | Get-Random
        B = 0..255 | Get-Random
        A = 0..255 | Get-Random
      }
    }, @{ text = 'Text 2'
      position = @{
        x = 1..20 | Get-Random
        y = 10..40 | Get-Random
      }
      color    = @{
        r = 100
        g = 200
        b = 100
        a = 100
      }
      fontsize = 20, 30, 12, 18, 13 | Get-Random
    })
  backgroundimage = $file
  bgcolor         = @{
    r = 100
    g = 200
    b = 100
    a = 100
  }
}