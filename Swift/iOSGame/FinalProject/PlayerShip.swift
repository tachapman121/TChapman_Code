//
//  PlayerShip.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/11/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class PlayerShip: GameObject
{
    // HP of ship
    private var _hp: Int
    
    // coordinates
    private var _xCoord: Float
    private var _yCoord: Float // change once figure out where it should go
    private var _speed: Float = 1
    private var _maxBullets: Int = 3
    
    static var getImage1: UIImage { return #imageLiteral(resourceName: "playerShip") }
    static var getImage2: UIImage { return #imageLiteral(resourceName: "explosion_player") }
    
    
    var bulletsFiring: Int = 0
    var maxBullets: Int { get { return _maxBullets } }
    
    var xCoord: Float { get { return _xCoord } }
    var yCoord: Float { get { return _yCoord } }
    static var quad: [Float] = [
        0.3, -0.3,
        1.0, 0.0, 0.0, 1.0,
        1.0, 1.0,
        
        -0.3, -0.3,
        0.0, 1.0, 0.0, 1.0,
        0.0, 1.0,
        
        0.3, 0.3,
        0.0, 0.0, 1.0, 1.0,
        1.0, 0.0,
        
        -0.3, 0.3,
        1.0, 0.5, 0.0, 1.0,
        0.0, 0.0
    ]
    
    init()
    {
        _hp = 10
        _xCoord = 0
        _yCoord = -0.8
    }
    
    // continuing game
    init(hp: Int, xCoord: Float, yCoord: Float)
    {
        _hp = hp
        _xCoord = xCoord
        _yCoord = yCoord
    }
    
    // move the ship left
    func strafeLeft(elapsedTime: Float)
    {
        let dist: Float = distance(startX: _xCoord, endX: -1.0, startY: _yCoord, endY: _yCoord)
        let point: Point = moveObject(startX: _xCoord, endX: -1.0, startY: _yCoord, endY: _yCoord, time: elapsedTime, speed: _speed, dist: dist)
        _xCoord += point.x
    }
    
    // move the ship right
    func strafeRight(elapsedTime: Float)
    {
        let dist: Float = distance(startX: _xCoord, endX: 1.0, startY: _yCoord, endY: _yCoord)
        let point: Point = moveObject(startX: _xCoord, endX: 1.0, startY: _yCoord, endY: _yCoord, time: elapsedTime, speed: _speed, dist: dist)
        _xCoord += point.x
    }
    
    func accelerate(elapsedTime: Float)
    {
        let dist: Float = distance(startX: _xCoord, endX: _xCoord, startY: _yCoord, endY: 1.0)
        let point: Point = moveObject(startX: _xCoord, endX: _xCoord, startY: _yCoord, endY: 1.0, time: elapsedTime, speed: _speed, dist: dist)
        _yCoord += point.y
    }
    
    func decelerate(elapsedTime: Float)
    {
        let dist: Float = distance(startX: _xCoord, endX: _xCoord, startY: _yCoord, endY: -0.9)
        let point: Point = moveObject(startX: _xCoord, endX: _xCoord, startY: _yCoord, endY: -0.9, time: elapsedTime, speed: _speed, dist: dist)
        _yCoord += point.y
    }
    
    // generate a player bullet
    func fire() -> Bullet
    {
        let bullet: Bullet = Bullet(playerBullet: true, xCoord: _xCoord, yCoord: _yCoord)
        return bullet
    }
    
    // player hit by enemy or object; returns true if dead and false otherwise
    func takeDamage(amount: Int) -> Bool
    {
        _hp -= amount;
        if (_hp <= 0) {
            return true }
        
        return false
    }
}
