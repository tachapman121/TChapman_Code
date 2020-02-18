//
//  Bullet.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/11/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class Bullet: GameObject
{
    // determines collision type to prevent enemy shooting enemy
    private let _playerBullet: Bool?
    private var _xCoord: Float
    private var _yCoord: Float
    private var _speed: Float = 1.0
    private var image: UIImage!
    private let _damage: Int = 10
    
    var xCoord: Float { get { return _xCoord } }
    var yCoord: Float { get { return _yCoord } }
    var isPlayers: Bool? { get { return _playerBullet } }
    var damage: Int { get { return _damage } }
    
    static var getImage1: UIImage { return #imageLiteral(resourceName: "bullet_enemy") }
    static var getImage2: UIImage { return #imageLiteral(resourceName: "bullet_player") }
    
    static var quad: [Float] = [
        0.1, -0.1,
        1.0, 0.0, 0.0, 1.0,
        1.0, 1.0,
        
        -0.1, -0.1,
        0.0, 1.0, 0.0, 1.0,
        0.0, 1.0,
        
        0.1, 0.1,
        0.0, 0.0, 1.0, 1.0,
        1.0, 0.0,
        
        -0.1, 0.1,
        1.0, 0.5, 0.0, 1.0,
        0.0, 0.0
    ]
    
    init(playerBullet: Bool, xCoord: Float, yCoord: Float)
    {
        _playerBullet = playerBullet
        _xCoord = xCoord
        _yCoord = yCoord
        _speed = 1.0
    }
    
    // Move the bullet; at the moment only goes vertical
    func move(time: Float)
    {
        // move towards top of screen
        let x2: Float = _xCoord
        let y2: Float
        if (isPlayers!) {
            y2 = 2.0 }
        else {
            y2 = -2.0 }
        
        let dist: Float = distance(startX: xCoord, endX: x2, startY: yCoord, endY: y2)
        
        // if > current path, update current to the next path
        let point: Point = moveObject(startX: xCoord, endX: x2, startY: yCoord, endY: y2, time: time, speed: _speed, dist: dist)
        _xCoord += point.x
        _yCoord += point.y
    }
    
    internal func imageName() -> UIImage
    {
        return image
    }
}
