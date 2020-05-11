//
//  Enemy.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/11/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class Enemy: GameObject
{
    private var _hp: Int
    
    // should be 3xX array. [xCoord,
    private var _movePath: [Path]
    private var _currentPath: Int = 0
    private var _scoreWorth: Int // may be used later if different enemies have different worths
    private var _xCoord: Float
    private var _yCoord: Float
    private var _speed: Float = 1.0
    private let _damage: Int = 10
    
    var delegate: GameDelegate?
    var xCoord: Float { get { return _xCoord } }
    var yCoord: Float { get { return _yCoord } }
    var damage: Int { get { return _damage } }
    var scoreWorth: Int { return _scoreWorth }
    
    static var getImage1: UIImage { return #imageLiteral(resourceName: "enemyShip") }
    static var getImage2: UIImage { return #imageLiteral(resourceName: "explosion_enemy")}
    
    static var quad: [Float] = [
        0.2, -0.2,
        1.0, 0.0, 0.0, 1.0,
        1.0, 1.0,
        
        -0.2, -0.2,
        0.0, 1.0, 0.0, 1.0,
        0.0, 1.0,
        
        0.2, 0.2,
        0.0, 0.0, 1.0, 1.0,
        1.0, 0.0,
        
        -0.2, 0.2,
        1.0, 0.5, 0.0, 1.0,
        0.0, 0.0
    ]
    
    init(hp: Int, path: [Path], x: Float, y: Float)
    {
        _hp = hp
        _movePath = path
        _xCoord = x
        _yCoord = y
        
        _scoreWorth = 10
        
        if path[0]._fire == true {
            fire() }
    }
    
    // returns true if ship destroyed
    func takeDamage() -> Bool
    {
        _hp -= 1
        if (_hp <= 0) {
            return true }
        
        return false
    }
    
    func move(time: Float)
    {
        // move towards current path
        let x2 = _movePath[_currentPath]._x
        let y2 = _movePath[_currentPath]._y
        let dist: Float = distance(startX: xCoord, endX: x2, startY: yCoord, endY: y2)
        
        // if > current path, update current to the next path
        if (dist <= 0.06)
        {
            _currentPath += 1
            if _movePath[_currentPath]._fire == true {
                fire() }
        }
        else
        {
            let point: Point = moveObject(startX: xCoord, endX: x2, startY: yCoord, endY: y2, time: time.divided(by: 2), speed: _speed, dist: dist)
            _xCoord += point.x
            _yCoord += point.y
        }
    }
    
    // fires a bullet
    func fire()
    {
        let bullet: Bullet = Bullet(playerBullet: false, xCoord: _xCoord, yCoord: _yCoord)
        delegate?.fire(bullet: bullet)
    }
}
