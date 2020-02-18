//
//  GameDelegate.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/11/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

protocol GameControllerDelegate {
    func GameOver(score: Int)
    func fire(bullet: Bullet)
    func movePlayer(playerMovement: ShipMovementEnum)
    func getObjects() -> [String : [GameObject]]
}
