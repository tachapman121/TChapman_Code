//
//  GameOver.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/30/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class GameOverView: UIView, UITextFieldDelegate
{
    private var _gameOver: UILabel?
    private var _scoreLabel: UILabel?
    private var _textField: UITextField?
    private var _enterInitials: UILabel?
    private var scores: [Score] = loadScores()
    
    var delegate: MenuDelegate?
    var _score: Int?
    
    override init(frame: CGRect)
    {
        super.init(frame: frame)
        
        _gameOver = UILabel()
        _gameOver?.text = "Game Over!"
        
        backgroundColor = UIColor.black
        
        _score = 0
        _scoreLabel = UILabel()
        _scoreLabel?.text = "Score: \(_score!)"
        
        _enterInitials = UILabel()
        _enterInitials?.text = "Enter Initials"
        
        _textField = UITextField()
        _textField!.returnKeyType = UIReturnKeyType.done
        _textField?.delegate = self
        
        addSubview(_gameOver!)
        addSubview(_scoreLabel!)
        addSubview(_textField!)
        addSubview(_enterInitials!)
        
        // read enter from keyboard
        becomeFirstResponder()
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    override func draw(_ rect: CGRect)
    {
        _gameOver?.textColor = UIColor.red
        _scoreLabel?.textColor = UIColor.red
        _enterInitials?.textColor = UIColor.red
        _textField?.backgroundColor = UIColor.white
        
        // only able to enter score if greater than high score
        if (_score! < (scores.last?.score)!)
        {
            _textField?.isEnabled = false
            _textField?.isHidden = true
            _enterInitials?.isHidden = true
        }
        else
        {
            _textField?.isEnabled = true
            _textField?.isHidden = false
            _enterInitials?.isHidden = false
        }
        
        _scoreLabel?.text = "Score: \(_score!)"
        
        _gameOver!.frame = CGRect(x: bounds.width.multiplied(by: 0.3), y: bounds.height.multiplied(by: 0.1), width: bounds.width.multiplied(by: 0.3), height: bounds.height.multiplied(by: 0.05))
        _scoreLabel!.frame = _gameOver!.frame.offsetBy(dx: 0.0, dy: bounds.height.multiplied(by: 0.1))
        _enterInitials?.frame = _scoreLabel!.frame.offsetBy(dx: 0.0, dy: bounds.height.multiplied(by: 0.1))
        _textField!.frame = _enterInitials!.frame.offsetBy(dx: 0.0, dy: bounds.height.multiplied(by: 0.1))
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?)
    {
        // If no score needed, exit
        if (_score! <= (scores.last?.score)!) {
            delegate?.highScore() }
    }
    
    // read DONE key from keyboard
    func textFieldShouldReturn(_ textField: UITextField) -> Bool
    {
        if (_score! > (scores.last?.score)!)
        {
            // pop up menu to enter name
            saveScores(score: Score(player: _textField!.text!, playerScore: _score!))
            delegate?.highScore()
        }
        
        return true
    }
}
