// include Fake lib
#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let mode = getBuildParamOrDefault "mode" "Release"
let buildDir = "./bin/" + mode + "/"

// Targets
Target "Clean" (fun _ -> 
        CleanDir buildDir
)

Target "Build" (fun _ ->
        "Building " + mode + " configuration" |> trace
        !! "src/**/*.csproj"
            |>
            match mode.ToLower() with
                | "release" -> MSBuildRelease buildDir "Build"
                | _ -> MSBuildDebug buildDir "Build"
            |> Log "AppBuild-Output: "
)

Target "Default" (fun _ ->
    ()
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Default"

// start build
RunTargetOrDefault "Default"