//
//  Background.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/28/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

class Background: GameObject
{
    internal static var getImage1: UIImage { return #imageLiteral(resourceName: "background") }
    static var getImage2: UIImage { return #imageLiteral(resourceName: "background") }
    
    internal var xCoord: Float = 0.0
    internal var yCoord: Float = 0.0

    static var quad: [Float] = [
        1.0, -1.0,
        1.0, 0.0, 0.0, 1.0,
        1.0, 1.0,
        
        -1.0, -1.0,
        0.0, 1.0, 0.0, 1.0,
        0.0, 1.0,
        
        1.0, 1.0,
        0.0, 0.0, 1.0, 1.0,
        1.0, 0.0,
        
        -1.0, 1.0,
        1.0, 0.5, 0.0, 1.0,
        0.0, 0.0
    ]
}
