param ($mode)
$token = "longlivedtoken"
$apiurl = "http://homeassistant.local:8123/api/"
$service = "light.toggle"
$entity = "light.mli_tint_colortemperature_light"

$background = "C:\Users\lubeda\Desktop\light bg.png"

function Invoke-HAService {
    param (
        $service, $token, $url, $entity
    )
    Invoke-RestMethod -Uri "$($url)services/$($service.Replace(".","/"))" -Method Post -Headers @{authorization = "Bearer $token"; "content-type" = 'application/json' } -Body (@{entity_id = $entity } | ConvertTo-Json)
}

function Get-HASState {
    param (
        $token, $url, $entity
    )
    $uri = "$($url)states/$($entity)"
    try {
        Invoke-RestMethod -Uri $uri -Method get -Headers @{authorization = "Bearer $token"; "content-type" = 'application/json' }    
    }
    catch {
        $null
    }
}

if ($mode -eq "trigger") {
    Invoke-HAService -url $apiurl -service $service -token  $token -entity $entity | Out-Null
}

$state = Get-HASState -url $apiurl -token  $token -entity $entity
$text = $state.attributes.friendly_name

if ($null -ne $state) {
    if ($state.state -eq "on") {
        $indcator = @{
            B = 0
            R = 0
	        G = 200 
        }
    }
    else {
        $indcator = @{
            R = 255
            G = 0
            B = 0
        }
    }
}    

$color = @{
    R = 25 
    G = 25
    B = 2 
}

$font1 = 12
$pos1 = @{
    x = 0
    y = 60
}
$text1 = $text

$pos2 = @{
    x = 0
    y = 28
}

$color2 = @{
    R = 15 
    G = 0
    B = 0 
}

@{ text             = @(
        @{ text      = $text1
            fontsize = $font1
            position = $pos1
            color    = $color
        })
    indicator       = $indcator
    backgroundimage = $background
    bgcolor         = @{
        r = 180
        g = 0
        b = 20
    }
} 