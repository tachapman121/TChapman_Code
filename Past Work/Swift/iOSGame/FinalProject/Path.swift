//
//  Path.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/25/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.

import Foundation

// small object to be used in paths
class Path
{
    let _x: Float
    let _y: Float
    let _fire: Bool
    
    init(x: Float, y: Float, fire: Bool)
    {
        _x = x
        _y = y
        _fire = fire
    }
    
    // initial path, random X location and starts at top of the screen
    static func initialPath() -> Path
    {
        var randX = Int(arc4random())
        randX = randX % 200
        randX -= 100
        
        var floatRand = Float(randX)
        floatRand = floatRand.divided(by: 100)
        
        return Path(x: floatRand, y: 1.0, fire: false)
    }
    
    // final path, goes way up to force deletion
    static func finalPath() -> Path
    {
        return Path(x: 3.0, y: 3.0, fire: false)
    }
}
