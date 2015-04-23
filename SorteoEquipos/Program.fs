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
    let weighedPlayers = 
        List.map ((AssignWeightToPlayer quantityPlayers)) players |> List.sortBy (fun (value, n) -> value)
    let totalWeight, result = weighedPlayers |> List.reduce (fun (peso1, name1) (peso2, name2) -> peso1 + peso2, "")
    let team1, team2 = List.partition (fun (weigh, n) -> weigh >= (totalWeight / quantityPlayers)) weighedPlayers
    let ultimateTeam1, ultimateTeam2 = EqualizeTeams team1 team2
    //==========Presentation================
    let eq1Text = String.Format("Equipo1:[{0}]", (convertList ultimateTeam1))
    let eq2Text = String.Format("Equipo2:[{0}]", (convertList ultimateTeam2))
    let contentFinal = 
        (new System.Text.StringBuilder()).AppendLine(eq1Text).AppendLine(",").AppendLine(eq2Text).ToString()
    printfn "Numero de players: %A" quantityPlayers
    printfn "%A" contentFinal
    File.WriteAllText(pathResult, contentFinal)
    0
