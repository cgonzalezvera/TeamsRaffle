module ConfigurationMisc

open System
open System.IO

//--Definitions
[<Literal>]
let ConfigFileName="config.txt"
(*
   players=c:\players.txt,
   results=c:\result.txt,

 *)


type LinePlayerConfig = 
    {Key:string;Value:string}
    member this.IsValue = 
        String.IsNullOrEmpty((this.Value))=false

let CurrentPath=System.Environment.CurrentDirectory

let GetVariablesFomConfig()=
    let content=File.ReadAllText(String.Format("{0}\\{1}",CurrentPath,ConfigFileName))
    let result=Array.toList (content.Split(','))
    result

let SeparatePartsTextLine (lineString:string option) =
    match lineString with
        |None->{Key="";Value=""}
        |Some(lineString)->
            let assignLine=Array.toList (lineString.Split('='))
            match assignLine with
            |[key]->{Key=key;Value=""}
            |[key;value]->{Key=key;Value=value}
            |_->{Key="";Value=""}

let findText (textToCompare:string) (text:string) =
    let isOk=text.ToLower().Contains(textToCompare.ToLower())
    isOk

let GetPlayersPathFromConfig()=
    let players=GetVariablesFomConfig() |> List.tryFind (findText "players") 
    SeparatePartsTextLine players
 

        
let GetResultPathFromConfig()=
    let players=GetVariablesFomConfig() |> List.tryFind (findText "results") 
    SeparatePartsTextLine players
 
        
    