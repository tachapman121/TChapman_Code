//
//  MenuDelegate.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/17/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

protocol MenuDelegate
{
    func mainMenu()
    func highScore()
    func newGame()
    func continueGame()
    
    func gameOver(score: Int)
    func saveScore(score: Score)
}
