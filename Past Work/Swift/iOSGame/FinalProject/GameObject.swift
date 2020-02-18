//
//  GameObject.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/23/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import UIKit

// Game Objects: Ships, bullets, asteroids, etc

/***** TO ADD A NEW OBJECT:
 1. Create new file using GameObject protocol
 2. Create new dictionary key string in Misc.swift
 3. Modify GameDelegate with new generate___() method if needed
 3. Modify Game.swift
    a. Create new Array for objects
    b. Update itemsToDelete global variable at top of file
    c. Modify itemsToDelete dict to include new item keyed off the string in Misc.swift
    d. Modify getObjects() to return new dict array
    e. Update moveObjects() with new objects
    f. Update checkCollisions() with new objects as needed
    g. If modified GameDelegate with generate, add code there to make object and appened to the new Array
 4. Update GameView.swift
    a. Add global sprite and [Sprite] as needed
    b. Add new sprite in viewDidLoad
    c. Modify update to get objects from dictionary
    d. Modify update to add sprites to _sprites as needed 
 5. Update levels to generate objects   ******/
protocol GameObject {
    // should be from -1.0 to 1.0
    var xCoord: Float { get }
    var yCoord: Float { get }
    
    // image to draw it; should convert to use array at some point
    static var getImage1: UIImage { get }
    static var getImage2: UIImage { get }
    
    // spcae to draw in
    static var quad: [Float] { get }
}
