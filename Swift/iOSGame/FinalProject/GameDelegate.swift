//
//  GameDelegate.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/13/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

protocol GameDelegate {
    func fire(bullet: Bullet)
    func generateEnemy(path: [Path])
    func generateAsteroid(path: [Path])
    func endLevel()
    func movePlayer(movement: ShipMovementEnum)
}
