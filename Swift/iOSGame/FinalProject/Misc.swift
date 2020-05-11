//
//  DictionaryDefs.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/25/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

/* Used to keep same keys across files */
let playerDict: String = "player"
let enemyDict: String = "enemy"
let bulletDict: String = "bullet"
let asteroidDict: String = "asteroid"

enum ShipMovementEnum
{
    case accelerate
    case decelerate
    case left
    case right
    case fire
    case none
}

// calculates distance between two points
func distance(startX: Float, endX: Float, startY: Float, endY: Float) -> Float
{
    var x = (endX - startX)
    x *= x
    
    var y = (endY-startY)
    y *= y
    
    return sqrt(x + y)
}

// basic point class for returning two values. Probably Swift-ier way to do so
class Point
{
    var x: Float
    var y: Float
    
    init(_x: Float, _y: Float)
    {
        x = _x
        y = _y
    }
}

// moves two objects, should probably modify to use point class instead
func moveObject(startX: Float, endX: Float, startY: Float, endY: Float, time: Float, speed: Float, dist: Float) -> Point
{
    let distX: Float = (endX - startX).divided(by: dist)
    let distY: Float = (endY - startY).divided(by: dist)
    
    let xCoord = distX * speed * time
    let yCoord = distY * speed * time
    
    return Point(_x: xCoord, _y: yCoord)
}
