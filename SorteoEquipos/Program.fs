//Author: Claudio E.Gonzalez Vera

open System
open System.IO
open Calculations

// By that function the program start
[<EntryPoint>]
let main argv = 
    // ===Initial assertions=====
    if argv.Length < 2 then failwith "Faltan los parametros: archivo_jugadores y archivo_resultado."
    let pathFile = argv.[0]
    let pathResult = argv.[1]
    if File.Exists(pathFile) = false then failwith "El archivo de players no se encuentra en el directorio indicado."
    if Directory.Exists(Path.GetDirectoryName(pathResult)) = false then 
        failwith "El directorio de resultados no existe."
    let players = FileToList pathFile
    let quantityPlayers = List.length players
    if quantityPlayers % 2 <> 0 then failwith "La cantidad de jugadores debe ser un numero par."
    // ========Algorithm===========
    let weighedPlayers = players|>List.map ((ValuingPlayer quantityPlayers))  |> List.sortBy (fun (player) -> player.Weight)
    let totalWeight = weighedPlayers |> List.fold (fun acc player->acc+player.Weight) 0 
    let team1, team2 = List.partition (fun (player) -> player.Weight>= (totalWeight / quantityPlayers)) weighedPlayers
    let ultimateTeam1, ultimateTeam2 = EqualizeTeams team1 team2
    //==========Presentation================
    let team1AsText = String.Format("Equipo1:[{0}]", (listToText ultimateTeam1))
    let team2AsText = String.Format("Equipo2:[{0}]", (listToText ultimateTeam2))
    let contentFinal = 
        (new System.Text.StringBuilder()).AppendLine(team1AsText).AppendLine(",").AppendLine(team2AsText).ToString()
    printfn "Numero de players: %A" quantityPlayers
    printfn "%A" contentFinal
    File.WriteAllText(pathResult, contentFinal)
    0
