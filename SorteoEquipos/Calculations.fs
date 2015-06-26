module Calculations

open System
open System.IO
 
type Player = 
    { Name : string;
      Weight : int ;
      SkillValue: int option ;
    }
    member this.GetSkillValue()=
       let value=this.SkillValue
       match value with
        |None-> 1
        |Some(value) -> value

let rand = new Random(System.DateTime.Now.Day + System.DateTime.Now.Minute)

//This method valuing a player
let ValuingPlayer maxPlayers (player:Player) = 
    
    let factor = rand.Next(1, maxPlayers * System.DateTime.Now.Second)
    let weight = player.GetSkillValue() * factor
    { player with Weight = weight } //"with"-> cloning a object with a property's value different

let EqualizeTeams (team1 : List<Player>) (team2 : List<Player>) = 
    let BalanceNow (teamSmall : List<Player>) (teamLarge : List<Player>) = 
        let rec transferPlayers (small : List<Player>) (large : List<Player>) = 
            if small.Length < large.Length then 
                let stillSmall = small @ [ large.Head ]
                let stillLarge = large.Tail
                let min, may = transferPlayers stillSmall stillLarge
                min, may
            else small, large
        transferPlayers teamSmall teamLarge
    match (team1, team2) with
    | (team1, team2) when team1.Length < team2.Length -> BalanceNow team1 team2
    | (team1, team2) when team1.Length > team2.Length -> BalanceNow team2 team1
    | (team1, team2) -> team1, team2

let FileToList pathFile = 
    let contentPlayers = File.ReadAllText(pathFile)
    //--Get skill value from name
    let getSkillValue (playerName:string)=
        let result=ConfigurationMisc.SeparatePartsTextLine (Some(playerName))
        match result.IsValue with
            |true-> Int32.Parse(result.Value)
            |false ->1
    let getOnlyName (playerName:string)=
        let result=ConfigurationMisc.SeparatePartsTextLine (Some(playerName))
        result.Key
    (*
        The separator between lines in player's file is with "," 
        Example player's file:
         pedro[=19],
         marcos[=10]

         In general, each line: identifier_player  [= fixed weight] ,

    *)
    let players = 
        Array.toList (contentPlayers.Split(',')) |> List.map (fun s -> 
                                                        { Name = getOnlyName(s.Trim())
                                                          Weight = 0
                                                          SkillValue=Some(getSkillValue(s.Trim()))
                                                          })
    players

let listToText (players : List<Player>) = 
    List.map (fun player -> player.Name) players |> List.reduce (fun n1 n2 -> String.Format("{0}, {1}", n1, n2))
