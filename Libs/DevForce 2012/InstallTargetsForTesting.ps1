

function getProgramFilesFolder {
  $pf = "${env:ProgramFiles(x86)}"
  if (test-path $pf) { return $pf }
  
  return "$env:ProgramFiles"
}

$cd = Get-location
$pf = getProgramFilesFolder

# Create IdeaBlade folder
$ibDir = "$pf\MSBuild\IdeaBlade\DevForce2012"
if (!(test-path -path $ibDir)) {
  New-Item -path $ibDir -type directory
}

# Copy files in
Copy-Item "$cd\MSBuild\IdeaBlade.DevForce.Common.targets" $ibDir
Copy-Item "$cd\MSBuild\IdeaBlade\DevForce2012\*.*" $ibDir


# Copy the custom targets only if it doesn't exist


$folder = "$pf\MSBuild\v4.0"
# Create folder if doesn't exist
if (!(test-path -path $folder)) {
  New-Item -path $folder -type directory
}

$file = "Custom.After.Microsoft.Common.targets"
if (!(test-path -path "$folder\$file")) {
  Copy-Item "$cd\Targets\$file" $folder
  "Files copied"
  
} else {
  # don't do anything and make dev update manually
  "Files copied, but you may need to update the $folder\$file to add IdeaBlade targets"
}