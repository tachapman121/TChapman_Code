//
//  Level.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/25/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

protocol Level
{
    var time: Float { get }
    func update(time: Float)
    func reset()
    var delegate: GameDelegate { get set }
    // should also have function to generate paths but won't let me put private func in protocols
}
