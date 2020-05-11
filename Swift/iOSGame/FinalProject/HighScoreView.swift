//
//  HighScore.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/17/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class HighScoreView: UIView
{
    private let numScores = 5
    
    private var nameLabel: [UILabel] = []
    private var scoreLabel: [UILabel] = []
    private var backButton: UIButton?
    
    var delegate: MenuDelegate?
    
    override init(frame: CGRect)
    {
        super.init(frame: frame)
        
        for index in 0...4 {
            nameLabel.append(UILabel())
            scoreLabel.append(UILabel())
            
            addSubview(nameLabel[index])
            addSubview(scoreLabel[index])
        }
        
        backButton = UIButton()
        
        backButton?.addTarget(self, action: #selector(backSelected), for: .touchDown)
        addSubview(backButton!)
        
        backgroundColor = UIColor.blue
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    override func draw(_ rect: CGRect)
    {
        let scores: [Score] = loadScores()
        
        //0.1 - 0.15
        nameLabel[0].frame = CGRect(x: bounds.width.multiplied(by: 0.1), y: bounds.height.multiplied(by: 0.1), width: bounds.width.multiplied(by: 0.3), height: bounds.height.multiplied(by: 0.05))
        scoreLabel[0].frame = nameLabel[0].frame.offsetBy(dx: nameLabel[0].frame.width + bounds.height.multiplied(by: 0.2), dy: 0.0)
        
        nameLabel[0].text = scores[0].name
        scoreLabel[0].text = scores[0].score.description
        
        for index in 1...numScores-1 {
            nameLabel[index].text = scores[index].name
            scoreLabel[index].text = scores[index].score.description
            
            nameLabel[index].frame = nameLabel[index-1].frame.offsetBy(dx: 0.0, dy: nameLabel[index-1].frame.height + bounds.height.multiplied(by: 0.1))
            scoreLabel[index].frame = scoreLabel[index-1].frame.offsetBy(dx: 0.0, dy: scoreLabel[index-1].frame.height + bounds.height.multiplied(by: 0.1))
        }
        
        backButton?.frame = CGRect(x: bounds.width.multiplied(by: 0.4), y: bounds.height.multiplied(by: 0.9), width: bounds.width.multiplied(by: 0.2), height: bounds.height.multiplied(by: 0.05))
        backButton?.setTitle("BACK", for: .normal)
        backButton?.backgroundColor = UIColor.brown
    }
    
    func backSelected()
    {
        delegate?.mainMenu()
    }
}
