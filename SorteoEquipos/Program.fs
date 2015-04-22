///Claudio E.Gonzalez Vera
open System
open System.IO


let rand = new Random(System.DateTime.Now.Day + System.DateTime.Now.Minute)

let AssignWeightToPlayer  maxPlayers player = 
    let weight = rand.Next(1, maxPlayers)
    weight, player

let EqualizeTeams (e1 : List<int * string>) (e2 : List<int * string>) = 
    let BalanceNow (menor : List<int * string>) (mayor : List<int * string>) = 
        let rec run (menor1 : List<int * string>) (mayor1 : List<int * string>) = 
            if menor1.Length < mayor1.Length then 
                let nuevoMenor = menor1 @ [ mayor1.Head ]
                let nuevoMayor = mayor1.Tail
                let min, may = run nuevoMenor nuevoMayor
                min, may
            else menor1, mayor1
        run menor mayor
    if e1.Length < e2.Length then 
        let r1, r2 = BalanceNow e1 e2
        r1, r2
    else if e1.Length > e2.Length then 
        let r1, r2 = BalanceNow e2 e1
        r1, r2
    else e1, e2

let FileToList pathFile=
   let contentPlayers = File.ReadAllText(pathFile)
   let players = Array.toList (contentPlayers.Split(',')) |> List.map (fun s -> s.Trim())
   players

[<EntryPoint>]
let main argv = 


    if argv.Length < 2 then 
        failwith "Faltan los parametros: archivo_jugadores y archivo_resultado."
    let pathFile = argv.[0]
    let pathResult = argv.[1]

    if File.Exists(pathFile) = false then failwith "El archivo de players no se encuentra en el directorio indicado."
    if Directory.Exists(Path.GetDirectoryName(pathResult)) = false then 
        failwith "El directorio de resultados no existe."
 
    let players = FileToList pathFile
    let maxPlayers = List.length players

    
    let listWithPeso = List.map ((AssignWeightToPlayer maxPlayers)) players |> List.sortBy (fun (value, n) -> value)
    let pesoTotal, result = listWithPeso |> List.reduce (fun (peso1, name1) (peso2, name2) -> peso1 + peso2, "")
    let team1, team2 = List.partition (fun (value, n) -> value >= (pesoTotal / maxPlayers)) listWithPeso
    
    let teamFinal1, teamFinal2 = EqualizeTeams team1 team2
    let convertList (list : List<int * string>) = 
        List.reduce (fun (peso1, name1) (peso2, name2) -> 0, name1 + ", " + name2) list |> (fun (p, n) -> n)

    let eq1Text = String.Format("Equipo1:[{0}]", (convertList teamFinal1))
    let eq2Text = String.Format("Equipo2:[{0}]", (convertList teamFinal2))

    let contentFinal = 
        (new System.Text.StringBuilder()).AppendLine(eq1Text).AppendLine(",").AppendLine(eq2Text).ToString()

    printfn "Numero de players: %A" maxPlayers
    printfn "%A" contentFinal

    File.WriteAllText(pathResult, contentFinal)
    0 // return an integer exit code
