//
//  MenuView.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/17/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class MenuView: UIView
{
    var _newGame: UIButton?
    var _loadGame: UIButton?
    var _highScore: UIButton?
    var _title: UIImage?
    
    var delegate: MenuDelegate?
    
    override init(frame: CGRect)
    {
        super.init(frame: frame)
        
        _newGame = UIButton()
        _loadGame = UIButton()
        _highScore = UIButton()
        
        _newGame?.addTarget(self, action: #selector(newGame), for: .touchDown)
        _loadGame?.addTarget(self, action: #selector(continueGame), for: .touchDown)
        _highScore?.addTarget(self, action: #selector(highScores), for: .touchDown)
        _title = UIImage(cgImage: #imageLiteral(resourceName: "game_title").cgImage!)
        
        addSubview(_newGame!)
        addSubview(_loadGame!)
        addSubview(_highScore!)
        
        backgroundColor = UIColor.black
        
        return
    }
    
    override func draw(_ rect: CGRect)
    {
        let rect: CGRect = CGRect(x: 0.0, y: bounds.height.multiplied(by: 0.05), width: bounds.width, height: bounds.height.multiplied(by: 0.05))
        _title?.draw(in: rect)
        
        _newGame?.frame = CGRect(x: bounds.width.multiplied(by: 0.15), y: bounds.height.multiplied(by: 0.2), width: bounds.width.multiplied(by: 0.666), height: bounds.height.multiplied(by: 0.1))
        _highScore?.frame = _newGame!.frame.offsetBy(dx: 0.0, dy: _newGame!.frame.height + bounds.height.multiplied(by: 0.1))
        // _loadGame?.frame = _newGame!.frame.offsetBy(dx: 0.0, dy: _newGame!.frame.height + bounds.height.multiplied(by: 0.1))
        
        
        _newGame?.backgroundColor = UIColor.red
        _loadGame?.backgroundColor = UIColor.red
        _highScore?.backgroundColor = UIColor.red
        
        _newGame?.setTitle("New Game", for: .normal)
        _loadGame?.setTitle("Continue Game", for: .normal)
        _highScore?.setTitle("High Scores", for: .normal)
        
        // NOT WORKING RIGHT NOW, SHOULD FIX AT SOME POINT? AND
        _loadGame?.isHidden = true
        _loadGame?.isEnabled = false
    }
    
    func newGame()
    {
        delegate?.newGame()
    }
    
    func continueGame()
    {
        delegate?.continueGame()
    }
    
    func highScores()
    {
        delegate?.highScore()
    }
    
    // needed for init
    required init?(coder aDecoder: NSCoder)
    {
        fatalError("init(coder:) has not been implemented")
    }
}
