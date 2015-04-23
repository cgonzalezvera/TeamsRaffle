module Calculations

open System
open System.IO

let rand = new Random(System.DateTime.Now.Day + System.DateTime.Now.Minute)

let AssignWeightToPlayer maxPlayers player = 
    let weight = rand.Next(1, maxPlayers)
    weight, player

let EqualizeTeams (team1 : List<int * string>) (team2 : List<int * string>) = 
    let BalanceNow (teamSmall : List<int * string>) (teamLarge : List<int * string>) = 
        let rec transferPlayers (small : List<int * string>) (large : List<int * string>) = 
            if small.Length < large.Length then 
                let stillSmall = small @ [ large.Head ]
                let stillLarge = large.Tail
                let min, may = transferPlayers stillSmall stillLarge
                min, may
            else small, large
        transferPlayers teamSmall teamLarge
    if team1.Length < team2.Length then 
        let r1, r2 = BalanceNow team1 team2
        r1, r2
    else if team1.Length > team2.Length then 
        let r1, r2 = BalanceNow team2 team1
        r1, r2
    else team1, team2

let FileToList pathFile = 
    let contentPlayers = File.ReadAllText(pathFile)
    let players = Array.toList (contentPlayers.Split(',')) |> List.map (fun s -> s.Trim())
    players

let convertList (list : List<int * string>) = 
    List.reduce (fun (peso1, name1) (peso2, name2) -> 0, name1 + ", " + name2) list |> (fun (p, n) -> n)
