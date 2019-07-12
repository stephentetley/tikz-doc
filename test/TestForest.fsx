﻿// Copyright (c) Stephen Tetley 2019
// License: BSD 3 Clause

#r "netstandard"

#I @"C:\Users\stephen\.nuget\packages\slformat\1.0.2-alpha-20190712\lib\netstandard2.0"
#r "SLFormat.dll"

#load "..\src\TikZDoc\Internal\Common.fs"
#load "..\src\TikZDoc\Internal\Invoke.fs"
#load "..\src\TikZDoc\Internal\Syntax.fs"
#load "..\src\TikZDoc\Base\GenLaTeX.fs"
#load "..\src\TikZDoc\Base\LaTeX.fs"
#load "..\src\TikZDoc\Base\TikZBase.fs"
#load "..\src\TikZDoc\Base\Properties\Misc.fs"
#load "..\src\TikZDoc\Base\Properties\Colors.fs"
#load "..\src\TikZDoc\Extensions\Forest\Forest.fs"

open System.IO
open TikZDoc.Base
open TikZDoc.Base.Properties
open TikZDoc.Extensions.Forest


let workingDirectory = Path.Combine(__SOURCE_DIRECTORY__, "..", "output")

let fillProp (value : GenLaTeX<'a>) : TikZProperty = 
    rawtext "fill" ^=^ value
     
let directioni (i : int) = latexInt i


let makeNode (color : TikZProperty) 
             (label : GenLaTeX<'a>) 
             (kids : ForestTikZ list) : ForestTikZ =
    forestNode (addNodeArgs label [ fillProp color ]) kids

let redNode (label : GenLaTeX<'a>) (kids : ForestTikZ list) : ForestTikZ =
    makeNode (Colors.modify Colors.red 25) label kids 

let greenNode (label : GenLaTeX<'a>) (kids : ForestTikZ list) : ForestTikZ =
    makeNode (Colors.modify Colors.green 25) label kids

let plainNode (label : GenLaTeX<'a>) (kids : ForestTikZ list) : ForestTikZ =
    forestNode label kids

let forestDoc () = 
    forestDocument ["edges"] 
        <| vcat [ forTree [growTick <| latexInt 0; folderProp; drawProp]
                ; plainNode (text "factx-fsharp") 
                    [ plainNode (text "src") [] 
                    ; plainNode (text "test") []
                    ]
                ]

let test01 () = 
    let doc = forestDoc ()
    printfn "%s" workingDirectory
    doc.Render 180 |> printfn "%s"
    doc.SaveToSVG(workingDirectory, "forest1-svg.svg")
    doc.SaveToPS(workingDirectory, "forest1-ps.ps")

