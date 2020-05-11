//
//  Asteroid.swift
//  FinalProject
//
//  Created by Trevor Chapman on 5/1/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//


import UIKit

class Asteroid: GameObject
{
    // determines collision type to prevent enemy shooting enemy
    private var _xCoord: Float
    private var _yCoord: Float
    private var _speed: Float
    private var image: UIImage!
    private let _damage: Int = 5
    
    var xCoord: Float { get { return _xCoord } }
    var yCoord: Float { get { return _yCoord } }
    var damage: Int { get { return _damage } }
    
    var delegate: GameDelegate?
    
    static var getImage1: UIImage { return #imageLiteral(resourceName: "asteroidImg") }
    static var getImage2: UIImage { return #imageLiteral(resourceName: "asteroidImg") }
    
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
    
    init(xCoord: Float, yCoord: Float)
    {
        _xCoord = xCoord
        _yCoord = yCoord
        
        _speed = 1.0
    }
    
    // Move the bullet; at the moment only goes vertical
    func move(time: Float)
    {
        // move towards top of screen
        let x2: Float = _xCoord
        let y2: Float = -2.0
        let dist: Float = distance(startX: xCoord, endX: x2, startY: yCoord, endY: y2)
        
        // if > current path, update current to the next path
        let point: Point = moveObject(startX: xCoord, endX: x2, startY: yCoord, endY: y2, time: time.divided(by: 7), speed: 1.0, dist: dist)
        _xCoord += point.x
        _yCoord += point.y
    }
    
    internal func imageName() -> UIImage
    {
        return image
    }
}
