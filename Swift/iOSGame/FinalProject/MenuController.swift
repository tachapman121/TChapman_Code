//
//  MenuController.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/17/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class MenuController: UIViewController, MenuDelegate
{
    var menuView: MenuView?
    var highScoreView: HighScoreView?
    var gameOverView: GameOverView?
    var gameView: GameView?

    override func loadView()
    {
        menuView = MenuView(frame: CGRect.zero)
        highScoreView = HighScoreView(frame: CGRect.zero)
        gameOverView = GameOverView(frame: CGRect.zero)
        gameView = GameView()
        
        gameOverView?._score = 20
        view = menuView
    }
    
    override func viewDidLoad()
    {
        menuView!.delegate = self
        highScoreView!.delegate = self
        gameOverView!.delegate = self
        gameView!.parentDelegate = self
    }
    
    override func viewWillAppear(_ animated: Bool)
    {
        view.setNeedsDisplay()
    }
    
    internal func highScore()
    {
        view = highScoreView
    }
    
    internal func mainMenu()
    {
        view = menuView
    }
    
    internal func newGame()
    {
        gameView?.reset()
        present(gameView!, animated: false, completion: nil)
    }
    
    internal func continueGame()
    {
        return
    }
    
    func saveScore(score: Score)
    {
        saveScores(score: score)
    }
    
    func gameOver(score: Int)
    {
        gameOverView?._score = score
        view = gameOverView
    }
}
