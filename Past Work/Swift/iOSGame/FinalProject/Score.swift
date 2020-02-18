//
//  Score.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/17/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

class Score: Comparable
{
    /// Returns a Boolean value indicating whether the value of the first
    /// argument is less than that of the second argument.
    ///
    /// This function is the only requirement of the `Comparable` protocol. The
    /// remainder of the relational operator functions are implemented by the
    /// standard library for any type that conforms to `Comparable`.
    ///
    /// - Parameters:
    ///   - lhs: A value to compare.
    ///   - rhs: Another value to compare.
    public static func <(lhs: Score, rhs: Score) -> Bool
    {
        if lhs.score < rhs.score {
            return true }
        else if lhs.score > rhs.score {
            return false }
        else {
            if lhs.name <= rhs.name {
                return true }
            else {
                return false }
        }
    }
    
    public static func ==(lhs: Score, rhs: Score) -> Bool
    {
        if lhs.name == rhs.name && lhs.score == rhs.score {
            return true
        }
        else {
            return false
        }
    }

    let name: String
    let score: Int
    
    init(player: String, playerScore: Int)
    {
        name = player
        score = playerScore
    }
}
