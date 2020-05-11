//
//  Load:Save.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/18/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation
let saveFile: String = "/Scores.json"

/* Loads scores from library, or returns 5  */
func loadScores() -> [Score]
{
    var scores: [Score] = []
    
    let jsonData: Data? = try? Data(contentsOf: URL.init(fileURLWithPath: documentDirectory() + "\(saveFile)"))
    
    // reads games if data not nil/was found
    if (jsonData != nil)
    {
        let scoreDictionary: NSDictionary = try! JSONSerialization.jsonObject(with: jsonData!, options: []) as! NSDictionary
        
        for score in scoreDictionary {
            scores.append(Score(player: score.key as! String, playerScore: score.value as! Int)) }
    }
    // for first run, read scores and save so min of 5
    else
    {
        scores.append(Score(player: "AAAAA", playerScore: 10))
        scores.append(Score(player: "BBBBB", playerScore: 10))
        scores.append(Score(player: "CCCCC", playerScore: 10))
        scores.append(Score(player: "DDDDD", playerScore: 10))
        scores.append(Score(player: "EEEEE", playerScore: 10))
    }
    
    return scores
}

func saveScores(score: Score)
{
    var scores: [Score] = loadScores()
    scores.append(score)
    scores.sort(by: inverse)
    scores.removeLast()
    
    let dictionaries: NSMutableDictionary = [:]
    for score in scores {
        dictionaries.addEntries(from: [score.name : score.score]) }
    
    let jsonData: Data = try! JSONSerialization.data(withJSONObject: dictionaries, options: .prettyPrinted)
    try! jsonData.write(to: URL.init(fileURLWithPath: documentDirectory() + "\(saveFile)"))
}

// code taken from http://theapplady.net/accessing-the-app-documents-folder-the-swift-way/
private func documentDirectory() -> String
{
    let paths = NSSearchPathForDirectoriesInDomains(.documentDirectory, .userDomainMask, true) as NSArray
    let directory = paths.firstObject as! String
    return directory
}

private func inverse(score1: Score, score2: Score) -> Bool
{
    return score1 >= score2
}
